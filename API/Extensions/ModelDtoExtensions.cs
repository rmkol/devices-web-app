using API.Data;
using API.DTO;

namespace API.Extensions
{
	public static class ModelDtoExtensions
	{
		public static UserDto ToDto(this User user)
		{
			return new UserDto(id: user.Id,
				emailAddress: user.EmailAddress,
				firstName: user.FirstName,
				lastName: user.LastName,
				password: null);
		}

		public static DeviceTypeDto ToDto(this DeviceType deviceType)
		{
			return new DeviceTypeDto(id: deviceType.Id,
				name: deviceType.Name,
				description: deviceType.Description);
		}

		public static DeviceDto ToDto(this Device device, DeviceType deviceType)
		{
			return new DeviceDto(id: device.Id,
				name: device.Name,
				type: deviceType?.ToDto(),
				status: device.Status?.ToString());
		}

		public static DeviceDetailsDto ToDto(this DeviceDetails details, DeviceType deviceType)
		{
			return new DeviceDetailsDto(id: details.Id,
				name: details.Name,
				type: deviceType?.ToDto(),
				status: details.Status?.ToString(),
				description: details.Description,
				createdTimestamp: details.CreatedTimestamp,
				updatedTimestamp: details.UpdatedTimestamp);
		}
	}
}