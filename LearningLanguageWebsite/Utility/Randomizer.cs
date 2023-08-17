using System;
using System.Linq;
using System.Security.Cryptography;

namespace LearningLanguageWebsite.Utility
{
    public static class Randomizer
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public static int Next()
        {
            var bytes = new byte[4];
            Rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static int Next(int minValue, int maxValue)
        {
            return RandomNumberGenerator.GetInt32(minValue, maxValue);
        }

        public static int Next(int maxValue)
        {
            if (maxValue <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue must be greater than zero.");
            }

            byte[] bytes = new byte[4];
            Rng.GetBytes(bytes);
            uint randomValue = BitConverter.ToUInt32(bytes, 0);
            return (int)(randomValue % maxValue);
        }

        public static string RandomString(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be zeor.", nameof(length));
            }

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz_-";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Next(s.Length)]).ToArray());
        }

        public static string RandomPassword(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be zeor.", nameof(length));
            }

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz(~!@#$%^&*_-+=`|(){}[]:;<>,.?/";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Next(s.Length)]).ToArray());
        }

        public static string RandomReadableString(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be zeor.", nameof(length));
            }

            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Next(s.Length)]).ToArray());
        }
    }
}
