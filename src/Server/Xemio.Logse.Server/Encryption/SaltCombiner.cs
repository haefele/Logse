using System.Security.Cryptography;

namespace Xemio.Logse.Server.Encryption
{
    public static class SaltCombiner
    {
        /// <summary>
        /// Combines the specified <paramref name="salt"/> with the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        public static byte[] Combine(byte[] salt, string password)
        {
            using (var hasher = new Rfc2898DeriveBytes(password, salt))
            {
                return hasher.GetBytes(128);
            }
        }
    }
}