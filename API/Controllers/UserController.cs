using System.Threading.Tasks;
using API.DTO;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("{userId:int}")]
		public async Task<UserDto> Get(int userId)
		{
			return await _userService.GetUserByIdAsync(userId);
		}

		[HttpPost]
		public async Task<int> Create(UserDto dto)
		{
			return await _userService.RegisterUserAsync(dto);
		}

		[HttpPost("auth")]
		public async Task<TokenDto> AuthenticateUser(UserDto dto)
		{
			return await _userService.AuthenticateUserAsync(dto);
		}
	}
}