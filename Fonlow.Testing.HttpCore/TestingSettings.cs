using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				var appSettingsSection = config.GetSection("Testing");
				obj.ConfigurationRoot = config;
				obj.DotNetServiceAssemblyPath = appSettingsSection["DotNetServiceAssemblyPath"];
				obj.BaseUrl = appSettingsSection["BaseUrl"];
				obj.Username = appSettingsSection["Username"];
				obj.Password = appSettingsSection["Password"];
				return obj;
			}

			internal static readonly TestingSettings instance = Create();
		}

		public IConfigurationRoot ConfigurationRoot { get; private set; }

		public string DotNetServiceAssemblyPath { get; private set; }
		public string BaseUrl { get; private set; }
		public string Username { get; private set; }
		public string Password { get; private set; }

	}
}
