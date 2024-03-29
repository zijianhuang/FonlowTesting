using System;

namespace Fonlow.Testing
{
	/// <summary>
	/// Launch DotNet Kestrel Web server according what defined in TestingSettings loaded from appsettings.json
	/// </summary>
	public class DotNetHostFixture : IDisposable
	{
		/// <summary>
		/// Create the fixture. And this constructor is also used in XUnit.ICollectionFixture.
		/// </summary>
		public DotNetHostFixture()
		{
			dotNetHostAgent = new DotNetHostAgent();
			dotNetHostAgent.Start();
			BaseUri = new Uri(TestingSettings.Instance.BaseUrl);
		}

		public Uri BaseUri { get; private set; }

		DotNetHostAgent dotNetHostAgent;

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
					if (dotNetHostAgent != null)
					{
						dotNetHostAgent.Stop();
					}
				}

				disposed = true;
			}
		}

	}

}