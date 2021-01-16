using GreenVille.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Domain.Interfaces.IServices
{
    public interface IAlocacoesService
    {

        Task<IEnumerable<AlocacaoDTO>> GetAlocacoes();

        Task<AlocacaoDTO> GetAlocacao(int id);

        Task<AlocacaoDTO> AddAlocacao(AlocacaoDTO alocacao);

        Task<AlocacaoDTO> UpdateAlocacao(AlocacaoDTO alocacao);

        Task<bool> DeleteAlocacao(int id);


        Task<IEnumerable<VeiculoAlocadoDTO>> GetCurrentAlocacoesDtoByEstacionamento(int idEstacionamento);

        Task<AlocacaoDTO> GetAlocacaoDtoByVehiclePlate(string plateNumber);

        Task<IEnumerable<RptEntradaSaidaVeiculoDTO>> ListParkingMovementByVehicle(int idVeiculo, DateTime startPeriod, DateTime endPeriod);

        Task<IEnumerable<RptUsoHoraEstacionamento>> ListParkingUseByHour(DateTime startPeriod, DateTime endPeriod);
    }
}
