using System;

namespace Fonlow.Testing
{
	public class DefaultHttpClient : IDisposable
	{
		public DefaultHttpClient()
		{
			BaseUri = new Uri(TestingSettings.Instance.BaseUrl);
			HttpClient = new System.Net.Http.HttpClient();
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
