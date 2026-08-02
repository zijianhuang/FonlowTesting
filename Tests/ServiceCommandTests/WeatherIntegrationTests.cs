using Fonlow.Testing;
using System;
using System.Threading;
using Xunit;

namespace IntegrationTests
{
	public class WhetherApiFixture : BasicHttpClient
	{
		public WhetherApiFixture()
		{
			var c = TestingSettings.Instance.ServiceCommands["LaunchWebApi"];
			this.HttpClient.BaseAddress = new System.Uri(c.BaseUrl);
			Api = new DemoCoreWeb.Controllers.Client.WeatherForecast(this.HttpClient);
		}

		public DemoCoreWeb.Controllers.Client.WeatherForecast Api { get; private set; }
	}

	[Collection("ServicesLaunch")]
	public class WeatherIntegrationTests: IClassFixture<WhetherApiFixture>
	{
		private readonly ITestOutputHelper output;
		public WeatherIntegrationTests(WhetherApiFixture fixture, ITestOutputHelper output)
		{
			api = fixture.Api;
			this.output = output;
		}

		DemoCoreWeb.Controllers.Client.WeatherForecast api;

		[Fact]
		public void Test1()
		{
			Assert.NotEmpty(api.Get());
		}

		[Fact]
		public void TestGetAppSettings()
		{
			var r = api.GetSettings();
			Assert.Equal("OK", r);
		}

		//[Fact]
		//public void TestSleep1()
		//{
		//	Console.WriteLine("Test1 is running");
		//	Thread.Sleep(3000);
		//	Console.WriteLine("Test1 is completed");
		//}

		//[Fact]
		//public void TestSleep2()
		//{
		//	output.WriteLine("Test2 is running");
		//	Thread.Sleep(5000);
		//	output.WriteLine("Test2 is completed");
		//}

	}
}
