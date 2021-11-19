namespace API.DTO
{
	public class TokenDto
	{
		public string Token { get; set; }
		public long ExpiresInSec { get; set; }
	}
}