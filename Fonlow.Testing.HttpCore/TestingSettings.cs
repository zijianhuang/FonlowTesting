using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Fonlow.Testing
{
    public sealed class TestingSettings
    {
        private TestingSettings()
        {
        }

        public static TestingSettings Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            static TestingSettings Create()
            {
                var obj = new TestingSettings();
                IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                obj.BuildConfiguration = GetBuildConfiguration();
                string specificAppSettingsFilename = $"appsettings.{obj.BuildConfiguration}.json";
                if (Path.Exists(specificAppSettingsFilename))
                {
                    configBuilder.AddJsonFile(specificAppSettingsFilename);
                }

                var config = configBuilder.Build();
                config.Bind("Testing", obj);//work only when properties are not with private setter.

                return obj;
            }

            internal static readonly TestingSettings instance = Create();

            static string GetBuildConfiguration()
            {
                var assemblyConfigurationAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyConfigurationAttribute>();
                return assemblyConfigurationAttribute?.Configuration;
            }
        }

        [Obsolete("In favor of ServiceCommandFixture")]
        public string DotNetServiceAssemblyPath { get; set; }
        [Obsolete("In favor of ServiceCommandFixture")]
        public string BaseUrl { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// For IIS Express, host site name
        /// </summary>
        [Obsolete("In favor of ServiceCommandFixture")]
        public string HostSite { get; set; }

        /// <summary>
        /// For IIS Express, application pool
        /// </summary>
        [Obsolete("In favor of ServiceCommandFixture")]
        public string HostSiteApplicationPool { get; set; }

        /// <summary>
        /// For IIS Express, the lib needs to be aware the SLN root
        /// </summary>
        [Obsolete("In favor of ServiceCommandFixture")]
        public string SlnRoot { get; set; }

        /// <summary>
        /// For IIS Express, the lib needs to know the SLN name
        /// </summary>
        [Obsolete("In favor of ServiceCommandFixture")]
        public string SlnName { get; set; }

        /// <summary>
        /// For testing with many different user credentials.
        /// </summary>
        public UsernamePassword[] Users { get; set; }

        public ServiceCommand[] ServiceCommands { get; set; }

        /// <summary>
        /// Build configuration such as Debug, Release or whatever custom build configuration. 
        /// ServiceCommandFixture will replace {BuildConfiguration} in arguments with this.
        /// </summary>
        public string BuildConfiguration { get; private set; }
    }

    public sealed class UsernamePassword
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public sealed class ServiceCommand
    {
        public string CommandPath { get; set; }
        public string Arguments { get; set; }

        /// <summary>
        /// Some services may take some seconds to launch then listen, especially in GitHub Actions which VM/container could be slow. A good bet may be 5 seconds.
        /// </summary>
        public int Delay { get; set; }
        public string ConnectionString { get; set; }
        public string BaseUrl { get; set; }
    }
}
