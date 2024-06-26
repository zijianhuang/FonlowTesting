﻿using System;
using System.Diagnostics;

namespace Fonlow.Testing
{
	/// <summary>
	/// For starting and stoping IIS Express
	/// </summary>
	/// <remarks>The IIS Express config is at </remarks>
	[Obsolete("In favor of ServiceCommandAgent")]
	public class IisExpressAgentBase
	{
		public IisExpressAgentBase(string hostSite, string hostSiteApplicationPool, string slnName, string slnRoot)
		{
			HostSite = hostSite;
			HostSiteApplicationPool = hostSiteApplicationPool;
			SlnName = slnName;
			SlnRoot = slnRoot;
		}

		public string HostSite { get; private set; }
		public string HostSiteApplicationPool { get; private set; }
		public string SlnRoot { get; private set; }
		public string SlnName { get; private set; }

		public void Start(string arguments)
		{
			ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files (x86)\IIS Express\iisexpress.exe", arguments)
			{
				// WindowStyle= ProcessWindowStyle.Minimized
				UseShellExecute=true,
			};

			process = Process.Start(info);
			Debug.WriteLine($"IIS Express (pid: {process.Id} started with {arguments}");
			timeStart = DateTime.Now;
		}

		/// <summary>
		/// Start IIS Express with settings in AppSettings, while Testing_UseIisExpress must be true.
		/// </summary>
		public void Start()
		{
			Start(IisExpressArguments);
		}

		/// <summary>
		/// Arguments for IisExpress.exe defined in app settings of the app.config.
		/// </summary>
		public string IisExpressArguments
		{
			get
			{
#pragma warning disable CA1065
				if (SlnRoot== "SLN_ROOT_.VS")
				{
					var d = DirFunctions.GetSlnDir(System.IO.Directory.GetCurrentDirectory());
					if (d != null)
					{
						SlnRoot = d.FullName;
					}
					else
					{
						throw new ArgumentException("The .vs folder of the VS 2015 solution does not seem to exist.");
					}
				}

				var appHostConfig = String.IsNullOrEmpty(SlnName) ? System.IO.Path.Combine(SlnRoot, @".vs\config\applicationhost.config"): System.IO.Path.Combine(SlnRoot, $@".vs\{SlnName}\config\applicationhost.config");
				if (!System.IO.File.Exists(appHostConfig))
				{
					throw new ArgumentException("app.config does not contain correct info pointing to applicationhost.config for IIS Express.");
				}
#pragma warning restore CA1065
				var iisStartArguments = String.IsNullOrEmpty(SlnRoot) ? String.Format("/site:\"{0}\" /apppool:\"{1}\"", HostSite, HostSiteApplicationPool)
					: String.Format("/site:\"{0}\" /apppool:\"{1}\" /config:\"{2}\"", HostSite, HostSiteApplicationPool, appHostConfig);
				return iisStartArguments;
			}
		}

		DateTime timeStart;

		Process process;

		/// <summary>
		/// Stop IIS Express along with the Website launched by this class.
		/// </summary>
		public void Stop()
		{
			if (process == null)
				return;

			try
			{
				//sometimes the process exited before the kill. Then the kill may leave a ghost terminal screen. This seems to be the new behavior in Windows 11.
				if (!process.HasExited)
				{
					process.Kill(true);
				}
			}
			catch (Exception ex) when (ex is System.ComponentModel.Win32Exception || ex is InvalidOperationException || ex is NotSupportedException)
			{
				Trace.TraceWarning(ex.Message);
			}
		}
	}
}
