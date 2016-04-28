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
    [Collection(TestConstants.IisExpressAndInit)]
    public class IisExpressFixtureTests
    {
        const string hostBaseAddress = "http://localhost:4060/Service1.svc";

        [Fact]
        public void TestGetData()
        {

            var client = ChannelFactory<IService1>.CreateChannel(new BasicHttpBinding(), new EndpointAddress(hostBaseAddress));
            var s = client.GetData(3);
            Assert.Equal("3", s);
        }

        [Fact]
        public void TestComposite()
        {
            var client = ChannelFactory<IService1>.CreateChannel(new BasicHttpBinding(), new EndpointAddress(hostBaseAddress));
            var s = client.GetDataUsingDataContract(new CompositeType() { BoolValue = true, StringValue = "hey" });
            Assert.Equal("heySuffix", s.StringValue);
        }
    }
}
