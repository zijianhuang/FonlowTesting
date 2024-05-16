﻿using System;
using System.Diagnostics;
using Xunit.Abstractions;

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
				ProcessStartInfo info = new ProcessStartInfo("dotnet", fileName)
				{
					UseShellExecute = true,
				};

				Console.WriteLine($"Starting dotnet {fileName} ...");
				process = Process.Start(info);
				timeStart = DateTime.Now;
				Console.WriteLine($"Started: dotnet {fileName} at {timeStart}");
			}
		}

		DateTime timeStart;

		Process process;

		/// <summary>
		/// Stop Kestrel launched by this agent.
		/// </summary>
		public void Stop()
		{
			Console.WriteLine("Stopping DotNetHostAgent");
			if (process == null)
			{
				Console.WriteLine("DotNetHostAgent process null.");
				return;
			}

			try
			{
				//sometimes the process exited before the kill. Then the kill may leave a ghost terminal screen. This seems to be the new behavior in Windows 11.
				// This typically happens when I launch the Web service manually, then run the test suite which will lauch the same service using this agent class.
				if (!process.HasExited)
				{
					process.Kill(true);
					Console.WriteLine($"Stopped successfully: {process.StartInfo.Arguments}");
				}
			}
			catch (System.ComponentModel.Win32Exception e)
			{
				Console.Error.WriteLine("Error: " + e.Message);
			}
			catch (InvalidOperationException e)
			{
				Console.Error.WriteLine("Error: " + e.Message);
			}
		}
	}
}
