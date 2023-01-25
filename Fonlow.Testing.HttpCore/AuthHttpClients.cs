using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Fonlow.Testing
{
	/// <summary>
	/// A dictionary of HttpClientWithUserName
	/// </summary>
	public class AuthHttpClients
	{
		public AuthHttpClients() : this(null)
		{
		}

		public AuthHttpClients(HttpMessageHandler handler)
		{
			TestingSettings.Instance.Users.ToDictionary(up => HttpClientWithUsername.Create(new Uri(TestingSettings.Instance.BaseUrl), up.Username, up.Password, handler));
		}

		public Dictionary<string, HttpClientWithUsername> Dic { get; }
	}
}
