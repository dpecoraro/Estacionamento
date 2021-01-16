using GreenVille.Application.Exceptions;
using GreenVille.Domain.DTO;
using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Interfaces.IServices;
using GreenVille.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GreenVille.Application.Services
{
    public class VeiculosService : IVeiculosService
    {
        private const string ERR_MSG_VEICULO_NOT_FOUND = "Veiculo de Id {0} não encontrado.";
        private const string ERR_MSG_VEICULO_BY_PLATE_NOT_FOUND = "Veiculo de Placa '{0}' não encontrado.";
        private const string ERR_MSG_VEICULO_ALREADY_EXISTS = "Veiculo de Id {0} já existe na base de dados.";

        private readonly IVeiculoRepository _baseRepo;
        private readonly IClienteRepository _clienteRepository;
        private readonly IAlocacaoRepository _alocacaoRepository;


        public VeiculosService(IVeiculoRepository veiculoRepository, IClienteRepository clienteRepository, IAlocacaoRepository alocacaoRepository)
        {
            _baseRepo = veiculoRepository;
            _clienteRepository = clienteRepository;
            _alocacaoRepository = alocacaoRepository;
        }


        public async Task<VeiculoDTO> AddVeiculo(VeiculoDTO veiculo)
        {
            try
            {
                //checks if entry already exists
                var veiculoRegisterExists = await VeiculoExists(veiculo.Id);
                if (veiculoRegisterExists)
                {
                    throw new RegisterDuplicatedException(string.Format(ERR_MSG_VEICULO_ALREADY_EXISTS, veiculo.Id));
                }

                var veiculoRegister = new Veiculo()
                {
                    Placa = veiculo.Placa,
                    Modelo = veiculo.Modelo,
                    Ano = veiculo.Ano,
                    Mensalista = veiculo.Mensalista,
                    Cor = veiculo.Cor
                };
                await _baseRepo.AddAsync(veiculoRegister);


                if ((null != veiculo.VeiculosClientes) && (veiculo.VeiculosClientes.Count > 0))
                {
                    veiculoRegister.VeiculosClientes = new List<VeiculoCliente>();
                    foreach (var vc in veiculo.VeiculosClientes)
                    {
                        var newVC = new VeiculoCliente()
                        {
                            VeiculoId = veiculo.Id,
                            ClienteId = vc.ClienteId
                        };

                        veiculoRegister.VeiculosClientes.Add(newVC);
                    }

                    _baseRepo.Update(veiculoRegister);
                }


                //salve all changes
                await _baseRepo.SaveAllAsync();

                veiculo.Id = veiculoRegister.Id;
                return veiculo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<VeiculoDTO> UpdateVeiculo(VeiculoDTO veiculo)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.VeiculosClientes);

                var veiculoRegister = await search.SingleOrDefaultAsync(x => x.Id == veiculo.Id);


                //checks if veiculo already exists
                if (null == veiculoRegister)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_VEICULO_NOT_FOUND, veiculo.Id));
                }
                else
                {
                    veiculoRegister.Placa = veiculo.Placa;
                    veiculoRegister.Modelo = veiculo.Modelo;
                    veiculoRegister.Ano = veiculo.Ano;
                    veiculoRegister.Mensalista = veiculo.Mensalista;
                    veiculoRegister.Cor = veiculo.Cor;

                    veiculoRegister.VeiculosClientes.Clear();
                    if ((null != veiculo.VeiculosClientes) && (veiculo.VeiculosClientes.Count > 0))
                    {
                        veiculoRegister.VeiculosClientes = new List<VeiculoCliente>();
                        foreach (var vc in veiculo.VeiculosClientes)
                        {
                            veiculoRegister.VeiculosClientes.Add(
                                new VeiculoCliente()
                                {
                                    VeiculoId = veiculo.Id,
                                    ClienteId = vc.ClienteId
                                }
                            );
                        }
                    }

                    _baseRepo.Update(veiculoRegister);
                }

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return veiculo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> DeleteVeiculo(int id)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.VeiculosClientes);

                var veiculo = await search.SingleOrDefaultAsync(x => x.Id == id);

                //checks if entry already exists
                if (null == veiculo)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_VEICULO_NOT_FOUND, id));
                }

                //delete all VeiculoClientes related
                veiculo.VeiculosClientes.Clear();
                _baseRepo.Update(veiculo);

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


        public async Task<VeiculoDTO> GetVeiculo(int id)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.VeiculosClientes);

                var veiculo = await search.SingleOrDefaultAsync(x => x.Id == id);

                if (null == veiculo)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_VEICULO_NOT_FOUND, id));
                }

                var veiculoDTO = new VeiculoDTO
                {
                    Id = veiculo.Id,
                    Placa = veiculo.Placa,
                    Modelo = veiculo.Modelo,
                    Ano = veiculo.Ano,
                    Mensalista = veiculo.Mensalista,
                    Cor = veiculo.Cor
                };

                if ((null != veiculo.VeiculosClientes) && (veiculo.VeiculosClientes.Count > 0))
                {
                    veiculoDTO.VeiculosClientes = new List<VeiculoClienteDTO>();
                    foreach (var vc in veiculo.VeiculosClientes)
                    {
                        veiculoDTO.VeiculosClientes.Add(
                            new VeiculoClienteDTO()
                            {
                                VeiculoId = veiculo.Id,
                                ClienteId = vc.ClienteId
                            }
                        );
                    }
                }

                return veiculoDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<VeiculoDTO>> GetVeiculos()
        {
            try
            {
                var veiculos = await _baseRepo.QueryAll()
                                                    .Include(f => f.VeiculosClientes)
                                                    .ToListAsync();

                var veiculosDTO = new List<VeiculoDTO>();
                foreach (var veiculo in veiculos)
                {
                    var veiculoDTO = new VeiculoDTO
                    {
                        Id = veiculo.Id,
                        Placa = veiculo.Placa,
                        Modelo = veiculo.Modelo,
                        Ano = veiculo.Ano,
                        Mensalista = veiculo.Mensalista,
                        Cor = veiculo.Cor
                    };

                    if ((null != veiculo.VeiculosClientes) && (veiculo.VeiculosClientes.Count > 0))
                    {
                        veiculoDTO.VeiculosClientes = new List<VeiculoClienteDTO>();
                        foreach (var vc in veiculo.VeiculosClientes)
                        {
                            veiculoDTO.VeiculosClientes.Add(
                                new VeiculoClienteDTO()
                                {
                                    VeiculoId = veiculo.Id,
                                    ClienteId = vc.ClienteId
                                }
                            );
                        }
                    }

                    veiculosDTO.Add(veiculoDTO);
                }

                return veiculosDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<VeiculoDTO>> GetVeiculosNotParked()
        {
            try
            {
                var veiculos = await _baseRepo.QueryAll()
                                                    .ToListAsync();



                var alocacoesCorrentes = await _alocacaoRepository.QueryAll()
                                                    .Include(f => f.Veiculo)
                                                    .Where(x => x.Saida == null)
                                                    .ToListAsync();


                //valida quais veículos não estão atualmente estacionados, e adiciona-os na lista de retorno
                var veiculosNaoEstacionados = new List<VeiculoDTO>();
                foreach (var veiculo in veiculos)
                {
                    //se o veículo estiver na lista de alocação, pula, ele não pode ser listado para uma nova alocação
                    if (alocacoesCorrentes.Exists(x => x.Veiculo.Placa == veiculo.Placa))
                    {
                        continue;
                    }

                    //veículo não alocado, cria o DTO e retorna os dados
                    var veiculoDTO = new VeiculoDTO
                    {
                        Id = veiculo.Id,
                        Placa = veiculo.Placa,
                        Modelo = veiculo.Modelo,
                        Ano = veiculo.Ano,
                        Mensalista = veiculo.Mensalista,
                        Cor = veiculo.Cor
                    };
                    veiculosNaoEstacionados.Add(veiculoDTO);
                }

                return veiculosNaoEstacionados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task<bool> VeiculoExists(int id)
        {
            return await _baseRepo.Exists(id);
        }


        public async Task<IEnumerable<RptVeiculoMensalistaDTO>> GetVeiculosMensalistas()
        {
            try
            {
                var veiculos = await _baseRepo.QueryAll()
                                                    .Where(v => v.Mensalista == true)
                                                    .Include(f => f.VeiculosClientes)
                                                        .ThenInclude(vc => vc.Cliente)
                                                    .ToListAsync();

                var veiculosMensalistas = new List<RptVeiculoMensalistaDTO>();
                foreach (var veiculo in veiculos)
                {
                    var veiculoMensalista = new RptVeiculoMensalistaDTO
                    {
                        Id = veiculo.Id,
                        Placa = veiculo.Placa,
                        Modelo = veiculo.Modelo,
                        Ano = veiculo.Ano,
                        Mensalista = veiculo.Mensalista,
                        Cor = veiculo.Cor
                    };

                    if ((null != veiculo.VeiculosClientes) && (veiculo.VeiculosClientes.Count > 0))
                    {
                        veiculoMensalista.Proprietarios = new List<ClienteDTO>();
                        foreach (var vc in veiculo.VeiculosClientes)
                        {
                            if (vc.Cliente != null)
                            {
                                veiculoMensalista.Proprietarios.Add(
                                    new ClienteDTO()
                                    {
                                        Id = vc.Cliente.Id,
                                        Nome = vc.Cliente.Nome,
                                        CPF = vc.Cliente.CPF,
                                        Telefone = vc.Cliente.Telefone
                                    }
                                );
                            }
                        }
                    }

                    veiculosMensalistas.Add(veiculoMensalista);
                }

                return veiculosMensalistas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<VeiculoDTO> GetVeiculoByPlateNumber(string placa)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.VeiculosClientes);

                var veiculo = await search.SingleOrDefaultAsync(x => x.Placa == placa);

                if (null == veiculo)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_VEICULO_BY_PLATE_NOT_FOUND, placa));
                }

                var veiculoDTO = new VeiculoDTO
                {
                    Id = veiculo.Id,
                    Placa = veiculo.Placa,
                    Modelo = veiculo.Modelo,
                    Ano = veiculo.Ano,
                    Mensalista = veiculo.Mensalista,
                    Cor = veiculo.Cor
                };

                if ((null != veiculo.VeiculosClientes) && (veiculo.VeiculosClientes.Count > 0))
                {
                    veiculoDTO.VeiculosClientes = new List<VeiculoClienteDTO>();
                    foreach (var vc in veiculo.VeiculosClientes)
                    {
                        veiculoDTO.VeiculosClientes.Add(
                            new VeiculoClienteDTO()
                            {
                                VeiculoId = veiculo.Id,
                                ClienteId = vc.ClienteId
                            }
                        );
                    }
                }

                return veiculoDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

}
