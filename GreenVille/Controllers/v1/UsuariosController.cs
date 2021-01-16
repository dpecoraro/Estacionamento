using GreenVille.Application.Exceptions;
using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GreenVille.API.Controllers.v1
{
    [ApiVersion("1")]
    [Route("/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet]        
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            try
            {
                var usuariosList = await _usuariosService.GetUsuarios();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, usuariosList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _usuariosService.GetUsuario(id);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, usuario));
            }
            catch (RegisterNotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDTO usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            try
            {
                usuario = await _usuariosService.UpdateUsuario(usuario);

                return NoContent();
            }
            catch (RegisterNotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioDTO usuario)
        {
            try
            {
                usuario = await _usuariosService.AddUsuario(usuario);

                var version = Request.HttpContext.GetRequestedApiVersion().ToString();
                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id, version }, usuario);
            }
            catch (RegisterDuplicatedException duplicatedException)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.Forbidden, duplicatedException.Message));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var result = await _usuariosService.DeleteUsuario(id);

                return NoContent();
            }
            catch (RegisterNotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
