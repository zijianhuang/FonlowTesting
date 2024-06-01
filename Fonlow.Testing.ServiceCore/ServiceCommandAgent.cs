﻿using System;
using System.Diagnostics;

namespace Fonlow.Testing
{
	/// <summary>
	/// For starting and stoping service
	/// </summary>
	public class ServiceCommandAgent
	{
		public ServiceCommandAgent(ServiceCommand serviceCommand)
		{
			this.ServicCommand = serviceCommand;
		}

		public ServiceCommand ServicCommand { get; private set; }

		/// <summary>
		/// Start DotNet Kestrel Web server.
		/// </summary>
		public void Start()
		{
			ProcessStartInfo info = new ProcessStartInfo(ServicCommand.CommandPath, ServicCommand.Arguments)
			{
				UseShellExecute = true,
			};

			Console.WriteLine($"Starting {ServicCommand.CommandPath} {ServicCommand.Arguments} ...");
			process = Process.Start(info);
			timeStart = DateTime.Now;
			Console.WriteLine($"Started: {ServicCommand.CommandPath} {ServicCommand.Arguments} at {timeStart}");
			System.Threading.Thread.Sleep(this.ServicCommand.Delay * 1000);
			timeStart = DateTime.Now;
			Console.WriteLine($"Wait a second: {ServicCommand.CommandPath} {ServicCommand.Arguments} at {timeStart}");
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