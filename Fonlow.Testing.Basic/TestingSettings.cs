using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fonlow.Testing
{
	/// <summary>
	/// Data model of the "Testing" section "appsettings.json" and optionally "appsettings.BuildConfiguration.json"; 
	/// and the singleton object to access the settings read from the JSON files
	/// </summary>
	public sealed class TestingSettings
	{
		#region Singleton and init
		private TestingSettings()
		{
		}

		private static readonly Lazy<TestingSettings> lazy =
			new Lazy<TestingSettings>(() => Create());

		public static TestingSettings Instance { get { return lazy.Value; } }

		static TestingSettings Create()
		{
			var obj = new TestingSettings();
			IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
			obj.BuildConfiguration = GetBuildConfiguration();
			obj.RuntimeId = RuntimeInformation.RuntimeIdentifier;

			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				obj.ExecutableExt = ".exe";
			}

			string specificAppSettingsFilename = $"appsettings.{obj.BuildConfiguration}.json";
			if (Path.Exists(specificAppSettingsFilename))
			{
				configBuilder.AddJsonFile(specificAppSettingsFilename);
			}

			var config = configBuilder.Build();
			config.Bind("Testing", obj);//work only when properties are not with private setter.

			return obj;
		}

		static string GetBuildConfiguration()
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			var location = executingAssembly.Location;
			var dir = Path.GetDirectoryName(location);
			var pathNames = Directory.GetFiles(dir, "*.dll");
			List<Assembly> testAssemblies = new List<Assembly>();
			foreach (var p in pathNames)
			{
				try
				{
					var a = Assembly.LoadFile(p);
					var referencedAssemblies = a.GetReferencedAssemblies();
					if (referencedAssemblies.Any(d => d.Name == "xunit.core") && !Path.GetFileName(p).StartsWith("xunit.", StringComparison.OrdinalIgnoreCase))
					{
						testAssemblies.Add(a);
					}
				}
				catch (FileLoadException)
				{
					continue;
				}
				catch (BadImageFormatException)
				{
					continue;
				}
			}

			if (testAssemblies.Count > 0)
			{
				var assemblyConfigurationAttribute = testAssemblies[0].GetCustomAttribute<AssemblyConfigurationAttribute>();
				return assemblyConfigurationAttribute?.Configuration;
			}
			else
			{
				return "Debug";
			}
		}
		#endregion

		#region Settings in appsettings.json
		/// <summary>
		/// Service Commands are executed in the order of JSON data initialization.
		/// </summary>
		public IReadOnlyDictionary<string, ServiceCommand> ServiceCommands { get; set; }

		/// <summary>
		/// Each CopyItem is executed synchronous, so items are executed subsequently.
		/// </summary>
		public CopyItem[] CopyItems { get; set; }

		/// <summary>
		/// Used when Web resource is there, no need to be under the control of the test suite.
		/// </summary>
		public string BaseUrl { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }

		/// <summary>
		/// For testing with many different user credentials.
		/// </summary>
		public UsernamePassword[] Users { get; set; }

		#endregion


		#region Environment variables
		/// <summary>
		/// Build configuration of the test suite such as Debug, Release or whatever custom build configuration. 
		/// ServiceCommandFixture will replace {BuildConfiguration} in commandPath and arguments with this.
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore]
		public string BuildConfiguration { get; private set; }

		/// <summary>
		/// RuntimeInformation.RuntimeIdentifier such as win-x64, linux-x64. Generally used for publishing to a specific platform.
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore]
		public string RuntimeId { get; private set; }

		/// <summary>
		/// The extention name of executable file. On Win, it is ".exe" including the dot, and on Linux and MacOs, empty.
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore]
		public string ExecutableExt { get; private set; } = string.Empty;
		#endregion

		//#region Obsolete
		//[Obsolete("In favor of ServiceCommandFixture")]
		//public string DotNetServiceAssemblyPath { get; set; }

		///// <summary>
		///// For IIS Express, host site name
		///// </summary>
		//[Obsolete("In favor of ServiceCommandFixture")]
		//public string HostSite { get; set; }

		///// <summary>
		///// For IIS Express, application pool
		///// </summary>
		//[Obsolete("In favor of ServiceCommandFixture")]
		//public string HostSiteApplicationPool { get; set; }

		///// <summary>
		///// For IIS Express, the lib needs to be aware the SLN root
		///// </summary>
		//[Obsolete("In favor of ServiceCommandFixture")]
		//public string SlnRoot { get; set; }

		///// <summary>
		///// For IIS Express, the lib needs to know the SLN name
		///// </summary>
		//[Obsolete("In favor of ServiceCommandFixture")]
		//public string SlnName { get; set; }
		//#endregion

	}

	public sealed class UsernamePassword
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public sealed class ServiceCommand
	{
		public string CommandPath { get; set; }

		public bool IsPowerShellCommand { get; set; }

		public string Arguments { get; set; }

		/// <summary>
		/// Some services may take some seconds to launch then listen, especially in GitHub Actions which VM/container could be slow. A good bet may be 5 seconds.
		/// </summary>
		public int Delay { get; set; }
		public string ConnectionString { get; set; }
		public string BaseUrl { get; set; }

		/// <summary>
		/// For testing with many different user credentials with different authorization.
		/// </summary>
		/// <remarks>Obviously 2FA and alike are not welcome. Good enough for integration tests, but not E2E.</remarks>
		public UsernamePassword[] Users { get; set; }
	}

	public sealed class CopyItem
	{
		public string Source { get; set; }
		public string Destination { get; set; }
	}
}
