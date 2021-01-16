using System;

namespace GreenVille.Application.Exceptions
{
    public class RegisterNotFoundException: Exception
    {
        public RegisterNotFoundException() : base()
        { }

        public RegisterNotFoundException(string message) : base(message)
        { }

        public RegisterNotFoundException(string message, Exception exception) : base(message, exception)
        { }
    }
}
