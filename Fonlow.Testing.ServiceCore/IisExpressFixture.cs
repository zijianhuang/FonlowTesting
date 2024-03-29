using System;

namespace Fonlow.Testing
{
	public class IisExpressFixture : IisExpressFixtureBase
	{
		public IisExpressFixture(): base(!String.IsNullOrWhiteSpace(TestingSettings.Instance.HostSite))
		{

		}
	}
}
