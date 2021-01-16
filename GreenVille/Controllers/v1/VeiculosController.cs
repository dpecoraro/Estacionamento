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
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculosService _veiculosService;

        public VeiculosController(IVeiculosService veiculosService)
        {
            _veiculosService = veiculosService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<VeiculoDTO>>> GetVeiculos()
        {
            try
            {
                var veiculoList = await _veiculosService.GetVeiculos();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, veiculoList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<VeiculoDTO>> GetVeiculo(int id)
        {
            try
            {
                var veiculo = await _veiculosService.GetVeiculo(id);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, veiculo));
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
        public async Task<IActionResult> PutVeiculo(int id, VeiculoDTO veiculo)
        {
            if (id != veiculo.Id)
            {
                return BadRequest();
            }

            try
            {
                veiculo = await _veiculosService.UpdateVeiculo(veiculo);

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
        public async Task<ActionResult<VeiculoDTO>> PostVeiculo(VeiculoDTO veiculo)
        {
            try
            {
                veiculo = await _veiculosService.AddVeiculo(veiculo);

                var version = Request.HttpContext.GetRequestedApiVersion().ToString();
                return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.Id, version }, veiculo);
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
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            try
            {
                var result = await _veiculosService.DeleteVeiculo(id);

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


        [HttpGet("ObterPorPlaca", Name = "ObterPorPlaca")]
        [Produces("application/json")]
        public async Task<ActionResult<VeiculoDTO>> GetVeiculoPorPlaca(string placa)
        {
            try
            {
                var veiculo = await _veiculosService.GetVeiculoByPlateNumber(placa);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, veiculo));
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


        [HttpGet("ListarVeiculosNaoEstacionados", Name = "ListarVeiculosNaoEstacionados")]
        [Produces("application/json")]
        public async Task<ActionResult<VeiculoDTO>> GetVeiculosNaoEstacionados()
        {
            try
            {
                var veiculoList = await _veiculosService.GetVeiculosNotParked();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, veiculoList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
