using Fonlow.Auth;
using System;
using System.Net.Http;

namespace Fonlow.Testing
{
	/// <summary>
	/// Wrap a HttpClient instance with a bearer token initialized through username and password.
	/// This class intentionally does not use refresh token, since it is for integration test suites which won't likely run for long hours.
	/// </summary>
	public class HttpClientWithUsername : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="baseUri"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="handler">Default AcceptAnyCertificateHandler. Injected handler should generally contains ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator </param>
		/// <returns></returns>
		public static HttpClientWithUsername Create(Uri baseUri, string username, string password, HttpMessageHandler handler = null)
		{
			return new HttpClientWithUsername(baseUri, username, password, handler);
		}

		/// <summary>
		/// Initialize HttpClient with a bear token obtained after posting username and password. To be used in a xUnit Fixture or Collection, the derived
		/// class must have a default constructor for initializing the parameters of this constructor.
		/// The access token is acquired from baseUri/token.
		/// </summary>
		/// <param name="baseUri"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="handler">Default AcceptAnyCertificateHandler. Injected handler should generally contains ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator </param>
		protected HttpClientWithUsername(Uri baseUri, string username, string password, HttpMessageHandler handler = null)
		{
			BaseUri = baseUri;
			Username = username;
			Password = password;
			this.httpMessageHandler = handler ?? AcceptAnyCertificateHandler;
			if (String.IsNullOrEmpty(AccessToken) || (Expiry - DateTime.Now) < TimeSpan.FromMinutes(5)) //don't bother to do refresh token
			{
				using var httpClient = new HttpClient(httpMessageHandler, false);
				httpClient.BaseAddress = baseUri;
				var authClient = new AuthClient(httpClient, null);
				var response = authClient.PostRopcTokenRequestAsFormDataToAuthAsync(new Auth.Models.ROPCRequst
				{
					Username = username,
					Password = password,
				}).Result;

				AccessToken = response.access_token;
			}

			if (!String.IsNullOrEmpty(AccessToken))
			{
				AuthorizedClient = new HttpClient(this.httpMessageHandler, false);
				AuthorizedClient.BaseAddress = BaseUri;
				AuthorizedClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", AccessToken);
			}
			else
			{
				throw new System.Security.Authentication.AuthenticationException($"Getting token failed for {username} at uri {baseUri}. Please check username, password and baseUri in app.config, and make sure http or httpS is all right.");
			}
		}

		HttpMessageHandler httpMessageHandler;

		/// <summary>
		/// Good for testing
		/// </summary>
		static HttpClientHandler AcceptAnyCertificateHandler
		{
			get
			{
				return new HttpClientHandler()
				{
					ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
				};
			}
		}

		public Uri BaseUri { get; private set; }

		/// <summary>
		/// Access_token returned in token
		/// </summary>
		public string AccessToken { get; private set; }

		public DateTime Expiry { get; private set; } = DateTime.Now.AddYears(-10); // Min may not be good. Just make sure expiry for now.

		public string Username { get; private set; }

		public string Password { get; private set; }

		/// <summary>
		/// Null if the authentication failed.
		/// </summary>
		public HttpClient AuthorizedClient { get; private set; }

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
