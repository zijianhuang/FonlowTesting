using Fonlow.DemoService;
using Fonlow.Testing;
using System;
using System.ServiceModel;
using Xunit;

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
                var s = client.GetData(3);
                Assert.Equal("3", s);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                throw;
            }
            finally
            {
                agent.Stop();
            }



        }
    }
}
