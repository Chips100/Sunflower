using Sunflower.Business.Contracts;

namespace Sunflower.Business.Exceptions
{
    /// <summary>
    /// Thrown when trying to use an account with an 
    /// email address that is not registered.
    /// </summary>
    public class EmailNotRegisteredException : BusinessException
    {
        public EmailNotRegisteredException(string email)
            : base($"An account with email address { email } does not exist.", null)
        { }
    }
}
