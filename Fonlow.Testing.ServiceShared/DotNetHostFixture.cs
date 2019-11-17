using System;

namespace Fonlow.Testing
{
	/// <summary>
	/// Launch IIS Express if AppSettings["Testing_UseIisExpress"] is true, using the following settings
	/// AppSettings["Testing_HostSite"]; AppSettings["Testing_HostSiteApplicationPool"]. This class is mainly for 
	/// launching IIS Express only once for one or multiple test classes that talk to the same Website.
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

		// implements IDisposable
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