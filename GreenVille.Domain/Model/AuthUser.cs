using System;
using System.Collections.Generic;
using System.Text;

namespace GreenVille.Domain.Model
{
    public class AuthUser
    {
        public string Nome { get; set; }

        public string CPF { get; set; }

        public string Password { get; set; }

        public bool NovoAcesso { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }
    }
}
