using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Helpers
{
    public static class PasswordHelper
    {
        public static byte[] ComputeHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPassword(string enteredPassword, byte[] storedHash)
        {
            var enteredHash = ComputeHash(enteredPassword);
            return enteredHash.SequenceEqual(storedHash);
        }
    }
}
