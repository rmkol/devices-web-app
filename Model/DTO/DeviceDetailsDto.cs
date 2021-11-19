using System;

namespace API.DTO
{
	public class DeviceDetailsDto
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public DeviceTypeDto Type { get; set; }
		public string Status { get; set; }
		public string Description { get; set; }
		public DateTime CreatedTimestamp { get; set; }
		public DateTime? UpdatedTimestamp { get; set; }

		public DeviceDetailsDto() { }

		public DeviceDetailsDto(int id, string name, DeviceTypeDto type, string status,
			string description, DateTime createdTimestamp, DateTime? updatedTimestamp)
		{
			Id = id;
			Name = name;
			Type = type;
			Status = status;
			Description = description;
			CreatedTimestamp = createdTimestamp;
			UpdatedTimestamp = updatedTimestamp;
		}
	}
}