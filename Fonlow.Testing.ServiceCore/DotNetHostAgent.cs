using System;
using System.Diagnostics;

namespace Fonlow.Testing
{
	/// <summary>
	/// For starting and stoping IIS Express
	/// </summary>
	/// <remarks>The IIS Express config is at </remarks>
	public class DotNetHostAgent
	{
		/// <summary>
		/// Start IIS Express with settings in AppSettings, while Testing_UseIisExpress must be true.
		/// </summary>
		public void Start()
		{
			if (!String.IsNullOrWhiteSpace(TestingSettings.Instance.DotNetServiceAssemblyPath))
			{
				ProcessStartInfo info = new ProcessStartInfo("dotnet.exe", TestingSettings.Instance.DotNetServiceAssemblyPath)
				{
					// WindowStyle= ProcessWindowStyle.Minimized
					UseShellExecute = true,
				};

				process = Process.Start(info);
				timeStart = DateTime.Now;
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
				var span = (DateTime.Now - timeStart).TotalSeconds;
				process.CloseMainWindow();//More graceful than Kill.
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
