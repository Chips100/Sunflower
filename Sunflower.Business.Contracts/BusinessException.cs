using System;

namespace Sunflower.Business.Contracts
{
    /// <summary>
    /// Serves as the base class for exceptions thrown
    /// in the Business Layer.
    /// </summary>
    public abstract class BusinessException : ApplicationException
    {
        protected BusinessException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}