using System;

namespace Fonlow.Testing
{
	/// <summary>
	/// To be used alone without fixture.
	/// </summary>
	[Obsolete("In favor of ServiceCommandAgent")]
	public class IisExpressAgent: IisExpressAgentBase
	{
		public IisExpressAgent(): base(TestingSettings.Instance.HostSite, TestingSettings.Instance.HostSiteApplicationPool, 
			TestingSettings.Instance.SlnName, TestingSettings.Instance.SlnRoot)
		{

		}
	}
}
