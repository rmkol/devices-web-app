namespace API.DTO
{
	public class UserDto
	{
		public int? Id { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public UserDto() { }

		public UserDto(int? id, string emailAddress, string password, string firstName, string lastName)
		{
			Id = id;
			EmailAddress = emailAddress;
			Password = password;
			FirstName = firstName;
			LastName = lastName;
		}
	}
}