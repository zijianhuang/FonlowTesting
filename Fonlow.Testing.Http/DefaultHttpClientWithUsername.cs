using System;

namespace Fonlow.Testing
{
    /// <summary>
    /// Provide an authorized HttpClient instance with uri and username/password defined in appsettings.json:
    /// BaseUrl, Username and Password.
    /// </summary>
    public class DefaultHttpClientWithUsername : HttpClientWithUsername
    {
        public DefaultHttpClientWithUsername() : base(new Uri(TestingSettings.Instance.BaseUrl), TestingSettings.Instance.Username, TestingSettings.Instance.Password)
        {

        }
    }

}
