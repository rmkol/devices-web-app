using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/device")]
	[ApiController]
	public class DeviceController : ControllerBase
	{
		private readonly IDeviceService _deviceService;

		public DeviceController(IDeviceService deviceService)
		{
			_deviceService = deviceService;
		}

		[HttpGet]
		public async Task<IEnumerable<DeviceDto>> Get()
		{
			return await _deviceService.GetAllAsync();
		}

		[HttpGet("{deviceId:int}")]
		public async Task<DeviceDetailsDto> Get(int deviceId)
		{
			return await _deviceService.GetByIdAsync(deviceId);
		}

		[HttpPost]
		public async Task<DeviceDetailsDto> Create(DeviceDetailsDto dto)
		{
			var deviceId = await _deviceService.AddAsync(dto);
			return await _deviceService.GetByIdAsync(deviceId);
		}

		[HttpPut("{deviceId:int}")]
		public async Task<DeviceDetailsDto> Update(int deviceId, DeviceDetailsDto dto)
		{
			await _deviceService.UpdateAsync(deviceId, dto);
			return await _deviceService.GetByIdAsync(deviceId);
		}

		[HttpDelete("{deviceId:int}")]
		public async Task Delete(int deviceId)
		{
			await _deviceService.DeleteAsync(deviceId);
		}

		[HttpGet("search")]
		public async Task<IEnumerable<DeviceDto>> SearchByName(string name)
		{
			return await _deviceService.SearchDevicesByName(name);
		}

		[HttpPost("/enable{deviceId:int}")]
		public async Task EnableDevice(int deviceId)
		{
			await _deviceService.ChangeDeviceStatus(deviceId, DeviceStatus.Active);
		}
	}
}