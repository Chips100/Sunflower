using Sunflower.Business.Contracts;

namespace Sunflower.Business.Exceptions
{
    /// <summary>
    /// Thrown when trying to create a new account with an email address
    /// that is already used.
    /// </summary>
    public class EmailAlreadyRegisteredException : BusinessException
    {
        public EmailAlreadyRegisteredException(string email)
            : base($"An account with email address { email } already exists.", null)
        { }
    }
}
