using System;

namespace Fonlow.Testing
{
    [Obsolete("In favor of ServiceCommandFixture")]
    public class IisExpressFixture : IisExpressFixtureBase
	{
		public IisExpressFixture(): base(!String.IsNullOrWhiteSpace(TestingSettings.Instance.HostSite))
		{

		}
	}
}
