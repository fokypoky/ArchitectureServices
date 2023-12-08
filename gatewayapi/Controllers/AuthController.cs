using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using gatewayapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Net.Mime.MediaTypeNames;

namespace gatewayapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private static List<User> Users { get; } = new List<User>()
		{
			new User() {Login = "admin", Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5"}, // 12345 SHA-256
			new User() {Login = "user2023", Password = "d398b29d3dbbb9bf201d4c7e1c19ff9d43c15fd45a0cec46fbe9885ec3f6e97f"} // 2023 SHA-256
		};

		[HttpGet]
		public ActionResult<string> Get(string login, string password)
		{
			string passwordHash = GetHash(password);

			var user = Users.FirstOrDefault(u => u.Login == login && u.Password == passwordHash);

			if (user == null)
			{
				return BadRequest("Incorrect login or password");
			}

			int userIndex = Users.IndexOf(user);
			var tokenHandler = new JwtSecurityTokenHandler();

			if (user.Token == null || DateTime.Now >= user.Token.ValidTo)
			{
				var key = Encoding.UTF8.GetBytes("keykeykeykeykeykeykeykeykeykey12");

				var descriptor = new SecurityTokenDescriptor()
				{
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
					Expires = DateTime.Now.AddDays(1),
				};

				user.Token = tokenHandler.CreateToken(descriptor);
				Users[userIndex] = user;
			}

			return Ok(tokenHandler.WriteToken(user.Token));
		}

		private string GetHash(string input)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(input));
				
				var hashStringBuilder = new StringBuilder();
				
				foreach (var hashByte in hashBytes)
				{
					hashStringBuilder.Append(hashByte.ToString("x2"));
				}

				return hashStringBuilder.ToString();
			}
		} 
	}
}
