using System;
using System.Net.Http;

namespace Fonlow.Testing
{
	public class DefaultHttpClient : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="handler">Default AcceptAnyCertificateHandler. Injected handler should generally contains ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator </param>
		public DefaultHttpClient(HttpMessageHandler handler = null)
		{
			BaseUri = new Uri(TestingSettings.Instance.BaseUrl);
			HttpClient = new System.Net.Http.HttpClient(handler ?? AcceptAnyCertificateHandler);
			HttpClient.BaseAddress = BaseUri;
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

		public Uri BaseUri { get; private set; }

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
