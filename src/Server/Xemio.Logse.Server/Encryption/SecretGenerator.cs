using System;
using System.Security.Cryptography;

namespace Xemio.Logse.Server.Encryption
{
    public static class SecretGenerator
    {
        /// <summary>
        /// Generates the string.
        /// </summary>
        /// <param name="length">The length.</param>
        public static string GenerateString(int length = 128)
        {
            byte[] randomData = Generate((length / 4 * 3) + 1);
            return Convert.ToBase64String(randomData).Substring(0, length).Replace('/', '-').Replace('+', '_');
        }
        /// <summary>
        /// Generates a new secret.
        /// </summary>
        public static byte[] Generate(int length = 128)
        {
            byte[] randomBytes = new byte[length];
            RandomNumberGenerator.Create().GetNonZeroBytes(randomBytes);

            return randomBytes;
        }
    }
}