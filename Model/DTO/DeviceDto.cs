namespace API.DTO
{
	public class DeviceDto
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public DeviceTypeDto Type { get; set; }
		public string Status { get; set; }

		public DeviceDto() { }

		public DeviceDto(int id, string name, string status, DeviceTypeDto type)
		{
			Id = id;
			Name = name;
			Type = type;
			Status = status;
		}
	}
}