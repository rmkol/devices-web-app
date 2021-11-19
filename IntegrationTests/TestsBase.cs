using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class TestsBase
	{
		protected const string ApiBaseUrl = "https://localhost:5001/api";

		protected HttpClient HttpClient;

		protected void Setup()
		{
			HttpClient = new HttpClient();
		}

		protected string HttpPost<TBody>(string url, TBody body)
		{
			var response = HttpClient.PostAsync(url,
				new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8,
					"application/json"));
			if (response.Result.IsSuccessStatusCode)
			{
				return response.Result.Content.ReadAsStringAsync().Result;
			}
			throw new ApplicationException($"POST \"{url}\" returned bad status code - {response.Result.StatusCode}. " +
										   $"Error: {response.Result.Content.ReadAsStringAsync().Result}");
		}

		protected TReply HttpGet<TReply>(string url)
		{
			var response = HttpClient.GetAsync(url).Result;
			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<TReply>(response.Content.ReadAsStringAsync().Result);
			}
			throw new ApplicationException($"GET \"{url}\" returned bad status code - {response.StatusCode}. " +
										   $"Error: {response.Content.ReadAsStringAsync().Result}");
		}
	}
}