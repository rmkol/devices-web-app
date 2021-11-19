using System.Threading.Tasks;
using API.Data;

namespace API.Services
{
	public interface ITokenService
	{
		Task<Token> IssueNewTokenForUser(User user);
		Task<Token> GetTokenByValue(string token);
	}
}