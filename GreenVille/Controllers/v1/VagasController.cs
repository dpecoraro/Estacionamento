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
    public class VagasController : ControllerBase
    {
        private readonly IVagasService _vagasService;

        public VagasController(IVagasService vagasService)
        {
            _vagasService = vagasService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<VagaDTO>>> GetVagas()
        {
            try
            {
                var vagaList = await _vagasService.GetVagas();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, vagaList));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("GetVagasByEstacionamento/{idEstacionamento}", Name = "GetVagasByEstacionamento")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<VagaDTO>>> GetVagasByEstacionamento(int idEstacionamento)
        {
            try
            {
                var vagaList = await _vagasService.GetVagasByEstacionamento(idEstacionamento);
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, vagaList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<VagaDTO>> GetVaga(int id)
        {
            try
            {
                var vaga = await _vagasService.GetVaga(id);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, vaga));
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
        public async Task<IActionResult> PutVaga(int id, VagaDTO vaga)
        {
            if (id != vaga.Id)
            {
                return BadRequest();
            }

            try
            {
                vaga = await _vagasService.UpdateVaga(vaga);

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
        public async Task<ActionResult<VagaDTO>> PostVaga(VagaDTO vaga)
        {
            try
            {
                vaga = await _vagasService.AddVaga(vaga);

                var version = Request.HttpContext.GetRequestedApiVersion().ToString();
                return CreatedAtAction(nameof(GetVaga), new { id = vaga.Id, version }, vaga);
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
        public async Task<IActionResult> DeleteVaga(int id)
        {
            try
            {
                var result = await _vagasService.DeleteVaga(id);

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
