using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;

namespace Fonlow.Testing
{
	/// <summary>
	/// For starting and stoping service
	/// </summary>
	public class ServiceCommandAgent
	{
		public ServiceCommandAgent(ServiceCommand serviceCommand)
		{
			this.Command = serviceCommand;
		}

		public ServiceCommand Command { get; private set; }

		/// <summary>
		/// Start DotNet Kestrel Web server.
		/// </summary>
		public void Start()
		{
			if (Command.IsPowerShellCommand)
			{
				try
				{
					using var  ps = PowerShell.Create();
					ps.AddScript(Command.CommandPath);//.AddArgument("c:/temp/auth.db").AddArgument("c:/temp/authGGGGGG.db");
					var rs = ps.Invoke();
					if (ps.HadErrors){
						var errMsg = string.Join(Environment.NewLine,  ps.Streams.Error.Select(d => d.ErrorDetails.ToString()));
						Console.Error.WriteLine(errMsg);
					}
				}
				catch (CommandNotFoundException ex)
				{
					Console.Error.WriteLine(ex);
					throw;
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine(ex);
					throw;
				}
			}
			else
			{
				ProcessStartInfo info;
				var dir = System.IO.Path.GetDirectoryName(Command.CommandPath);
				if (string.IsNullOrEmpty(dir))
				{
					info = new ProcessStartInfo(Command.CommandPath, Command.Arguments)
					{
						UseShellExecute = true,
					};
				}
				else
				{
					string command = System.IO.Path.GetFileName(Command.CommandPath);
					string workingDir = System.IO.Path.GetFullPath(dir);
					info = new ProcessStartInfo(command, Command.Arguments)
					{
						UseShellExecute = true,
						WorkingDirectory = workingDir,
					};

					Console.WriteLine($"Working Dir: {workingDir}; Current Dir: {System.IO.Directory.GetCurrentDirectory()}");
				}

				Console.WriteLine($"Starting {Command.CommandPath} {Command.Arguments} ...");
				process = Process.Start(info);
				timeStart = DateTime.Now;
				Console.WriteLine($"Started: {Command.CommandPath} {Command.Arguments} at {timeStart}");
				System.Threading.Thread.Sleep(this.Command.Delay * 1000);
				timeStart = DateTime.Now;
				Console.WriteLine($"Wait a second: {Command.CommandPath} {Command.Arguments} at {timeStart}");
			}
		}

		DateTime timeStart;

		Process process;

		/// <summary>
		/// Stop service launched by this agent.
		/// </summary>
		public void Stop()
		{
			Console.WriteLine("Stopping ServiceCommandAgent");
			if (process == null)
			{
				Console.WriteLine("ServiceCommandAgent process null.");
				return;
			}

			try
			{
				//sometimes the process exited before the kill. Then the kill may leave a ghost terminal screen. This seems to be the new behavior in Windows 11.
				// This typically happens when the Web service was launched manually, then run the test suite which will lauch the same service using this agent class.
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
