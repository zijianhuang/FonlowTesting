using System;
using System.Diagnostics;

namespace Fonlow.Testing
{
    /// <summary>
    /// Launch IIS Express if AppSettings["Testing_UseIisExpress"] is true, using the following settings
    /// AppSettings["Testing_HostSite"]; AppSettings["Testing_HostSiteApplicationPool"]. This class is mainly for 
    /// launching IIS Express only once for one or multiple test classes that talk to the same Website.
    /// </summary>
    [Obsolete("In favor of ServiceCommandFixture")]
    public class IisExpressFixtureBase : IDisposable
    {
        /// <summary>
        /// Create the fixture. And this constructor is also used in XUnit.ICollectionFixture.
        /// </summary>
        public IisExpressFixtureBase(bool active)
        {
            if (active)
            {
                iisExpressAgent = new IisExpressAgent();
                iisExpressAgent.Start();
            }
        }
       
        readonly IisExpressAgent iisExpressAgent;


        bool disposed;

        // implements IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // if this is a dispose call dispose on all state you
                // hold, and take yourself off the Finalization queue.
                if (disposing)
                {
                    if (iisExpressAgent!=null)
                    {
                        iisExpressAgent.Stop();
                        Trace.TraceInformation("IIS Express stoped.");
                    }
                }

                disposed = true;
            }
        }

    }

}