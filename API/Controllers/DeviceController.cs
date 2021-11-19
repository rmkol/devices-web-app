using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Core;
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
		private const string _DeviceManagerRole = "device_manager"; // todo: this should not be here

		private readonly IRequestContext _requestContext;
		private readonly IDeviceService _deviceService;
		private readonly IUserService _userService;

		public DeviceController(IRequestContext requestContext, IDeviceService deviceService,
			IUserService userService)
		{
			_requestContext = requestContext;
			_deviceService = deviceService;
			_userService = userService;
		}

		[HttpGet("type")]
		public async Task<IEnumerable<DeviceTypeDto>> GetAllDeviceTypes()
		{
			return await _deviceService.GetAllDeviceTypes();
		}

		[HttpGet]
		public async Task<IEnumerable<DeviceDto>> GetAllDevices()
		{
			return await _deviceService.GetAllDevicesAsync();
		}

		[HttpGet("{deviceId:int}")]
		public async Task<DeviceDetailsDto> GetDeviceById(int deviceId)
		{
			return await _deviceService.GetDeviceByIdAsync(deviceId);
		}

		[HttpPost]
		public async Task<int> CreateDevice(DeviceDetailsDto dto)
		{
			return await _deviceService.AddDeviceAsync(dto);
		}

		[HttpPut("{deviceId:int}")]
		public async Task<DeviceDetailsDto> UpdateDevice(int deviceId, DeviceDetailsDto dto)
		{
			await _deviceService.UpdateDeviceAsync(deviceId, dto);
			return await _deviceService.GetDeviceByIdAsync(deviceId);
		}

		[HttpDelete("{deviceId:int}")]
		public async Task DeleteDevice(int deviceId)
		{
			await _deviceService.DeleteDeviceAsync(deviceId);
		}

		[HttpGet("search")]
		public async Task<IEnumerable<DeviceDto>> SearchDeviceByName(string name)
		{
			return await _deviceService.SearchDevicesByName(name);
		}

		[HttpPost("{deviceId:int}/enable")]
		public async Task EnableDevice(int deviceId)
		{
			// todo: merge with "DisableDevice"
			// todo: move authorization to middleware
			if (await _userService.UserHasRole(_requestContext.User.Id, _DeviceManagerRole))
			{
				await _deviceService.ChangeDeviceStatus(deviceId, DeviceStatus.Active);
				return;
			}
			throw new UnauthorizedAccessException($"Operation is not allowed for user {_requestContext.User.Id}");
		}

		[HttpPost("{deviceId:int}/disable")]
		public async Task DisableDevice(int deviceId)
		{
			if (await _userService.UserHasRole(_requestContext.User.Id, _DeviceManagerRole))
			{
				await _deviceService.ChangeDeviceStatus(deviceId, DeviceStatus.Disabled);
				return;
			}
			throw new UnauthorizedAccessException($"Operation is not allowed for user {_requestContext.User.Id}");
		}
	}
}