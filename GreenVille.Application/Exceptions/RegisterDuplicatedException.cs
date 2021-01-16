using System;

namespace GreenVille.Application.Exceptions
{
    public class RegisterDuplicatedException : Exception
    {
        public RegisterDuplicatedException() : base()
        { }

        public RegisterDuplicatedException(string message) : base(message)
        { }

        public RegisterDuplicatedException(string message, Exception exception) : base(message, exception)
        { }
    }
}
