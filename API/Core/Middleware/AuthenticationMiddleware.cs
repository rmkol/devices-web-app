using System.Threading.Tasks;
using API.Data;
using API.Services;
using Microsoft.AspNetCore.Http;

namespace API.Core.Middleware
{
	public class AuthenticationMiddleware
	{
		private readonly RequestDelegate _next;

		public AuthenticationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, IRequestContext requestContext,
			ITokenService tokenService, IUserService userService)
		{
			string authHeader = context.Request.Headers["Authorization"];
			if (authHeader != null && authHeader.StartsWith("Bearer"))
			{
				var tokenStr = authHeader.Substring("Bearer ".Length).Trim();
				var token = await tokenService.GetTokenByValue(tokenStr);
				if (token != null && !token.IsExpired)
				{
					var userDto = await userService.GetUserByIdAsync(token.UserId); // todo rk rewrite; make service return entity instead of dto?
					requestContext.User = new User { Id = userDto.Id.Value };
					await _next.Invoke(context);
				}
				else
				{
					context.Response.StatusCode = 401;
				}
			}
			else
			{
				context.Response.StatusCode = 401;
			}
		}
	}
}