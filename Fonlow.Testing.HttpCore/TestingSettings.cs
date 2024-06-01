using Microsoft.Extensions.Configuration;

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
				var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
				config.Bind("Testing", obj);//work only when properties are not with private setter.

				return obj;
			}

			internal static readonly TestingSettings instance = Create();
		}

		//Settings for DotNet Kestrel
		public string DotNetServiceAssemblyPath { get; set; }
		public string BaseUrl { get; set; }
		
		public string Username { get; set; }
		public string Password { get; set; }

		/// <summary>
		/// For IIS Express, host site name
		/// </summary>
		public string HostSite { get; set; }

		/// <summary>
		/// For IIS Express, application pool
		/// </summary>
		public string HostSiteApplicationPool { get; set; }

		/// <summary>
		/// For IIS Express, the lib needs to be aware the SLN root
		/// </summary>
		public string SlnRoot { get; set; }

		/// <summary>
		/// For IIS Express, the lib needs to know the SLN name
		/// </summary>
		public string SlnName { get; set; }

		/// <summary>
		/// For testing with many different user credentials.
		/// </summary>
		public UsernamePassword[] Users { get; set; }

		public ServiceCommand[] ServiceCommands { get; set; }
	}

	public sealed class UsernamePassword
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public sealed class ServiceCommand {
		public string CommandPath { get; set; }
		public string Arguments { get; set; }

		/// <summary>
		/// Some services may take some seconds to launch then listen.
		/// </summary>
		public int Delay { get; set; }
		public string ConnectionString{ get; set; }
		public string BaseUrl { get; set; }
	}
}
