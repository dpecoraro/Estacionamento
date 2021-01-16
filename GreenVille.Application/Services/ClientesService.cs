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
    public class ClientesService : IClientesService
    {
        private const string ERR_MSG_CLIENTE_NOT_FOUND = "Cliente de Id {0} não encontrado.";
        private const string ERR_MSG_CLIENTE_NOT_FOUND_BY_CPF = "Cliente de CPF '{0}' não encontrado.";
        private const string ERR_MSG_CLIENTE_ALREADY_EXISTS = "Cliente de Id {0} já existe na base de dados.";

        private readonly IClienteRepository _baseRepo;

        public ClientesService(IClienteRepository clienteRepository)
        {
            _baseRepo = clienteRepository;
        }


        public async Task<ClienteDTO> AddCliente(ClienteDTO cliente)
        {
            try
            {
                //checks if entry already exists
                var clienteRegisterExists = await ClienteExists(cliente.Id);
                if (clienteRegisterExists)
                {
                    throw new RegisterDuplicatedException(string.Format(ERR_MSG_CLIENTE_ALREADY_EXISTS, cliente.Id));
                }

                var clienteRegister = new Cliente()
                {
                    Nome = cliente.Nome,
                    CPF = cliente.CPF,
                    Telefone = cliente.Telefone
                };

                if ((null != cliente.VeiculosClientes) && (cliente.VeiculosClientes.Count > 0))
                {
                    clienteRegister.VeiculosClientes = new List<VeiculoCliente>();
                    foreach (var vc in cliente.VeiculosClientes)
                    {
                        clienteRegister.VeiculosClientes.Add(
                            new VeiculoCliente()
                            {
                                ClienteId = clienteRegister.Id,
                                VeiculoId = vc.VeiculoId
                            }
                        );
                    }
                }

                await _baseRepo.AddAsync(clienteRegister);


                //salve all changes
                await _baseRepo.SaveAllAsync();

                cliente.Id = clienteRegister.Id;
                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ClienteDTO> UpdateCliente(ClienteDTO cliente)
        {
            try
            {
                //checks if cliente already exists
                var clienteRegister = await _baseRepo.GetAsync(cliente.Id);
                if (null == clienteRegister)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_CLIENTE_NOT_FOUND, cliente.Id));
                }
                else
                {
                    clienteRegister.Nome = cliente.Nome;
                    clienteRegister.CPF = cliente.CPF;
                    clienteRegister.Telefone = cliente.Telefone;

                    _baseRepo.Update(clienteRegister);
                }

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> DeleteCliente(int id)
        {
            try
            {
                //checks if entry already exists
                var clienteRegisterExists = await ClienteExists(id);
                if (!clienteRegisterExists)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_CLIENTE_NOT_FOUND, id));
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


        public async Task<ClienteDTO> GetCliente(int id)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.VeiculosClientes);

                var cliente = await search.SingleOrDefaultAsync(x => x.Id == id);

                if (null == cliente)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_CLIENTE_NOT_FOUND, id));
                }

                var clienteDTO = new ClienteDTO
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    CPF = cliente.CPF,
                    Telefone = cliente.Telefone,
                    VeiculosClientes = new List<VeiculoClienteDTO>()
                };

                if ((null != cliente.VeiculosClientes) && (cliente.VeiculosClientes.Count > 0))
                {
                    foreach (var vc in cliente.VeiculosClientes)
                    {
                        clienteDTO.VeiculosClientes.Add(
                            new VeiculoClienteDTO()
                            {
                                ClienteId = cliente.Id,
                                VeiculoId = vc.VeiculoId
                            }
                        );
                    }
                }

                return clienteDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<ClienteDTO>> GetClientes()
        {
            try
            {
                var clientes = await _baseRepo.QueryAll()
                                                    .Include(f => f.VeiculosClientes)
                                                    .ToListAsync();

                var clientesDTO = new List<ClienteDTO>();
                foreach (var cliente in clientes)
                {
                    var clienteDTO = new ClienteDTO
                    {
                        Id = cliente.Id,
                        Nome = cliente.Nome,
                        CPF = cliente.CPF,
                        Telefone = cliente.Telefone,
                        VeiculosClientes = new List<VeiculoClienteDTO>()
                    };

                    if ((null != cliente.VeiculosClientes) && (cliente.VeiculosClientes.Count > 0))
                    {
                        foreach (var vc in cliente.VeiculosClientes)
                        {
                            clienteDTO.VeiculosClientes.Add(
                                new VeiculoClienteDTO()
                                {
                                    ClienteId = cliente.Id,
                                    VeiculoId = vc.VeiculoId
                                }
                            );
                        }
                    }

                    clientesDTO.Add(clienteDTO);
                }

                return clientesDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ClienteDTO> GetClientePorCPF(string cpf)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.VeiculosClientes);

                var cliente = await search.SingleOrDefaultAsync(x => x.CPF == cpf);

                if (null == cliente)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_CLIENTE_NOT_FOUND_BY_CPF, cpf));
                }

                var clienteDTO = new ClienteDTO
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    CPF = cliente.CPF,
                    Telefone = cliente.Telefone,
                    VeiculosClientes = new List<VeiculoClienteDTO>()
                };

                if ((null != cliente.VeiculosClientes) && (cliente.VeiculosClientes.Count > 0))
                {
                    foreach (var vc in cliente.VeiculosClientes)
                    {
                        clienteDTO.VeiculosClientes.Add(
                            new VeiculoClienteDTO()
                            {
                                ClienteId = cliente.Id,
                                VeiculoId = vc.VeiculoId
                            }
                        );
                    }
                }

                return clienteDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task<bool> ClienteExists(int id)
        {
            return await _baseRepo.Exists(id);
        }

    }
}
