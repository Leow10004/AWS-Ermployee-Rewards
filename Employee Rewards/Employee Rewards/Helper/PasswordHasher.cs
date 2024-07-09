using System.Security.Cryptography;
using System.Text;

namespace MyProject.Helpers
{
    public static class PasswordHasher
    {
        public static byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
                return sha256.ComputeHash(saltedPassword);
            }
        }
    }
}