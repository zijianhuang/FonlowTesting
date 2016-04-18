using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Fonlow.Testing;
using Fonlow.DemoService;
using System.ServiceModel;

namespace IntegrationTests
{
    public class WCFTests
    {
        const string hostBaseAddress = "http://localhost:4060/Service1.svc";

        [Fact]
        public void TestGetData()
        {
            IisExpressAgent agent = new IisExpressAgent();
            try
            {
                agent.Start();
                var client = ChannelFactory<IService1>.CreateChannel(new BasicHttpBinding(), new EndpointAddress(hostBaseAddress));
                var s=  client.GetData(3);
                Assert.Equal("3", s);
            }
            finally
            {
                agent.Stop();
            }



        }
    }
}
