using GreenVille.Application.Exceptions;
using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Interfaces.IServices;
using GreenVille.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenVille.Application.Services
{
    public class EstacionamentosService : IEstacionamentosService
    {
        private const string ERR_MSG_ESTACIONAMENTO_NOT_FOUND = "Estacionamento de Id {0} não encontrado.";
        private const string ERR_MSG_ESTACIONAMENTO_ALREADY_EXISTS = "Estacionamento de Id {0} já existe na base de dados.";

        private readonly IEstacionamentoRepository _baseRepo;

        public EstacionamentosService(IEstacionamentoRepository estacionamentoRepository)
        {
            _baseRepo = estacionamentoRepository;
        }


        public async Task<EstacionamentoDTO> AddEstacionamento(EstacionamentoDTO estacionamento)
        {
            try
            {
                //checks if entry already exists
                var estacionamentoRegisterExists = await EstacionamentoExists(estacionamento.Id);
                if (estacionamentoRegisterExists)
                {
                    throw new RegisterDuplicatedException(string.Format(ERR_MSG_ESTACIONAMENTO_ALREADY_EXISTS, estacionamento.Id));
                }


                var estacionamentoRegister = new Estacionamento
                {
                    NomeUnidade = estacionamento.NomeUnidade,
                    ValorHora = estacionamento.ValorHora,
                    GeracaoCreditosCarbonoHora = estacionamento.GeracaoCreditosCarbonoHora
                };

                await _baseRepo.AddAsync(estacionamentoRegister);


                //salve all changes
                await _baseRepo.SaveAllAsync();

                estacionamento.Id = estacionamentoRegister.Id;
                return estacionamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EstacionamentoDTO> UpdateEstacionamento(EstacionamentoDTO estacionamento)
        {
            try
            {
                //checks if estacionamento already exists
                var estacionamentoRegister = await _baseRepo.GetAsync(estacionamento.Id);
                if (null == estacionamentoRegister)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_ESTACIONAMENTO_NOT_FOUND, estacionamento.Id));
                }
                else
                {
                    estacionamentoRegister.NomeUnidade = estacionamento.NomeUnidade;
                    estacionamentoRegister.ValorHora = estacionamento.ValorHora;
                    estacionamentoRegister.GeracaoCreditosCarbonoHora = estacionamento.GeracaoCreditosCarbonoHora;

                    _baseRepo.Update(estacionamentoRegister);
                }

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return estacionamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteEstacionamento(int id)
        {
            try
            {
                //checks if entry already exists
                var estacionamentoRegisterExists = await EstacionamentoExists(id);
                if (!estacionamentoRegisterExists)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_ESTACIONAMENTO_NOT_FOUND, id));
                }

                //deletes the entry
                _baseRepo.Delete(x => x.Id == id);

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EstacionamentoDTO> GetEstacionamento(int id)
        {
            try
            {
                var estacionamento = await _baseRepo.GetAsync(id);
                if (null == estacionamento)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_ESTACIONAMENTO_NOT_FOUND, id));
                }

                var estacionamentoDTO = new EstacionamentoDTO
                {
                    Id = estacionamento.Id,
                    NomeUnidade = estacionamento.NomeUnidade,
                    ValorHora = estacionamento.ValorHora,
                    GeracaoCreditosCarbonoHora = estacionamento.GeracaoCreditosCarbonoHora
            };

                return estacionamentoDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EstacionamentoDTO>> GetEstacionamentos()
        {
            try
            {
                var estacionamentos = await _baseRepo.ListAllAsync();

                var estacionamentosDTO = new List<EstacionamentoDTO>();
                foreach (var estacionamento in estacionamentos)
                {
                    var estacionamentoDTO = new EstacionamentoDTO
                    {
                        Id = estacionamento.Id,
                        NomeUnidade = estacionamento.NomeUnidade,
                        ValorHora = estacionamento.ValorHora,
                        GeracaoCreditosCarbonoHora = estacionamento.GeracaoCreditosCarbonoHora
                    };
                    estacionamentosDTO.Add(estacionamentoDTO);
                }

                return estacionamentosDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task<bool> EstacionamentoExists(int id)
        {
            return await _baseRepo.Exists(id);
        }
    }
}