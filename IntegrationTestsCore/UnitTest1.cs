using System;
using Xunit;
using Fonlow.Testing;
using DemoCoreWeb.Client;
using DemoCoreWeb;

namespace IntegrationTestsCore
{
	public class WhetherApiFixture : IDisposable
	{
		public WhetherApiFixture()
		{
			var baseUri = new Uri(TestingSettings.Instance.BaseUrl);
			httpClient = new System.Net.Http.HttpClient();
			Api = new DemoCoreWeb.Controllers.Client.WeatherForecast(this.httpClient, baseUri);
		}

		public DemoCoreWeb.Controllers.Client.WeatherForecast Api { get; private set; }

	    readonly System.Net.Http.HttpClient httpClient;

		#region IDisposable pattern
		bool disposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					httpClient.Dispose();
				}

				disposed = true;
			}
		}
		#endregion
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
