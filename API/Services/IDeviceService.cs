using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTO;

namespace API.Services
{
	public interface IDeviceService
	{
		Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
		Task<DeviceDetailsDto> GetDeviceByIdAsync(int id);
		Task<int> AddDeviceAsync(DeviceDetailsDto dto);
		Task UpdateDeviceAsync(int deviceId, DeviceDetailsDto dto);
		Task DeleteDeviceAsync(int deviceId);
		Task<IEnumerable<DeviceDto>> SearchDevicesByName(string name);
		Task ChangeDeviceStatus(int deviceId, DeviceStatus status);
		Task<IEnumerable<DeviceTypeDto>> GetAllDeviceTypes();
	}
}