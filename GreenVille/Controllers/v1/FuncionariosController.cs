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
    public class FuncionariosController : ControllerBase
    {
        private readonly IFuncionariosService _funcionariosService;

        public FuncionariosController(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<FuncionarioDTO>>> GetFuncionarios(int? idEstacionamento = null)
        {
            try
            {
                var funcionariosList = await _funcionariosService.GetFuncionarios(idEstacionamento);
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, funcionariosList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<FuncionarioDTO>> GetFuncionario(int id)
        {
            try
            {
                var funcionario = await _funcionariosService.GetFuncionario(id);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, funcionario));
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
        public async Task<IActionResult> PutFuncionario(int id, FuncionarioDTO funcionario)
        {
            if (id != funcionario.Id)
            {
                return BadRequest();
            }

            try
            {
                funcionario = await _funcionariosService.UpdateFuncionario(funcionario);

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
        public async Task<ActionResult<FuncionarioDTO>> PostFuncionario(FuncionarioDTO funcionario)
        {
            try
            {
                funcionario = await _funcionariosService.AddFuncionario(funcionario);

                var version = Request.HttpContext.GetRequestedApiVersion().ToString();
                return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.Id, version }, funcionario);

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
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            try
            {
                var result = await _funcionariosService.DeleteFuncionario(id);

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
