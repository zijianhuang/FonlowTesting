﻿using System;
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
                UseShellExecute=true,
            };

            process = Process.Start(info);
            Debug.WriteLine($"IIS Express (pid: {process.Id} started with {arguments}");
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

                var hostSite = System.Configuration.ConfigurationManager.AppSettings["Testing_HostSite"];
                var hostSiteApplicationPool = System.Configuration.ConfigurationManager.AppSettings["Testing_HostSiteApplicationPool"];
                var slnRoot = System.Configuration.ConfigurationManager.AppSettings["Testing_SlnRoot"];//for VS 2015+
                var iisStartArguments = String.IsNullOrEmpty(slnRoot) ? String.Format("/site:\"{0}\" /apppool:\"{1}\"", hostSite, hostSiteApplicationPool)
                    : String.Format("/site:\"{0}\" /apppool:\"{1}\" /config:\"{2}\"", hostSite, hostSiteApplicationPool, System.IO.Path.Combine(slnRoot, @".vs\config\applicationhost.config"));
                return iisStartArguments;
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
                process.CloseMainWindow();//More graceful than Kill.
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