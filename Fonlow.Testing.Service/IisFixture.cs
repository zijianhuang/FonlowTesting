using System;
using System.Diagnostics;

namespace Fonlow.Testing
{
    /// <summary>
    /// Launch IIS Express if AppSettings["Testing_UseIisExpress"] is true, using the following settings
    /// AppSettings["Testing_HostSite"]; AppSettings["Testing_HostSiteApplicationPool"]. This class is mainly for 
    /// launching IIS Express only once for one or multiple test classes that talk to the same Website.
    /// </summary>
    public class IisExpressFixture : IDisposable
    {
        /// <summary>
        /// Create the fixture. And this constructor is also used in XUnit.ICollectionFixture.
        /// </summary>
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
        /// Create the fixture only if AppSettings["Testing_UseIisExpress"]=true.
        /// </summary>
        /// <returns>Null if AppSettings["Testing_UseIisExpress"] is false.</returns>
        public static IisExpressFixture Create()
        {
            if (IisExpressAgent.UseIisExpress)
                return new IisExpressFixture();

            return null;
        }

        /// <summary>
        /// What defined in AppSettings["Testing_BaseUrl"]
        /// </summary>
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