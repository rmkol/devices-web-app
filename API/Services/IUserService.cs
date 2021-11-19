using System.Threading.Tasks;
using API.DTO;

namespace API.Services
{
	public interface IUserService
	{
		Task<UserDto> GetUserByIdAsync(int userId);
		Task<int> RegisterUserAsync(UserDto dto);
		Task<TokenDto> AuthenticateUserAsync(UserDto dto);
		Task<bool> UserHasRole(int userId, string roleName);
	}
}