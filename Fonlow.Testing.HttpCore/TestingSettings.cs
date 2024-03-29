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

		//Settings for IIS Express
		public string HostSite { get; set; }
		public string HostSiteApplicationPool { get; set; }
		public string SlnRoot { get; set; }
		public string SlnName { get; set; }

		public UsernamePassword[] Users { get; set; }
	}

	public sealed class UsernamePassword
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
