using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace gatewayapi
{
	public class JwtAuthMiddleware
	{
		private readonly RequestDelegate _next;
		public JwtAuthMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			var controllerName = context.Request.RouteValues["controller"].ToString();
			if (controllerName != "Auth")
			{
				var header = context.Request.Headers["Authorization"].FirstOrDefault();
				if (header is null)
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					await context.Response.WriteAsync("Missing token");
					return;
				}

				var token = header.Split(" ").Last();
				var tokenHandler = new JwtSecurityTokenHandler();

				var validationParameters = new TokenValidationParameters()
				{
					IssuerSigningKey =
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes("keykeykeykeykeykeykeykeykeykey12")),
					ValidateAudience = false,
					ValidateIssuer = false
				};

				try
				{
					tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
				}
				catch
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					await context.Response.WriteAsync("Invalid token");
					return;
				}

			}

			await _next(context);
		}
	}
}
