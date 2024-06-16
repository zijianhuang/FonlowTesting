using System;
using System.Net.Http;
namespace Fonlow.Testing
{
	/// <summary>
	/// Provide an authorized HttpClient instance with uri and username/password defined in appsettings.json:
	/// BaseUrl, Username and Password.
	/// </summary>
	[Obsolete("In favour of ServiceCommand along with its username/password pairs")]
	public class DefaultHttpClientWithUsername : HttpClientWithUsername
	{
		public DefaultHttpClientWithUsername() : this(null)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handler">Default AcceptAnyCertificateHandler. Injected handler should generally contains ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator </param>
		public DefaultHttpClientWithUsername(HttpMessageHandler handler) : base(new Uri(TestingSettings.Instance.BaseUrl), TestingSettings.Instance.Username, TestingSettings.Instance.Password, handler)
		{

		}
	}

}
