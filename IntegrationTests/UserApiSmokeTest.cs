using System.Collections.Generic;
using System.Net.Http.Headers;
using API.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using Utils;

namespace Tests
{
	public class UserApiSmokeTest : TestsBase
	{
		[SetUp]
		public new void Setup()
		{
			base.Setup();
		}

		[Test]
		public void Test()
		{
			var user = new UserDto
			{
				EmailAddress = $"user-{StringUtils.RandomString(true, false, 8).ToLower()}@mail.com",
				Password = "very_strong_password",
				FirstName = "John",
				LastName = "Smith"
			};

			// 1 - Register user
			{
				var userId = HttpPost($"{ApiBaseUrl}/user", user);
				Assert.IsNotEmpty(userId);

				var created = HttpGet<UserDto>($"{ApiBaseUrl}/user/{userId}");
				Assert.NotNull(created);
				Assert.AreEqual(user.EmailAddress, created.EmailAddress);
				Assert.AreEqual(user.FirstName, created.FirstName);
				Assert.AreEqual(user.LastName, created.LastName);
				Assert.Null(created.Password); // :)
			}

			// 2 - Get token
			var tokenJson = HttpPost($"{ApiBaseUrl}/user/auth", user);
			Assert.IsNotEmpty(tokenJson);

			var token = (TokenDto)JsonConvert.DeserializeObject(tokenJson, typeof(TokenDto));
			Assert.NotNull(token.Token);
			Assert.True(token.ExpiresInSec > 0);

			// 3 - Try to use token
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
			var devices = HttpGet<IList<DeviceDto>>($"{ApiBaseUrl}/device");
			Assert.NotNull(devices);
		}
	}
}