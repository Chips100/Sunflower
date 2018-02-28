using System.Collections.Generic;

namespace Sunflower.Entities
{
    /// <summary>
    /// Represents an account of a registered user.
    /// </summary>
    public class Account : EntityBase
    {
        /// <summary>
        /// Creates an account.
        /// </summary>
        public Account()
        {
            ContextFreeTransactions = new HashSet<ContextFreeTransaction>();
        }

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

        /// <summary>
        /// ContextFreeTransactions that belong to the account.
        /// </summary>
        public virtual ICollection<ContextFreeTransaction> ContextFreeTransactions { get; set; }
    }
}