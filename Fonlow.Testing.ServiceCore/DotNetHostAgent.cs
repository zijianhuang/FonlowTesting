using System;
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
				ProcessStartInfo info = new ProcessStartInfo("dotnet", TestingSettings.Instance.DotNetServiceAssemblyPath)
				{
					UseShellExecute = true,
				};

				Console.WriteLine($"Starting dotnet {TestingSettings.Instance.DotNetServiceAssemblyPath} ...");
				process = Process.Start(info);
				timeStart = DateTime.Now;
				Console.WriteLine($"Started: dotnet {TestingSettings.Instance.DotNetServiceAssemblyPath} at {timeStart}");
				System.Threading.Thread.Sleep(5000);
				timeStart = DateTime.Now;
				Console.WriteLine($"Wait a second: dotnet {TestingSettings.Instance.DotNetServiceAssemblyPath} at {timeStart}");

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
			catch (Exception ex) when (ex is System.ComponentModel.Win32Exception || ex is InvalidOperationException || ex is NotSupportedException)
			{
				Console.Error.WriteLine("Error: " + ex.Message);
			}
		}
	}
}
