using System;
using System.Net.Http;

namespace Fonlow.Testing
{
	/// <summary>
	/// Used together with ServiceCommandsFixture and ServiceCommandAgent. HttpClient wrapped read test settings from appsettings.json, and ignore SSL certificates.
	/// </summary>
	public class BasicHttpClient : IDisposable
	{
		public BasicHttpClient(HttpMessageHandler handler)
		{
			HttpClient = new System.Net.Http.HttpClient(handler ?? AcceptAnyCertificateHandler);
		}

		public BasicHttpClient(): this(null)
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
