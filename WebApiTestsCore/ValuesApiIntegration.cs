using System;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace WebApiTestsCore
{
	[CollectionDefinition("WebApiInit")]
	public class IisCollection : ICollectionFixture<Fonlow.Testing.IisExpressFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}

	[Collection("WebApiInit")]
	public class ValuesApiIntegration : IClassFixture<ValuesFixture>
	{
		public ValuesApiIntegration(ValuesFixture fixture)
		{
			api = fixture.Api;
		}

		Values api;

		[Fact]
		public void TestValuesGet()
		{
			//var task = authorizedClient.GetStringAsync(new Uri(baseUri, "api/Values"));
			//var text = task.Result;
			//var array = JArray.Parse(text);
			var array = api.Get().ToArray();
			Assert.Equal("value2", array[1]);
		}



		[Fact]
		public void TestValuesDelete()
		{
			api.Delete(1);
		}
	}

	///// <summary>
	///// Test with Proxy http://localhost:8888
	///// </summary>
	//public class ValuesApiWithProxyIntegration : IClassFixture<ValuesWithProxyFixture>
	//{
	//	public ValuesApiWithProxyIntegration(ValuesWithProxyFixture fixture)
	//	{
	//		api = fixture.Api;
	//	}

	//	Values api;

	//	[Fact]
	//	public void TestValuesGet()
	//	{
	//		//var task = authorizedClient.GetStringAsync(new Uri(baseUri, "api/Values"));
	//		//var text = task.Result;
	//		//var array = JArray.Parse(text);
	//		var array = api.Get().ToArray();
	//		Assert.Equal("value2", array[1]);
	//	}



	//	[Fact]
	//	public void TestValuesDelete()
	//	{
	//		api.Delete(1);
	//	}
	//}

	public class ValuesFixture : Fonlow.Testing.DefaultHttpClient
	{
		public ValuesFixture()
		{
			Api = new Values(base.HttpClient, base.BaseUri);
		}

		public Values Api { get; private set; }
	}

	public class ValuesWithProxyFixture : Fonlow.Testing.DefaultHttpClient
	{
		public ValuesWithProxyFixture(): base(handler)
		{
			Api = new Values(base.HttpClient, base.BaseUri);
		}

		static readonly HttpMessageHandler handler = new HttpClientHandler()
		{
			Proxy = new System.Net.WebProxy()
			{
				Address = new Uri("http://localhost:8888"), // Talk to proxy of Fiddler. Whether Fiddler is capturing traffic does not matter.
				BypassProxyOnLocal = false,
				//UseDefaultCredentials = proxyCredentials == null ? true : false,
				//Credentials = proxyCredentials,
			},

			UseProxy = true,

		};

		public Values Api { get; private set; }
	}

	public partial class Values
	{

		private System.Net.Http.HttpClient client;

		private System.Uri baseUri;

		public Values(System.Net.Http.HttpClient client, System.Uri baseUri)
		{
			if (client == null)
				throw new ArgumentNullException("client", "Null HttpClient.");

			if (baseUri == null)
				throw new ArgumentNullException("baseUri", "Null baseUri");

			this.client = client;
			this.baseUri = baseUri;
		}

		/// <summary>
		/// DELETE api/Values/{id}
		/// </summary>
		public async Task DeleteAsync(int id)
		{
			var requestUri = new Uri(this.baseUri, "api/Values/" + id);
			var responseMessage = await client.DeleteAsync(requestUri);
			responseMessage.EnsureSuccessStatusCode();
		}

		/// <summary>
		/// DELETE api/Values/{id}
		/// </summary>
		public void Delete(int id)
		{
			var requestUri = new Uri(this.baseUri, "api/Values/" + id);
			var responseMessage = this.client.DeleteAsync(requestUri).Result;
			responseMessage.EnsureSuccessStatusCode();
		}

		/// <summary>
		/// GET api/Values
		/// </summary>
		public async Task<string[]> GetAsync()
		{
			var requestUri = new Uri(this.baseUri, "api/Values");
			var responseMessage = await client.GetAsync(requestUri);
			responseMessage.EnsureSuccessStatusCode();
			var stream = await responseMessage.Content.ReadAsStreamAsync();
			using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
			{
				var serializer = new JsonSerializer();
				return serializer.Deserialize<string[]>(jsonReader);
			}
		}

		/// <summary>
		/// GET api/Values
		/// </summary>
		public string[] Get()
		{
			var requestUri = new Uri(this.baseUri, "api/Values");
			var responseMessage = this.client.GetAsync(requestUri).Result;
			responseMessage.EnsureSuccessStatusCode();
			var stream = responseMessage.Content.ReadAsStreamAsync().Result;
			using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
			{
				var serializer = new JsonSerializer();
				return serializer.Deserialize<string[]>(jsonReader);
			}
		}

		/// <summary>
		/// GET api/Values/{id}?name={name}
		/// </summary>
		public async Task<string> GetAsync(int id, string name)
		{
			var requestUri = new Uri(this.baseUri, "api/Values/" + id + "?name=" + Uri.EscapeDataString(name));
			var responseMessage = await client.GetAsync(requestUri);
			responseMessage.EnsureSuccessStatusCode();
			var stream = await responseMessage.Content.ReadAsStreamAsync();
			using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
			{
				return jsonReader.ReadAsString();
			}
		}

		/// <summary>
		/// GET api/Values/{id}?name={name}
		/// </summary>
		public string Get(int id, string name)
		{
			var requestUri = new Uri(this.baseUri, "api/Values/" + id + "?name=" + Uri.EscapeDataString(name));
			var responseMessage = this.client.GetAsync(requestUri).Result;
			responseMessage.EnsureSuccessStatusCode();
			var stream = responseMessage.Content.ReadAsStreamAsync().Result;
			using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
			{
				return jsonReader.ReadAsString();
			}
		}

		/// <summary>
		/// GET api/Values?name={name}
		/// </summary>
		public async Task<string> GetAsync(string name)
		{
			var requestUri = new Uri(this.baseUri, "api/Values?name=" + Uri.EscapeDataString(name));
			var responseMessage = await client.GetAsync(requestUri);
			responseMessage.EnsureSuccessStatusCode();
			var stream = await responseMessage.Content.ReadAsStreamAsync();
			using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
			{
				return jsonReader.ReadAsString();
			}
		}

		/// <summary>
		/// GET api/Values?name={name}
		/// </summary>
		public string Get(string name)
		{
			var requestUri = new Uri(this.baseUri, "api/Values?name=" + Uri.EscapeDataString(name));
			var responseMessage = this.client.GetAsync(requestUri).Result;
			responseMessage.EnsureSuccessStatusCode();
			var stream = responseMessage.Content.ReadAsStreamAsync().Result;
			using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
			{
				return jsonReader.ReadAsString();
			}
		}

		/// <summary>
		/// GET api/Values/{id}
		/// </summary>
		public async Task<string> GetAsync(int id)
		{
			var requestUri = new Uri(this.baseUri, "api/Values/" + id);
			var responseMessage = await client.GetAsync(requestUri);
			responseMessage.EnsureSuccessStatusCode();
			var stream = await responseMessage.Content.ReadAsStreamAsync();
			using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
			{
				return jsonReader.ReadAsString();
			}
		}

		/// <summary>
		/// GET api/Values/{id}
		/// </summary>
		public string Get(int id)
		{
			var requestUri = new Uri(this.baseUri, "api/Values/" + id);
			var responseMessage = this.client.GetAsync(requestUri).Result;
			responseMessage.EnsureSuccessStatusCode();
			var stream = responseMessage.Content.ReadAsStreamAsync().Result;
			using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
			{
				return jsonReader.ReadAsString();
			}
		}

		/// <summary>
		/// POST api/Values
		/// </summary>
		public async Task<string> PostAsync(string value)
		{
			var requestUri = new Uri(this.baseUri, "api/Values");
			using (var requestWriter = new System.IO.StringWriter())
			{
				var requestSerializer = JsonSerializer.Create();
				requestSerializer.Serialize(requestWriter, value);
				var content = new StringContent(requestWriter.ToString(), System.Text.Encoding.UTF8, "application/json");
				var responseMessage = await client.PostAsync(requestUri, content);
				responseMessage.EnsureSuccessStatusCode();
				var stream = await responseMessage.Content.ReadAsStreamAsync();
				using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
				{
					return jsonReader.ReadAsString();
				}
			}
		}

		/// <summary>
		/// POST api/Values
		/// </summary>
		public string Post(string value)
		{
			var requestUri = new Uri(this.baseUri, "api/Values");
			using (var requestWriter = new System.IO.StringWriter())
			{
				var requestSerializer = JsonSerializer.Create();
				requestSerializer.Serialize(requestWriter, value);
				var content = new StringContent(requestWriter.ToString(), System.Text.Encoding.UTF8, "application/json");
				var responseMessage = this.client.PostAsync(requestUri, content).Result;
				responseMessage.EnsureSuccessStatusCode();
				var stream = responseMessage.Content.ReadAsStreamAsync().Result;
				using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
				{
					return jsonReader.ReadAsString();
				}
			}
		}

		/// <summary>
		/// PUT api/Values/{id}
		/// </summary>
		public async Task PutAsync(int id, string value)
		{
			var requestUri = new Uri(this.baseUri, "api/Values/" + id);
			using (var requestWriter = new System.IO.StringWriter())
			{
				var requestSerializer = JsonSerializer.Create();
				requestSerializer.Serialize(requestWriter, value);
				var content = new StringContent(requestWriter.ToString(), System.Text.Encoding.UTF8, "application/json");
				var responseMessage = await client.PutAsync(requestUri, content);
				responseMessage.EnsureSuccessStatusCode();
			}
		}

		/// <summary>
		/// PUT api/Values/{id}
		/// </summary>
		public void Put(int id, string value)
		{
			var requestUri = new Uri(this.baseUri, "api/Values/" + id);
			using (var requestWriter = new System.IO.StringWriter())
			{
				var requestSerializer = JsonSerializer.Create();
				requestSerializer.Serialize(requestWriter, value);
				var content = new StringContent(requestWriter.ToString(), System.Text.Encoding.UTF8, "application/json");
				var responseMessage = this.client.PutAsync(requestUri, content).Result;
				responseMessage.EnsureSuccessStatusCode();
			}
		}
	}

}
