using System;
using System.Net.Http;

namespace Fonlow.Testing
{
	/// <summary>
	/// HttpClient wrapped read test settings from appsettings.json, and ignore SSL certificates.
	/// </summary>
	public class DefaultHttpClient : IDisposable
	{
		public DefaultHttpClient(HttpMessageHandler handler)
		{
			BaseUri = new Uri(TestingSettings.Instance.BaseUrl);
			HttpClient = new System.Net.Http.HttpClient(handler ?? AcceptAnyCertificateHandler);
			HttpClient.BaseAddress = BaseUri;
		}

		public DefaultHttpClient(): this(null)
		{
		}

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

		public System.Net.Http.HttpClient HttpClient { get; private set; }

		public Uri BaseUri { get; protected set; }

		#region IDisposable pattern
		bool disposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					HttpClient.Dispose();
				}

				disposed = true;
			}
		}
		#endregion
	}

}
