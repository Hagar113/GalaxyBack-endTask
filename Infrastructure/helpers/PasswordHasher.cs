using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.helpers
{
    
    public class PasswordHasher
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        private static readonly int SaltSize = 16;

        private static readonly int HashSize = 20;

        private static readonly int Iterations = 10000;

        public static string HashPassword(string Password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[SaltSize]);

            var Key = new Rfc2898DeriveBytes(Password, salt, Iterations);
            var Hash = Key.GetBytes(HashSize);

            var HashBytes = new byte[SaltSize + HashSize];

            Array.Copy(salt, 0, HashBytes, 0, SaltSize);
            Array.Copy(Hash, 0, HashBytes, SaltSize, HashSize);

            var Base64Hash = Convert.ToBase64String(HashBytes);

            return Base64Hash;
        }

        public static bool VerifyPassword(string Password, string Base64Hash)
        {
            var HashBytes = Convert.FromBase64String(Base64Hash);

            var Salt = new byte[SaltSize];

            Array.Copy(HashBytes, 0, Salt, 0, SaltSize);

            var Key = new Rfc2898DeriveBytes(Password, Salt, Iterations);

            byte[] Hash = Key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (HashBytes[i + SaltSize] != Hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
