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
    public class FuncionariosService : IFuncionariosService
    {
        private const string ERR_MSG_FUNCIONARIO_NOT_FOUND = "Funcionário de Id {0} não encontrado.";
        private const string ERR_MSG_ESTACIONAMENTO_PARA_FUNCIONARIO_NOT_FOUND = "Estacionamento selecionado para o Funcionário não existe.";
        private const string ERR_MSG_CARGO_NOT_FOUND = "Cargo de Funcionário não existente.";
        private const string ERR_MSG_FUNCIONARIO_ALREADY_EXISTS = "Funcionário de Id {0} já existe na base de dados.";

        private readonly IFuncionarioRepository _baseRepo;
        private readonly IEstacionamentoRepository _estacionamentoRepository;

        public FuncionariosService(IFuncionarioRepository funcionarioRepository, IEstacionamentoRepository estacionamentoRepository)
        {
            _baseRepo = funcionarioRepository;
            _estacionamentoRepository = estacionamentoRepository;
        }


        public async Task<FuncionarioDTO> AddFuncionario(FuncionarioDTO funcionario)
        {
            try
            {
                //checks if entry already exists
                var funcionarioRegisterExists = await FuncionarioExists(funcionario.Id);
                if (funcionarioRegisterExists)
                {
                    throw new RegisterDuplicatedException(string.Format(ERR_MSG_FUNCIONARIO_ALREADY_EXISTS, funcionario.Id));
                }

                //retrieves cargo
                var cargo = await _baseRepo.FindCargo(funcionario.CargoId);
                if (null == cargo) { throw new RegisterNotFoundException(ERR_MSG_CARGO_NOT_FOUND); }

                //retrives Estacionamento
                var estacionamento = await _estacionamentoRepository.GetAsync(funcionario.EstacionamentoId);
                if (null == estacionamento) { throw new RegisterNotFoundException(ERR_MSG_ESTACIONAMENTO_PARA_FUNCIONARIO_NOT_FOUND); }


                var funcionarioRegister = new Funcionario
                {
                    Nome = funcionario.Nome,
                    CPF = funcionario.CPF,
                    Cargo = cargo,
                    Estacionamento = estacionamento
                };

                await _baseRepo.AddAsync(funcionarioRegister);


                //salve all changes
                await _baseRepo.SaveAllAsync();

                funcionario.Id = funcionarioRegister.Id;
                return funcionario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<FuncionarioDTO> UpdateFuncionario(FuncionarioDTO funcionario)
        {
            try
            {
                //retrieves cargo
                var cargo = await _baseRepo.FindCargo(funcionario.CargoId);
                if (null == cargo) { throw new RegisterNotFoundException(ERR_MSG_CARGO_NOT_FOUND); }

                //retrives Estacionamento
                var estacionamento = await _estacionamentoRepository.GetAsync(funcionario.EstacionamentoId);
                if (null == estacionamento) { throw new RegisterNotFoundException(ERR_MSG_ESTACIONAMENTO_PARA_FUNCIONARIO_NOT_FOUND); }


                //checks if funcionario already exists
                var funcionarioRegister = await _baseRepo.GetAsync(funcionario.Id);
                if (null == funcionarioRegister)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_FUNCIONARIO_NOT_FOUND, funcionario.Id));
                }
                else
                {
                    funcionarioRegister.Nome = funcionario.Nome;
                    funcionarioRegister.CPF = funcionario.CPF;
                    funcionarioRegister.Cargo = cargo;

                    _baseRepo.Update(funcionarioRegister);
                }

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return funcionario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> DeleteFuncionario(int id)
        {
            try
            {
                //checks if entry already exists
                var funcionarioRegisterExists = await FuncionarioExists(id);
                if (!funcionarioRegisterExists)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_FUNCIONARIO_NOT_FOUND, id));
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


        public async Task<FuncionarioDTO> GetFuncionario(int id)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.Estacionamento)
                                        .Include(f => f.Cargo);

                var funcionario = await search.SingleOrDefaultAsync(x => x.Id == id);

                if (null == funcionario)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_FUNCIONARIO_NOT_FOUND, id));
                }

                var funcionarioDTO = new FuncionarioDTO
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    CPF = funcionario.CPF,
                    CargoId = funcionario.CargoId,
                    CargoDescricao = funcionario.Cargo.Descricao,
                    EstacionamentoId = funcionario.EstacionamentoId,
                    EstacionamentoUnidade = funcionario.Estacionamento.NomeUnidade
                };

                return funcionarioDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<FuncionarioDTO>> GetFuncionarios(int? estacionamentoId = null)
        {
            try
            {
                var funcionariosQuery = _baseRepo.QueryAll()
                                                    .Include(f => f.Estacionamento)
                                                    .Include(f => f.Cargo)
                                                    .AsQueryable();

                if (estacionamentoId.HasValue)
                {
                    funcionariosQuery = funcionariosQuery.Where(f => f.EstacionamentoId == estacionamentoId.Value);
                }

                var funcionarios = await funcionariosQuery.ToListAsync();

                var funcionariosDTO = new List<FuncionarioDTO>();
                foreach (var funcionario in funcionarios)
                {
                    var funcionarioDTO = new FuncionarioDTO
                    {
                        Id = funcionario.Id,
                        Nome = funcionario.Nome,
                        CPF = funcionario.CPF,
                        CargoId = funcionario.CargoId,
                        CargoDescricao = funcionario.Cargo.Descricao,
                        EstacionamentoId = funcionario.EstacionamentoId,
                        EstacionamentoUnidade = funcionario.Estacionamento.NomeUnidade
                    };
                    funcionariosDTO.Add(funcionarioDTO);
                }

                return funcionariosDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<CargoDTO>> GetCargos()
        {
            try
            {
                var cargos = await _baseRepo.GetCargosAsync();

                var cargosDTO = new List<CargoDTO>();
                foreach (var cargo in cargos)
                {
                    var cargoDTO = new CargoDTO
                    {
                        Id = cargo.Id,
                        Descricao = cargo.Descricao
                    };
                    cargosDTO.Add(cargoDTO);
                }

                return cargosDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> FuncionarioExists(int id)
        {
            return await _baseRepo.Exists(id);
        }

    }
}
