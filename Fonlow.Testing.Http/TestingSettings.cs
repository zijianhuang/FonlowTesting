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
				var appSettingsSection = System.Configuration.ConfigurationManager.AppSettings;
				obj.DotNetServiceAssemblyPath = appSettingsSection["Testing_DotNetServiceAssemblyPath"];
				obj.BaseUrl = appSettingsSection["Testing_BaseUrl"];
				obj.Username = appSettingsSection["Testing_Username"];
				obj.Password = appSettingsSection["Testing_Password"];
				obj.HostSite = appSettingsSection["Testing_HostSite"];
				obj.HostSiteApplicationPool = appSettingsSection["Testing_HostSiteApplicationPool"];
				obj.SlnRoot = appSettingsSection["Testing_SlnRoot"];
				obj.SlnName = appSettingsSection["Testing_SlnName"];
				return obj;
			}

			internal static readonly TestingSettings instance = Create();
		}

		public string DotNetServiceAssemblyPath { get; private set; }
		public string BaseUrl { get; private set; }
		public string Username { get; private set; }
		public string Password { get; private set; }

		public string HostSite { get; private set; }
		public string HostSiteApplicationPool { get; private set; }
		public string SlnRoot { get; private set; }
		public string SlnName { get; private set; }


	}
}
