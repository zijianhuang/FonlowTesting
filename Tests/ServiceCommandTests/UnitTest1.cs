using Xunit;
using Fonlow.Testing;
using System.Linq;

namespace IntegrationTests
{
	public class WhetherApiFixture : BasicHttpClient
	{
		public WhetherApiFixture()
		{
			var c = TestingSettings.Instance.ServiceCommands[0];
			this.HttpClient.BaseAddress = new System.Uri(c.BaseUrl);
			Api = new DemoCoreWeb.Controllers.Client.WeatherForecast(this.HttpClient);
		}

		public DemoCoreWeb.Controllers.Client.WeatherForecast Api { get; private set; }
	}

	[Collection("ServicesLaunch")]
	public class UnitTest1: IClassFixture<WhetherApiFixture>
	{
		public UnitTest1(WhetherApiFixture fixture)
		{
			api = fixture.Api;
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
	}
}
