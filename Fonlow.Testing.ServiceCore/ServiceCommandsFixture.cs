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
                foreach (var item in TestingSettings.Instance.ServiceCommands)
                {
                    item.Arguments = item.Arguments?.Replace("{BuildConfiguration}", TestingSettings.Instance.BuildConfiguration);
                    item.CommandPath = item.CommandPath?.Replace("{BuildConfiguration}", TestingSettings.Instance.BuildConfiguration);
                    item.CommandPath = item.CommandPath?.Replace("{ExecutableExt}", TestingSettings.Instance.ExecutableExt);
                    var serviceCommandAgent = new ServiceCommandAgent(item);
                    serviceCommandAgent.Start();
                    serviceCommandAgents.Add(serviceCommandAgent);
                }
            }

            if (TestingSettings.Instance.CopyItems != null)
            {
                foreach (var item in TestingSettings.Instance.CopyItems)
                {
                    item.Source = item.Source?.Replace("{BuildConfiguration}", TestingSettings.Instance.BuildConfiguration);
                    item.Destination = item.Destination?.Replace("{BuildConfiguration}", TestingSettings.Instance.BuildConfiguration);
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