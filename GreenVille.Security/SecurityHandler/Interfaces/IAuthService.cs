using GreenVille.Domain.DTO;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GreenVille.Security.SecurityHandler.Interfaces
{
    public interface IAuthService
    {
        SigningCredentials GetSigningCredentials();

        List<Claim> GetClaims(AuthUserDTO user);

        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
    }
}