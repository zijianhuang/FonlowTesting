using System;
using System.Collections.Generic;

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
			if (TestingSettings.Instance.ServiceCommands != null)
			{
				foreach (var key in TestingSettings.Instance.ServiceCommands.Keys)
				{
					var item = TestingSettings.Instance.ServiceCommands[key];
					item.Arguments = item.Arguments?.Replace("{BuildConfiguration}", TestingSettings.Instance.BuildConfiguration)
						.Replace("{ExecutableExt}", TestingSettings.Instance.ExecutableExt)
						.Replace("{RuntimeId}", TestingSettings.Instance.RuntimeId);
					item.CommandPath = item.CommandPath?.Replace("{BuildConfiguration}", TestingSettings.Instance.BuildConfiguration)
						.Replace("{ExecutableExt}", TestingSettings.Instance.ExecutableExt)
						.Replace("{RuntimeId}", TestingSettings.Instance.RuntimeId);
					var serviceCommandAgent = new ServiceCommandAgent(item);
					serviceCommandAgent.Start();
					serviceCommandAgents.Add(serviceCommandAgent);
				}
			}

			if (TestingSettings.Instance.CopyItems != null)
			{
				foreach (var item in TestingSettings.Instance.CopyItems)
				{
					item.Source = item.Source?.Replace("{BuildConfiguration}", TestingSettings.Instance.BuildConfiguration)
						.Replace("{RuntimeId}", TestingSettings.Instance.RuntimeId);
					item.Destination = item.Destination?.Replace("{BuildConfiguration}", TestingSettings.Instance.BuildConfiguration)
						.Replace("{RuntimeId}", TestingSettings.Instance.RuntimeId);
					DeploymentItemFixture.CopyDirectory(item.Source, item.Destination, true);
				}
			}
		}

		List<ServiceCommandAgent> serviceCommandAgents = new List<ServiceCommandAgent>();

		bool disposed;

		public void Dispose()
		{
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