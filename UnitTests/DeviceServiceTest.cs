using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Exceptions;
using API.Services;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
	public class DeviceServiceTest
	{
		private IDeviceService _service;
		private Mock<IDevicesDbContext> _db;

		private readonly List<Device> _devices = new List<Device>();
		private readonly List<DeviceDetails> _deviceDetails = new List<DeviceDetails>();
		private readonly List<DeviceType> _deviceTypes = new List<DeviceType>();

		[SetUp]
		public void Setup()
		{
			_db = new Mock<IDevicesDbContext>();
			_service = new DeviceService(_db.Object);

			var devicesDbSet = _devices.AsQueryable().BuildMockDbSet();
			var deviceDetailsDbSet = _deviceDetails.AsQueryable().BuildMockDbSet();
			var deviceTypesDbSet = _deviceTypes.AsQueryable().BuildMockDbSet();

			_db.Setup(x => x.Devices).Returns(() => devicesDbSet.Object);
			_db.Setup(x => x.DeviceDetails).Returns(() => deviceDetailsDbSet.Object);
			_db.Setup(x => x.DeviceTypes).Returns(() => deviceTypesDbSet.Object);
		}

		#region Positive Test Cases

		[Test]
		public async Task CreateDevice()
		{
			var deviceId = await _service.AddAsync(new DeviceDetailsDto
			{
				Name = "Device 1",
				Type = new DeviceTypeDto { Id = 1 }
			});
			Assert.AreEqual(0, deviceId);
		}

		[Test]
		public async Task GetDevice()
		{
			const int deviceId = 1;
			const int deviceTypeId = 1;
			const string deviceName = "Device 1";

			AddDevices(new DeviceDetails
			{
				Id = deviceId,
				Name = deviceName,
				TypeId = deviceTypeId,
				Status = DeviceStatus.Disabled
			});
			AddDeviceTypes(new DeviceType
				{
					Id = deviceId,
					Name = deviceName,
				}
			);

			var result = await _service.GetByIdAsync(deviceTypeId);
			Assert.AreEqual(deviceId, result.Id);
			Assert.AreEqual(deviceName, result.Name);
			Assert.AreEqual(DeviceStatus.Disabled.ToString(), result.Status);

			Assert.NotNull(result.Type);
			Assert.AreEqual(deviceTypeId, result.Type.Id);
		}

		#endregion Positive Test Cases

		#region Negative Test Cases

		[Test]
		public void CreateDevice_EmptyName()
		{
			Assert.ThrowsAsync<InvalidArgumentException>(() => _service.AddAsync(new DeviceDetailsDto
			{
				Name = null,
				Type = new DeviceTypeDto { Id = 1 }
			}));
			Assert.ThrowsAsync<InvalidArgumentException>(() => _service.AddAsync(new DeviceDetailsDto
			{
				Name = "",
				Type = new DeviceTypeDto { Id = 1 }
			}));
			Assert.ThrowsAsync<InvalidArgumentException>(() => _service.AddAsync(new DeviceDetailsDto
			{
				Name = " ",
				Type = new DeviceTypeDto { Id = 1 }
			}));
		}

		#endregion Negative Test Cases

		private void AddDevices(params Device[] devices)
		{
			_devices.AddRange(devices);
		}

		private void AddDevices(params DeviceDetails[] devices)
		{
			_deviceDetails.AddRange(devices);
		}

		private void AddDeviceTypes(params DeviceType[] deviceTypes)
		{
			_deviceTypes.AddRange(deviceTypes);
		}
	}
}