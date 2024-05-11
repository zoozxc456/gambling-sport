using System;
namespace gamblingsportmember.Models
{
	public class JwtMemberModel
	{
		public string Role { get; set; } = string.Empty;
		public string Account { get; set; } = string.Empty;
		public Guid Id { get; set; }
	}
}

