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
    public class RelatoriosController : ControllerBase
    {

        private readonly IVeiculosService _veiculosService;
        private readonly IAlocacoesService _alocacoesService;


        public RelatoriosController(IVeiculosService veiculosService, IAlocacoesService alocacoesService)
        {
            _veiculosService = veiculosService;
            _alocacoesService = alocacoesService;
        }


        [HttpGet("VeiculosMensalistas", Name = "VeiculosMensalistas")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<RptVeiculoMensalistaDTO>>> GetVeiculosMensalistas()
        {
            try
            {
                var clientesMensalistas = await _veiculosService.GetVeiculosMensalistas();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, clientesMensalistas));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [HttpGet("MovimentacaoPorVeiculo/{idVeiculo}", Name = "MovimentacaoPorVeiculo")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<RptEntradaSaidaVeiculoDTO>>> GetMovimentacaoPorVeiculo(int idVeiculo, DateTime periodoInicio, DateTime periodoFim)
        {
            try
            {
                var movementList = await _alocacoesService.ListParkingMovementByVehicle(idVeiculo, periodoInicio, periodoFim);
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, movementList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [HttpGet("UsoPorHorario", Name = "UsoPorHorario")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<RptUsoHoraEstacionamento>>> GetUsoPorHorario(DateTime periodoInicio, DateTime periodoFim)
        {
            try
            {
                var movementList = await _alocacoesService.ListParkingUseByHour(periodoInicio, periodoFim);
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, movementList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
