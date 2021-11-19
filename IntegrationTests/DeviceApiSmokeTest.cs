using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using API.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using static Utils.StringUtils;

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
			LogStep("Get auth token.");
			AuthAs(_IsaacTestUser);

			// 2 - Get devices types
			LogStep("Get device types.");
			var deviceTypes = HttpGet<IList<DeviceTypeDto>>($"{ApiBaseUrl}/device/type");
			Assert.NotNull(deviceTypes);
			Assert.True(deviceTypes.Any());

			var cameraType = deviceTypes.FirstOrDefault(dt => dt.Name.ToLower().Contains("camera"));
			Assert.NotNull(cameraType?.Name);

			// 3 - Add new device
			LogStep("Add new device.");
			var device = new DeviceDetailsDto
			{
				Name = $"TestDevice-{RandomString(false, true, 8)}",
				Description = "Just a test device",
				Type = new DeviceTypeDto { Id = cameraType.Id.Value },
				Status = "Active"
			};
			var deviceId = HttpPost($"{ApiBaseUrl}/device", device);
			Assert.IsNotEmpty(deviceId);

			// 4 - Get device by id
			LogStep("Get device by id.");
			var created = HttpGet<DeviceDetailsDto>($"{ApiBaseUrl}/device/{deviceId}");
			Assert.NotNull(created);
			Assert.AreEqual(device.Name, created.Name);
			Assert.AreEqual(device.Description, created.Description);
			Assert.NotNull(created.Type);
			Assert.AreEqual(device.Type.Id, created.Type.Id);
			Assert.AreEqual(device.Status, created.Status);

			// 5 - Update device
			LogStep("Update device.");
			device.Name = $"{device.Name}-new";
			HttpPut($"{ApiBaseUrl}/device/{deviceId}", device);

			var updated = HttpGet<DeviceDetailsDto>($"{ApiBaseUrl}/device/{deviceId}");
			Assert.NotNull(updated);
			Assert.AreEqual(device.Name, updated.Name);

			// 6 - Get all devices
			LogStep("Get all devices.");
			var devices = HttpGet<IList<DeviceDto>>($"{ApiBaseUrl}/device");
			Assert.True(devices.Any());
			Assert.True(devices.First().Id > 0);
			Assert.NotNull(devices.FirstOrDefault(d => $"{d.Id}" == deviceId));

			// 7 - Search devices by name
			LogStep("Search devices by name.");
			var found = HttpGet<IList<DeviceDto>>($"{ApiBaseUrl}/device?name=ca");
			Assert.True(found.Any());
			Assert.NotNull(found.FirstOrDefault(d => d.Name == device.Name));

			// 8 - Disable
			LogStep("Disable device.");
			HttpPost($"{ApiBaseUrl}/device/{deviceId}/disable", null);
			updated = HttpGet<DeviceDetailsDto>($"{ApiBaseUrl}/device/{deviceId}");
			Assert.NotNull(updated);
			Assert.AreEqual("Disabled", updated.Status);

			// 9 - Enable
			LogStep("Enable device.");
			HttpPost($"{ApiBaseUrl}/device/{deviceId}/enable", null);
			updated = HttpGet<DeviceDetailsDto>($"{ApiBaseUrl}/device/{deviceId}");
			Assert.NotNull(updated);
			Assert.AreEqual("Active", updated.Status);

			// 10 - Delete device
			LogStep("Delete device.");
			HttpDelete($"{ApiBaseUrl}/device/{deviceId}");
			var deleted = HttpGet<DeviceDetailsDto>($"{ApiBaseUrl}/device/{deviceId}");
			Assert.Null(deleted);
		}

		[Test]
		public void CreateDevice_EmptyName()
		{
			AuthAs(_IsaacTestUser);
			
			var device = new DeviceDetailsDto
			{
				Name = ""
			};
			var ex = Assert.Throws<ApplicationException>(() => HttpPost($"{ApiBaseUrl}/device", device));
			Assert.That(ex.Message.Contains("name cannot be empty"));
		}
		
		private void AuthAs(UserDto user)
		{
			var tokenJson = HttpPost($"{ApiBaseUrl}/user/auth", user);
			Assert.IsNotEmpty(tokenJson);

			var token = (TokenDto)JsonConvert.DeserializeObject(tokenJson, typeof(TokenDto));
			Assert.NotNull(token.Token);
			Assert.True(token.ExpiresInSec > 0);

			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
		}
	}
}