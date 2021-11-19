using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using API.DTO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
	public class DeviceApiSmokeTest : TestsBase
	{
		private static readonly UserDto _IsaacTestUser = new UserDto
		{
			EmailAddress = "isw@mail.com",
			Password = "very_strong_password"
		};

		[SetUp]
		public new void Setup()
		{
			base.Setup();
		}

		[Test]
		public void Test()
		{
			// 1 - Get token
			var tokenJson = HttpPost($"{ApiBaseUrl}/user/auth", _IsaacTestUser);
			Assert.IsNotEmpty(tokenJson);

			var token = (TokenDto)JsonConvert.DeserializeObject(tokenJson, typeof(TokenDto));
			Assert.NotNull(token.Token);
			Assert.True(token.ExpiresInSec > 0);
			
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

			// 2 - Add new device
			var device = new DeviceDetailsDto
			{
				Name = "TestDevice"
			};
			var deviceId = HttpPost($"{ApiBaseUrl}/device", device);
			
			// Get device by id
			
			// Get all devices
			var devices = HttpGet<IList<DeviceDto>>($"{ApiBaseUrl}/device");
			Assert.True(devices.Any());
			Assert.True(devices.First().Id > 0);
			
			// Search by device name
			
			// Enable
			// Disable
		}

		[Test]
		public void CreateDevice_EmptyName()
		{
			var device = new DeviceDetailsDto
			{
				Name = ""
			};
			var ex = Assert.Throws<ApplicationException>(() => HttpPost($"{ApiBaseUrl}/device", device));
			Assert.That(ex.Message.Contains("type id is required"));
		}
	}
}