namespace Fonlow.Testing
{
	/// <summary>
	/// To be used alone without fixture.
	/// </summary>
	public class IisExpressAgent: IisExpressAgentBase
	{
		public IisExpressAgent(): base(TestingSettings.Instance.HostSite, TestingSettings.Instance.HostSiteApplicationPool, 
			TestingSettings.Instance.SlnName, TestingSettings.Instance.SlnRoot)
		{

		}
	}
}
