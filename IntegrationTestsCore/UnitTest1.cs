using Xunit;
using Fonlow.Testing;

namespace IntegrationTestsCore
{
	public class WhetherApiFixture : DefaultHttpClient
	{
		public WhetherApiFixture()
		{
			Api = new DemoCoreWeb.Controllers.Client.WeatherForecast(this.HttpClient, this.BaseUri);
		}

		public DemoCoreWeb.Controllers.Client.WeatherForecast Api { get; private set; }
	}

	[Collection("ServiceLaunch")]
	public class UnitTest1: IClassFixture<WhetherApiFixture>
	{
		public UnitTest1(WhetherApiFixture fixture)
		{
			Api = fixture.Api;
		}

		DemoCoreWeb.Controllers.Client.WeatherForecast Api;

		[Fact]
		public void Test1()
		{
			Assert.NotEmpty(Api.Get());
		}
	}
}
