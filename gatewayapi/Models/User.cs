using Microsoft.IdentityModel.Tokens;

namespace gatewayapi.Models
{
	public class User
	{
		public string Login { get; set; } = null!;
		public string Password { get; set; } = null!;
		public SecurityToken? Token { get; set; }
	}
}
