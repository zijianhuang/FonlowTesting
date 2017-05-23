using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Fonlow.Testing
{
    /// <summary>
    /// Provide an authorized HttpClient instance with uri and username/password defined in app config, with AppSettings:
    /// Testing_BaseUrl, Testing_Username and Testing_Password.
    /// </summary>
    public class DefaultHttpClientFixture : HttpClientWithUsername
    {
        public DefaultHttpClientFixture()
            : base(new Uri(System.Configuration.ConfigurationManager.AppSettings["Testing_BaseUrl"])
            , System.Configuration.ConfigurationManager.AppSettings["Testing_Username"]
            , System.Configuration.ConfigurationManager.AppSettings["Testing_Password"])
        {

        }
    }


    /// <summary>
    /// Wrap a HttpClient instance with a bearer token initialized through username and password.
    /// </summary>
    public class HttpClientWithUsername : IDisposable
    {
        public static HttpClientWithUsername Create(Uri baseUri, string username, string password)
        {
            return new HttpClientWithUsername(baseUri, username, password);
        }

        /// <summary>
        /// Initialize HttpClient with a bear token obtained after posting username and password. To be used in a xUnit Fixture or Collection, the derived
        /// class must have a default constructor for initializing the parameters of this constructor.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        protected HttpClientWithUsername(Uri baseUri, string username, string password)
        {
            BaseUri = baseUri;
            Username = username;
            Password = password;
            AnalyzeToken();

            if (!String.IsNullOrEmpty(AccessToken))
            {
                AuthorizedClient = new HttpClient();
                AuthorizedClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TokenType, AccessToken);
            }
            else
            {
                throw new System.Security.Authentication.AuthenticationException($"Getting token failed for {username} at uri {baseUri}. Please check username, password and baseUri in app.config, and make sure http or httpS is all right.");
            }
        }

        public Uri BaseUri { get; private set; }

        /// <summary>
        /// Access_token returned in token
        /// </summary>
        public string AccessToken { get; private set; }

        public DateTime Expiry { get; private set; }

        /// <summary>
        /// Generally bearer
        /// </summary>
        public string TokenType { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

        /// <summary>
        /// Null if the authentication failed.
        /// </summary>
        public HttpClient AuthorizedClient { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        void AnalyzeToken()
        {
            var text = GetToken(BaseUri, Username, Password);
            if (String.IsNullOrEmpty(text))
                return;

            var jObject = JObject.Parse(text);
            var accessTokenObject = jObject["access_token"];
            var expiriesInObject = jObject["expires_in"];
            var tokenTypeObject = jObject["token_type"];
            var expiriesIn = TimeSpan.FromSeconds(Int32.Parse(expiriesInObject.ToString()));
            Expiry = DateTime.Now.Add(expiriesIn);
            AccessToken = accessTokenObject.ToString();
            TokenType = tokenTypeObject.ToString();
        }

        /// <summary>
        /// Get the token body as json text from WebApi default token path
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Null if fails, and the error is in Trace.</returns>
        public static string GetToken(Uri baseUri, string userName, string password)
        {
            //inspired by http://www.codeproject.com/Articles/823263/ASP-NET-Identity-Introduction-to-Working-with-Iden
            var pairs = new KeyValuePair<string, string>[]
                        {
                            new KeyValuePair<string, string>( "grant_type", "password" ),
                            new KeyValuePair<string, string>( "username", userName ),
                            new KeyValuePair<string, string> ( "password", password )
                        };
            var content = new FormUrlEncodedContent(pairs);
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync(new Uri(baseUri, "Token"), content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        var error = String.Format("Please check app.config or Web dependencies. Cannot get token for {0}:{1} with Uri {2}, with status code {3} and message {4}", userName, password, baseUri, response.StatusCode, response.ReasonPhrase);
                        Trace.TraceError(error);
                        throw new System.Security.Authentication.AuthenticationException(error);
                    }

                    var text = response.Content.ReadAsStringAsync().Result;
                    return text;
                }

            }
            catch (AggregateException e)
            {
                e.Handle((innerException) =>
                {
                    Trace.TraceWarning(innerException.Message);
                    return false;//Better to make it false here, since the test runner may shutdown before the trace message could be written to the log file.
                });
                return null;
            }
        }

        bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (AuthorizedClient != null)
                    {
                        AuthorizedClient.Dispose();
                    }
                }

                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }


}
