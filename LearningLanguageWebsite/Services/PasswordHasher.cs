using LearningLanguageWebsite.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace LearningLanguageWebsite.Services
{
	public class PasswordHasher : IPasswordHasher
	{
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32; // 256 bits
        private const int Iterations = 10000; // iteration

        public bool Check(string hash, string password)
        {
            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(password))
                return false;

            byte[] salt = new byte[SaltSize];
            byte[] decodedHashedPassword = Convert.FromBase64String(hash);
            Buffer.BlockCopy(decodedHashedPassword, 0, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[KeySize];
            Buffer.BlockCopy(decodedHashedPassword, salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, KeySize);

            return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
        }

        public string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] subkey = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize);

            byte[] finalPassword = new byte[salt.Length + subkey.Length];
            Buffer.BlockCopy(salt, 0, finalPassword, 0, salt.Length);
            Buffer.BlockCopy(subkey, 0, finalPassword, salt.Length, subkey.Length);

            return Convert.ToBase64String(finalPassword);
        }
    }
}
