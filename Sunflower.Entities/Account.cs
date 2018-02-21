namespace Sunflower.Entities
{
    /// <summary>
    /// Represents an account of a registered user.
    /// </summary>
    public sealed class Account : EntityBase
    {
        /// <summary>
        /// Email address this account was registered with.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Hash generated from the password of this account.
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// Salt used to generate the hash from the password of this account.
        /// </summary>
        public byte[] PasswordSalt { get; set; }
    }
}