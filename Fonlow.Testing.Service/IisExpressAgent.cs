using System;
using System.Diagnostics;

namespace Fonlow.Testing
{
    /// <summary>
    /// For starting and stoping IIS Express
    /// </summary>
    /// <remarks>The IIS Express config is at </remarks>
    public class IisExpressAgent
    {
        public void Start(string arguments)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files (x86)\IIS Express\iisexpress.exe", arguments)
            {
                // WindowStyle= ProcessWindowStyle.Minimized
            };

            process = Process.Start(info);
            Debug.WriteLine("IIS Express started: "+arguments);
            timeStart = DateTime.Now;
        }

        /// <summary>
        /// Start IIS Express with settings in AppSettings, while Testing_UseIisExpress must be true.
        /// </summary>
        public void Start()
        {
            if (UseIisExpress)
            {
                Start(IisExpressArguments);
            }
            else
            {
                throw new ArgumentException("Please make sure Testing_UseIisExpress in AppSettings is true.");
            }
        }

        public static string IisExpressArguments
        {
            get
            {
                var useIisExpress = System.Configuration.ConfigurationManager.AppSettings["Testing_UseIisExpress"];
                if (String.Equals(useIisExpress, "true", StringComparison.CurrentCultureIgnoreCase))
                {
                    var hostSite = System.Configuration.ConfigurationManager.AppSettings["Testing_HostSite"];
                    var hostSiteApplicationPool = System.Configuration.ConfigurationManager.AppSettings["Testing_HostSiteApplicationPool"];
                    return String.Format("/site:\"{0}\" /apppool:\"{1}\"", hostSite, hostSiteApplicationPool);
                }

                return null;
            }
        }

        public static bool UseIisExpress
        {
            get
            {
                var useIisExpress = System.Configuration.ConfigurationManager.AppSettings["Testing_UseIisExpress"];
                return String.Equals(useIisExpress, "true", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        DateTime timeStart;
         
        Process process;

        public void Stop()
        {
            if (process == null)
                return;

            try
            {
                var span = (DateTime.Now - timeStart).TotalSeconds;
                Debug.WriteLine(String.Format("Test cases with IIS Express had run for {0} seconds.", span));
                process.Kill();
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
