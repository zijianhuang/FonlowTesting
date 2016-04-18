using System;
using System.Diagnostics;

namespace Fonlow.Testing
{
    /// <summary>
    /// Launch IIS Express if AppSettings["Testing_UseIisExpress"] is true, using the following settings
    /// AppSettings["Testing_HostSite"]; AppSettings["Testing_HostSiteApplicationPool"];
    /// </summary>
    public class IisExpressFixture : IDisposable
    {
        public IisExpressFixture()
        {
            Debug.WriteLine("To create IisExpressFixture");
            if (IisExpressAgent.UseIisExpress)
            {
                iisExpressAgent = new IisExpressAgent();
                iisExpressAgent.Start();
                BaseUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["Testing_BaseUrl"]);
            }
        }

        /// <summary>
        /// Create the fixture only if AppSettings["Testing_UseIisExpress"].
        /// </summary>
        /// <returns>Null if AppSettings["Testing_UseIisExpress"] is false.</returns>
        public static IisExpressFixture Create()
        {
            var useIisExpress = System.Configuration.ConfigurationManager.AppSettings["Testing_UseIisExpress"];
            if (String.Equals(useIisExpress, "true", StringComparison.CurrentCultureIgnoreCase))
                return new IisExpressFixture();

            return null;
        }

        public Uri BaseUri
        { get; private set; }
        
        IisExpressAgent iisExpressAgent;


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