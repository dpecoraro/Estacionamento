using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IServices;
using GreenVille.Security.SecurityHandler.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;

namespace GreenVille.API.Controllers.v1
{
    [ApiVersion("1")]
    [Route("/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;
        private readonly IAuthService _authenticationService;

        public AuthenticationController(IUsuariosService usuariosService, IAuthService authenticationService)
        {
            _usuariosService = usuariosService;
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthUserDTO authUser)
        {
            try
            {
                var usuario = await _usuariosService.GetUsuarioByMail(authUser.Email);

                if (null == usuario)
                {
                    return Unauthorized(new AuthResponseDTO { ErrorMessage = "Usuário não encontrado!" });
                }
                if (usuario.Senha != authUser.Password)
                {
                    return Unauthorized(new AuthResponseDTO { ErrorMessage = "Senha incorreta!" });
                }

                var signingCredentials = _authenticationService.GetSigningCredentials();
                var claims = _authenticationService.GetClaims(authUser);
                var tokenOptions = _authenticationService.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new AuthResponseDTO { IsAuthSuccessful = true, Token = token });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
