using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Fonlow.Testing
{
	/// <summary>
	/// A dictionary of HttpClientWithUserName
	/// </summary>
	[Obsolete("In favour of derived classes of HttpClientWithUsername in the integration test suites")]
	public class AuthHttpClients: IDisposable
	{
		public AuthHttpClients() : this(null)
		{
		}

		public AuthHttpClients(HttpMessageHandler handler)
		{
			Dic = TestingSettings.Instance.Users.ToDictionary(up => up.Username,
				up => HttpClientWithUsername.Create(new Uri(TestingSettings.Instance.BaseUrl), up.Username, up.Password, handler));
		}

		public Dictionary<string, HttpClientWithUsername> Dic { get; }

		bool disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (Dic != null)
					{
						foreach (var item in Dic.Values)
						{
							item.Dispose();
						}
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
