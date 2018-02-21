using System.Linq;
using System.Security.Cryptography;

namespace Sunflower.Business.Security
{
    /// <summary>
    /// Represents a password that will be stored as a combination of hash and salt.
    /// </summary>
    public sealed class HashedPassword
    {
        /// <summary>
        /// Recreates a stored password from its hash and its salt.
        /// </summary>
        /// <param name="hash">Hash that has been stored for the password.</param>
        /// <param name="salt">Salt that has been stored for the password.</param>
        public HashedPassword(byte[] hash, byte[] salt)
        {
            Hash = hash;
            Salt = salt;
        }

        /// <summary>
        /// Hash that is stored for the password.
        /// </summary>
        public byte[] Hash { get; }

        /// <summary>
        /// Salt that is stored for the password.
        /// </summary>
        public byte[] Salt { get; }

        /// <summary>
        /// Creates a HashedPassword from a password contained in a plain string.
        /// </summary>
        /// <param name="plainPassword">String that contains the password.</param>
        /// <returns>An instance of HashedPassword that represents the specified password.</returns>
        public static HashedPassword CreateFromPlainPassword(string plainPassword)
        {
            var salt = GenerateSalt();
            return new HashedPassword(GenerateHash(plainPassword, salt), salt);
        }

        /// <summary>
        /// Checks if this HashedPassword matches the specified password.
        /// </summary>
        /// <param name="plainPassword">String that contains the password to check this HashedPassword against.</param>
        /// <returns>True, if the specified password matches this HashedPassword; otherwise false.</returns>
        public bool EqualsPlainPassword(string plainPassword)
        {
            var hash = GenerateHash(plainPassword, this.Salt);
            return hash.SequenceEqual(this.Hash);
        }

        /// <summary>
        /// Generates a random salt for a new HashedPassword.
        /// </summary>
        /// <returns>An array of <see cref="byte"/> with the randomly generated salt.</returns>
        private static byte[] GenerateSalt()
        {
            using (var generator = RandomNumberGenerator.Create())
            {
                var salt = new byte[16];
                generator.GetBytes(salt);
                return salt;
            }
        }

        /// <summary>
        /// Creates a hash for the specified password with the specified salt.
        /// </summary>
        /// <param name="plainPassword">String that contains the password to create the hash from.</param>
        /// <param name="salt">Salt that should be used for creating the hash.</param>
        /// <returns>An array of <see cref="byte"/> with the generated hash.</returns>
        private static byte[] GenerateHash(string plainPassword, byte[] salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(plainPassword, salt, 10000))
            {
                return deriveBytes.GetBytes(20);
            }
        }
    }
}