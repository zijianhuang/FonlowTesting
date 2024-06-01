﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Fonlow.Testing
{
	/// <summary>
	/// Launch services defined in TestingSettings
	/// </summary>
	public class ServiceCommandsFixture : IDisposable
	{
		/// <summary>
		/// Create the fixture. And this constructor is also used in XUnit.ICollectionFixture.
		/// </summary>
		public ServiceCommandsFixture()
		{
			foreach (var item in TestingSettings.Instance.ServiceCommands)
			{
				var serviceCommandAgent = new ServiceCommandAgent(item);
				serviceCommandAgent.Start();
				serviceCommandAgents.Add(serviceCommandAgent);
			}
		}

		public ServiceCommand[] ServiceCommands => TestingSettings.Instance.ServiceCommands;

		List<ServiceCommandAgent> serviceCommandAgents = new List<ServiceCommandAgent>();

		bool disposed;

		public void Dispose()
		{
			Console.WriteLine("ServiceCommandsFixture is being disposed.");
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					foreach (var agent in serviceCommandAgents)
					{
						if (agent != null)
						{
							agent.Stop();
						}
					}
				}

				disposed = true;
			}
		}

	}

}