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
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _clientesService;

        public ClientesController(IClientesService clientesService)
        {
            _clientesService = clientesService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
        {
            try
            {
                var clienteList = await _clientesService.GetClientes();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, clienteList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            try
            {
                var cliente = await _clientesService.GetCliente(id);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, cliente));
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
        public async Task<IActionResult> PutCliente(int id, ClienteDTO cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            try
            {
                cliente = await _clientesService.UpdateCliente(cliente);

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
        public async Task<ActionResult<ClienteDTO>> PostCliente(ClienteDTO cliente)
        {
            try
            {
                cliente = await _clientesService.AddCliente(cliente);

                var version = Request.HttpContext.GetRequestedApiVersion().ToString();
                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id, version }, cliente);
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
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var result = await _clientesService.DeleteCliente(id);

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


        [HttpGet("ObterPorCPF", Name = "ObterPorCPF")]
        [Produces("application/json")]
        public async Task<ActionResult<ClienteDTO>> GetClientePorCPF(string cpf)
        {
            try
            {
                var cliente = await _clientesService.GetClientePorCPF(cpf);

                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, cliente));
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
