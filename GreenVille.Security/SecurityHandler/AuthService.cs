using GreenVille.Domain;
using GreenVille.Domain.DTO;
using GreenVille.Security.SecurityHandler.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GreenVille.Security.SecurityHandler
{
    public class AuthService: IAuthService
    {

        private readonly Appsettings _configuration;

        public AuthService(Appsettings configuration)
        {
            _configuration = configuration;
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_configuration.JwtSettings.securityKey) ;
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public List<Claim> GetClaims(AuthUserDTO user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            return claims;
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration.JwtSettings.validIssuer,
                audience: _configuration.JwtSettings.validAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.JwtSettings.expiryInMinutes)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}