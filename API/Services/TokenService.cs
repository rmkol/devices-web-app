using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace API.Services
{
	public class TokenService : ITokenService
	{
		private const int _TokenLength = 64;
		private const long _TokenTtlSec = 3600; // todo: move to settings

		private readonly IDevicesDbContext _db;

		public TokenService(IDevicesDbContext db)
		{
			_db = db;
		}

		public async Task<Token> IssueNewTokenForUser(User user)
		{
			var tokens = _db.Tokens.Where(t => t.UserId == user.Id).ToList();
			foreach (var token in tokens)
			{
				_db.Tokens.Remove(token);
			}

			var newToken = new Token
			{
				Value = StringUtils.RandomString(true, true, _TokenLength),
				TtlSec = _TokenTtlSec,
				UserId = user.Id
			};
			_db.Tokens.Add(newToken);

			await _db.SaveChangesAsync();

			return newToken;
		}

		public async Task<Token> GetTokenByValue(string token)
		{
			return await _db.Tokens.FirstOrDefaultAsync(t => t.Value == token);
		}
	}
}