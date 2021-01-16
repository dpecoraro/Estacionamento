using GreenVille.Application.Exceptions;
using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Interfaces.IServices;
using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenVille.Application.Services
{
    public class VagasService : IVagasService
    {

        private const string ERR_MSG_VAGA_NOT_FOUND = "Vaga de Id {0} não encontrado.";
        private const string ERR_MSG_VAGA_ALREADY_EXISTS = "Vaga de Id {0} já existe na base de dados.";
        private const string ERR_MSG_ESTACIONAMENTO_PARA_VAGA_NOT_FOUND = "Estacionamento selecionado para a Vaga não existe.";

        private readonly IVagaRepository _baseRepo;
        private readonly IEstacionamentoRepository _estacionamentoRepository;


        public VagasService(IVagaRepository vagaRepository, IEstacionamentoRepository estacionamentoRepository)
        {
            _baseRepo = vagaRepository;
            _estacionamentoRepository = estacionamentoRepository;
        }

        public async Task<VagaDTO> AddVaga(VagaDTO vagaDto)
        {
            try
            {
                //checks if entry already exists
                var vagaRegisterExists = await VagaExists(vagaDto.Id);

                if (vagaRegisterExists)
                {
                    throw new RegisterDuplicatedException(string.Format(ERR_MSG_VAGA_ALREADY_EXISTS, vagaDto.Id));
                }

                //retrives Estacionamento
                var estacionamento = await _estacionamentoRepository.GetAsync(vagaDto.EstacionamentoId);
                if (null == estacionamento) { throw new RegisterNotFoundException(ERR_MSG_ESTACIONAMENTO_PARA_VAGA_NOT_FOUND); }


                Vaga vagaRegister = new Vaga()
                {
                    Interditada = vagaDto.Interditada,
                    Nome = vagaDto.Nome,
                    Ocupada = vagaDto.Ocupada,
                    EstacionamentoId = vagaDto.EstacionamentoId
                };

                await _baseRepo.AddAsync(vagaRegister);

                //salve all changes
                await _baseRepo.SaveAllAsync();

                vagaDto.Id = vagaRegister.Id;
                return vagaDto;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteVaga(int id)
        {
            try
            {
                //checks if entry already exists
                var vagaRegisterExists = await VagaExists(id);
                if (!vagaRegisterExists)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_VAGA_NOT_FOUND, id));
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

        public async Task<VagaDTO> GetVaga(int id)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.Estacionamento);

                var vaga = await search.SingleOrDefaultAsync(x => x.Id == id);

                if (null == vaga)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_VAGA_NOT_FOUND, id));
                }

                var vagaDTO = new VagaDTO
                {
                    Id = vaga.Id,
                    Interditada = vaga.Interditada,
                    Nome = vaga.Nome,
                    Ocupada = vaga.Ocupada,
                    EstacionamentoId = vaga.EstacionamentoId,
                    EstacionamentoUnidade = vaga.Estacionamento.NomeUnidade
                };

                return vagaDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<VagaDTO>> GetVagas()
        {
            try
            {
                var vagas = await _baseRepo.QueryAll()
                                                    .Include(f => f.Estacionamento)
                                                    .ToListAsync();

                var vagasDTO = new List<VagaDTO>();
                foreach (var vaga in vagas)
                {
                    var vagaDTO = new VagaDTO()
                    {
                        Id = vaga.Id,
                        Interditada = vaga.Interditada,
                        Nome = vaga.Nome,
                        Ocupada = vaga.Ocupada,
                        EstacionamentoId = vaga.EstacionamentoId,
                        EstacionamentoUnidade = vaga.Estacionamento.NomeUnidade
                    };
                    vagasDTO.Add(vagaDTO);
                }
                return vagasDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<VagaDTO>> GetVagasByEstacionamento(int idEstacionamento)
        {
            try
            {
                var vagas = await _baseRepo.QueryAll()
                                                    .Include(f => f.Estacionamento)
                                                    .Where(f => f.EstacionamentoId == idEstacionamento)
                                                    .ToListAsync();

                var vagasDTO = new List<VagaDTO>();
                foreach (var vaga in vagas)
                {
                    var vagaDTO = new VagaDTO()
                    {
                        Id = vaga.Id,
                        Interditada = vaga.Interditada,
                        Nome = vaga.Nome,
                        Ocupada = vaga.Ocupada,
                        EstacionamentoId = vaga.EstacionamentoId,
                        EstacionamentoUnidade = vaga.Estacionamento.NomeUnidade
                    };
                    vagasDTO.Add(vagaDTO);
                }
                return vagasDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<VagaDTO> UpdateVaga(VagaDTO vaga)
        {
            try
            {
                //retrives Estacionamento
                var estacionamento = await _estacionamentoRepository.GetAsync(vaga.EstacionamentoId);
                if (null == estacionamento) { throw new RegisterNotFoundException(ERR_MSG_ESTACIONAMENTO_PARA_VAGA_NOT_FOUND); }


                var vagaExists = await _baseRepo.Exists(vaga.Id);

                if (vagaExists)
                {
                    var vagaRegister = await _baseRepo.GetAsync(vaga.Id);

                    vagaRegister.Interditada = vaga.Interditada;
                    vagaRegister.Nome = vaga.Nome;
                    vagaRegister.Ocupada = vaga.Ocupada;
                    vagaRegister.EstacionamentoId = vaga.EstacionamentoId;

                    _baseRepo.Update(vagaRegister);
                }
                else
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_VAGA_NOT_FOUND, vaga.Id));
                }

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return vaga;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> VagaExists(int id)
        {
            return await _baseRepo.Exists(id);
        }
    }
}
