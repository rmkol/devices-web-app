namespace API.DTO
{
	public class DeviceTypeDto
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public DeviceTypeDto() { }

		public DeviceTypeDto(int? id, string name, string description)
		{
			Id = id;
			Name = name;
			Description = description;
		}
	}
}