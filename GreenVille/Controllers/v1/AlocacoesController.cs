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
    public class AlocacoesController : ControllerBase
    {
        private readonly IAlocacoesService _alocacoesService;

        public AlocacoesController(IAlocacoesService alocacoesService)
        {
            _alocacoesService = alocacoesService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<AlocacaoDTO>>> GetAlocacoes()
        {
            try
            {
                var alocacoesList = await _alocacoesService.GetAlocacoes();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, alocacoesList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<AlocacaoDTO>> GetAlocacao(int id)
        {
            try
            {
                var alocacao = await _alocacoesService.GetAlocacao(id);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, alocacao));
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
        public async Task<IActionResult> PutAlocacao(int id, AlocacaoDTO alocacao)
        {
            if (id != alocacao.Id)
            {
                return BadRequest();
            }

            try
            {
                alocacao = await _alocacoesService.UpdateAlocacao(alocacao);

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
        public async Task<ActionResult<AlocacaoDTO>> PostAlocacao(AlocacaoDTO alocacao)
        {
            try
            {
                alocacao = await _alocacoesService.AddAlocacao(alocacao);

                var version = Request.HttpContext.GetRequestedApiVersion().ToString();
                return CreatedAtAction(nameof(GetAlocacao), new { id = alocacao.Id, version }, alocacao.Id);

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
        public async Task<IActionResult> DeleteAlocacao(int id)
        {
            try
            {
                var result = await _alocacoesService.DeleteAlocacao(id);

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


        [HttpGet("ListarPorEstacionamento/{idEstacionamento}", Name = "ListarPorEstacionamento")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<RptEntradaSaidaVeiculoDTO>>> GetAlocacoesPorEstacionamento(int idEstacionamento)
        {
            try
            {
                var movementList = await _alocacoesService.GetCurrentAlocacoesDtoByEstacionamento(idEstacionamento);
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, movementList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("ListarPorPlaca", Name = "ListarPorPlaca")]
        [Produces("application/json")]
        public async Task<ActionResult<AlocacaoDTO>> GetAlocacaoPorPlaca(string plateNumber)
        {
            try
            {
                var alocacao = await _alocacoesService.GetAlocacaoDtoByVehiclePlate(plateNumber);
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, alocacao));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
