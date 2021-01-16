using GreenVille.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Portal.Services.Interfaces
{
    public interface IRelatoriosApiClient : IBaseApiClient<object>
    {

        Task<List<RptVeiculoMensalistaDTO>> GetAllVeiculosMensalistas();

        Task<List<RptEntradaSaidaVeiculoDTO>> GetAtividadeVeiculosPorPeriodo(int idVeiculo, DateTime periodoInicio, DateTime periodoFim);

        Task<List<RptUsoHoraEstacionamento>> GetUsoPorHorario(DateTime periodoInicio, DateTime periodoFim);

    }
}
