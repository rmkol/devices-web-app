using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTO;

namespace API.Services
{
	public interface IDeviceService
	{
		Task<IEnumerable<DeviceDto>> GetAllAsync();
		Task<DeviceDetailsDto> GetByIdAsync(int id);
		Task<int> AddAsync(DeviceDetailsDto dto);
		Task UpdateAsync(int deviceId, DeviceDetailsDto dto);
		Task DeleteAsync(int deviceId);
		Task<IEnumerable<DeviceDto>> SearchDevicesByName(string name);
		Task ChangeDeviceStatus(int deviceId, DeviceStatus status);
	}
}