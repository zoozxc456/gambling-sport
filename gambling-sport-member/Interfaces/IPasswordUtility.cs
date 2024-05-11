using System;
namespace gamblingsportmember.Interfaces
{
	public interface IPasswordUtility
	{
        public bool VerifyPassword(string rawPassword, string dbPassword);
        public string HashPassword(string rawPassword);
    }
}

