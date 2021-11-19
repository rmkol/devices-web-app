using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Exceptions;
using API.Extensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class UserService : IUserService
	{
		private readonly IDevicesDbContext _db;
		private readonly ITokenService _tokenService;

		public UserService(IDevicesDbContext db, ITokenService tokenService)
		{
			_tokenService = tokenService;
			_db = db;
		}

		public async Task<UserDto> GetUserByIdAsync(int userId)
		{
			var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null)
			{
				throw new InvalidArgumentException($"User {userId} doesn't exist.");
			}
			return user.ToDto();
		}

		public async Task<int> RegisterUserAsync(UserDto dto)
		{
			var user = new User
			{
				EmailAddress = dto.EmailAddress,
				Password = ComputeSha256Hash(dto.Password),
				FirstName = dto.FirstName,
				LastName = dto.LastName
			};

			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();

			return user.Id;
		}

		public async Task<TokenDto> AuthenticateUserAsync(UserDto dto)
		{
			var passwordHash = ComputeSha256Hash(dto.Password);

			var user = await _db.Users.FirstOrDefaultAsync(u =>
				u.EmailAddress == dto.EmailAddress && u.Password == passwordHash);
			if (user == null)
			{
				throw new BadCredentialsException("Bad credentials.");
			}

			var token = await _tokenService.IssueNewTokenForUser(user);
			return new TokenDto { Token = token.Value, ExpiresInSec = token.TtlSec };
		}

		public async Task<bool> UserHasRole(int userId, string roleName)
		{
			return await (from u in _db.Users
						  join ur in _db.UserRoles on u.Id equals ur.UserId
						  join r in _db.Roles on ur.RoleId equals r.Id
						  where u.Id == userId && r.Name == roleName
						  select r).AnyAsync();
		}

		private static string ComputeSha256Hash(string str)
		{
			using (var sha256Hash = SHA256.Create())
			{
				var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
				var builder = new StringBuilder();
				foreach (var b in bytes)
				{
					builder.Append(b.ToString("x2"));
				}
				return builder.ToString();
			}
		}
	}
}