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
    public class CargosController : ControllerBase
    {
        private readonly IFuncionariosService _funcionariosService;

        public CargosController(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
        }

        [HttpGet()]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<CargoDTO>>> GetCargos()
        {
            try
            {
                var cargosList = await _funcionariosService.GetCargos();
                return await Task.FromResult(StatusCode((int)HttpStatusCode.OK, cargosList));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode((int)HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
