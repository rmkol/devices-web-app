using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using API.DTO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
	public class DeviceApiTest
	{
		private const string _ApiBaseUrl = "https://localhost:5001/api";

		private HttpClient _HttpClient;

		[SetUp]
		public void Setup()
		{
			_HttpClient = new HttpClient();
		}

		[Test]
		public void GetAllDevices()
		{
			var devices = HttpGet<IList<DeviceDto>>($"{_ApiBaseUrl}/device");
			Assert.True(devices.Any());
			Assert.True(devices.First().Id > 0);
		}

		[Test]
		public void CreateDevice_EmptyName()
		{
			var device = new DeviceDetailsDto
			{
				Name = ""
			};
			var ex = Assert.Throws<ApplicationException>(() => HttpPost($"{_ApiBaseUrl}/device", device));
			Assert.That(ex.Message.Contains("type id is required"));
		}

		private string HttpPost<TBody>(string url, TBody body)
		{
			var response = _HttpClient.PostAsync(url,
				new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8,
					"application/json"));
			if (response.IsCompletedSuccessfully)
			{
				return response.Result.Content.ReadAsStringAsync().Result;
			}
			throw new ApplicationException($"POST \"{url}\" returned bad status code - {response.Result.StatusCode}. " +
										   $"Error: {response.Result.Content.ReadAsStringAsync().Result}");
		}

		private TReply HttpGet<TReply>(string url)
		{
			var response = _HttpClient.GetAsync(url).Result;
			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<TReply>(response.Content.ReadAsStringAsync().Result);
			}
			throw new ApplicationException($"GET \"{url}\" returned bad status code - {response.StatusCode}. " +
										   $"Error: {response.Content.ReadAsStringAsync().Result}");
		}
	}
}