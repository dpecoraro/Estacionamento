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
    public class EstacionamentosController : ControllerBase
    {
        private readonly IEstacionamentosService _estacionamentosService;

        public EstacionamentosController(IEstacionamentosService estacionamentosService)
        {
            _estacionamentosService = estacionamentosService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<EstacionamentoDTO>>> GetEstacionamentos()
        {
            try
            {
                var estacionamentoList = await _estacionamentosService.GetEstacionamentos();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, estacionamentoList));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<EstacionamentoDTO>> GetEstacionamento(int id)
        {
            try
            {
                var estacionamento = await _estacionamentosService.GetEstacionamento(id);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, estacionamento));
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
        public async Task<IActionResult> PutEstacionamento(int id, EstacionamentoDTO estacionamento)
        {
            if (id != estacionamento.Id)
            {
                return BadRequest();
            }

            try
            {
                estacionamento = await _estacionamentosService.UpdateEstacionamento(estacionamento);

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
        public async Task<ActionResult<EstacionamentoDTO>> PostEstacionamento(EstacionamentoDTO estacionamento)
        {
            try
            {
                estacionamento = await _estacionamentosService.AddEstacionamento(estacionamento);

                var version = Request.HttpContext.GetRequestedApiVersion().ToString();
                return CreatedAtAction(nameof(GetEstacionamento), new { id = estacionamento.Id, version }, estacionamento);
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
        public async Task<IActionResult> DeleteEstacionamento(int id)
        {
            try
            {
                var result = await _estacionamentosService.DeleteEstacionamento(id);

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
