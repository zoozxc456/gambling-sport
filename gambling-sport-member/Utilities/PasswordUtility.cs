using System;
using System.Text;
using System.Security.Cryptography;
using gamblingsportmember.Interfaces;

namespace gamblingsportmember.Utilities
{
    public class PasswordUtility : IPasswordUtility
    {
        public PasswordUtility()
        {
        }
        public bool VerifyPassword(string rawPassword, string dbPassword)
        {
            var hashPassword = HashPassword(rawPassword);
            return hashPassword == dbPassword;
        }

        public string HashPassword(string rawPassword)
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(rawPassword);
            var hashBytes = sha256.ComputeHash(bytes);

            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            var hashPassword = sb.ToString();
            return hashPassword;
        }
    }
}

