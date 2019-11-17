using System;
using System.Collections.Generic;
using System.Text;

namespace Fonlow.Testing
{
	public class IisExpressAgent: IisExpressAgentBase
	{
		public IisExpressAgent(): base(TestingSettings.Instance.HostSite, TestingSettings.Instance.HostSiteApplicationPool, 
			TestingSettings.Instance.SlnName, TestingSettings.Instance.SlnRoot)
		{

		}
	}
}
