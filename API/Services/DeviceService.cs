using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Exceptions;
using API.Extensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class DeviceService : IDeviceService
	{
		private readonly IDevicesDbContext _db;

		public DeviceService(IDevicesDbContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
		{
			return await (from d in _db.Devices
						  join dt in _db.DeviceTypes on d.TypeId equals dt.Id
						  select d.ToDto(dt)).ToListAsync();
		}

		public async Task<DeviceDetailsDto> GetDeviceByIdAsync(int id)
		{
			return await (from dd in _db.DeviceDetails
						  join dt in _db.DeviceTypes on dd.TypeId equals dt.Id
						  where dd.Id == id
						  select dd.ToDto(dt)).FirstOrDefaultAsync();
		}

		public async Task<int> AddDeviceAsync(DeviceDetailsDto dto)
		{
			ValidateDeviceDetailsForAddUpdate(dto);

			var dTypeId = dto.Type.Id;
			var dType = _db.DeviceTypes.FirstOrDefaultAsync(dt => dt.Id == dTypeId);
			if (dType == null)
			{
				throw new InvalidArgumentException($"Device type {dTypeId} doesn't exist.");
			}

			var deviceDetails = new DeviceDetails
			{
				Name = dto.Name,
				Description = dto.Description,
				TypeId = dTypeId.Value
			};

			await _db.AddAsync(deviceDetails);
			await _db.SaveChangesAsync();

			return deviceDetails.Id;
		}

		public async Task UpdateDeviceAsync(int deviceId, DeviceDetailsDto dto)
		{
			ValidateDeviceDetailsForAddUpdate(dto);

			var device = new DeviceDetails { Id = deviceId };
			_db.DeviceDetails.Attach(device);

			device.Name = dto.Name;
			device.Description = dto.Description;

			var dTypeId = dto.Type.Id;
			var dType = await _db.DeviceTypes.FirstOrDefaultAsync(dt => dt.Id == dTypeId);
			if (dType == null)
			{
				throw new InvalidArgumentException($"Device type {dTypeId} doesn't exist.");
			}
			device.TypeId = dTypeId.Value;

			await _db.SaveChangesAsync();
		}

		public async Task DeleteDeviceAsync(int deviceId)
		{
			var device = await GetDeviceById(deviceId);

			device.Status = DeviceStatus.Deleted;
			// todo rk delete child devices as well? if any

			await _db.SaveChangesAsync();
		}

		public async Task<IEnumerable<DeviceDto>> SearchDevicesByName(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<DeviceDto>();

			name = name.ToLower();
			return await (from d in _db.Devices
						  join dt in _db.DeviceTypes on d.TypeId equals dt.Id
						  where d.Name.ToLower().Contains(name)
						  select d.ToDto(dt)).ToListAsync();
		}

		public async Task ChangeDeviceStatus(int deviceId, DeviceStatus status)
		{
			var device = await GetDeviceById(deviceId);
			if (device.Status == status)
			{
				return;
			}
			device.Status = status;
			await _db.SaveChangesAsync();
		}

		public async Task<IEnumerable<DeviceTypeDto>> GetAllDeviceTypes()
		{
			return await (from dt in _db.DeviceTypes
						  select dt.ToDto()).ToListAsync();
		}

		private async Task<Device> GetDeviceById(int deviceId)
		{
			var device = await _db.Devices.FirstOrDefaultAsync(d => d.Id == deviceId);
			if (device == null || device.Status == DeviceStatus.Deleted)
			{
				throw new InvalidArgumentException($"Device {deviceId} doesn't exist.");
			}
			return device;
		}

		private static void ValidateDeviceDetailsForAddUpdate(DeviceDetailsDto dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Name))
			{
				throw new InvalidArgumentException("Device name cannot be empty.");
			}
		}
	}
}