﻿using System;
using System.Diagnostics;

namespace Fonlow.Testing
{
	/// <summary>
	/// For starting and stoping DotNet Kestrel Web server.
	/// </summary>
	public class DotNetHostAgent
	{
		/// <summary>
		/// Start DotNet Kestrel Web server.
		/// </summary>
		public void Start()
		{
			if (!String.IsNullOrWhiteSpace(TestingSettings.Instance.DotNetServiceAssemblyPath))
			{
				var workingDirectory = System.IO.Path.GetDirectoryName(TestingSettings.Instance.DotNetServiceAssemblyPath);
				System.IO.Directory.SetCurrentDirectory(workingDirectory); // setting ProcessStartInfo.WorkingDirectory is not always working. Working in this demo, but not working in other heavier .net core Web app.
				var fileName = System.IO.Path.GetFileName(TestingSettings.Instance.DotNetServiceAssemblyPath);
				ProcessStartInfo info = new ProcessStartInfo("dotnet.exe", fileName)
				{
					UseShellExecute = true,
				};

				process = Process.Start(info);
				timeStart = DateTime.Now;
			}
		}

		DateTime timeStart;

		Process process;

		/// <summary>
		/// Stop Kestrel launched by this agent.
		/// </summary>
		public void Stop()
		{
			if (process == null)
				return;

			try
			{
				var span = (DateTime.Now - timeStart).TotalSeconds;
				process.Kill();
			}
			catch (System.ComponentModel.Win32Exception e)
			{
				Trace.TraceWarning(e.Message);
			}
			catch (InvalidOperationException e)
			{
				Trace.TraceWarning(e.Message);
			}
		}
	}
}
