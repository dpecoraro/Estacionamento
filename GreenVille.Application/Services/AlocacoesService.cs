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
    public class AlocacoesService : IAlocacoesService
    {
        private const string ERR_MSG_ALOCACAO_NOT_FOUND = "Alocação de Id {0} não encontrado.";
        private const string ERR_MSG_VAGA_PARA_ALOCACAO_NOT_FOUND = "Vaga selecionada para Alocação não existe.";
        private const string ERR_MSG_VEICULO_PARA_ALOCACAO_NOT_FOUND = "Veículo selecionada para a Alocação não existe.";
        private const string ERR_MSG_ALOCACAO_POR_VEICULO_NOT_FOUND = "Não foram encontradas Alocações atuais para o Veículo de placa {0}.";
        private const string ERR_MSG_VEHICLE_ALREADY_ALLOCATED = "O Veículo de placa '{0}' já está alocado no Estacionamento '{1}' e Vaga '{2}'.";

        private const string ERR_MSG_ATENDENTEENT_PARA_ALOCACAO_NOT_FOUND = "Atendente de Entrada selecionado para a Alocação não existe.";
        private const string ERR_MSG_ATENDENTESAI_PARA_ALOCACAO_NOT_FOUND = "Atendente de Saída selecionado para a Alocação não existe.";
        private const string ERR_MSG_MANOBRISTAENT_PARA_ALOCACAO_NOT_FOUND = "Manobrista de Entrada selecionado para a Alocação não existe.";
        private const string ERR_MSG_MANOBRISTASAI_PARA_ALOCACAO_NOT_FOUND = "Manobrista de Saída selecionado para a Alocação não existe.";

        private const string ERR_MSG_ALOCACAO_ALREADY_EXISTS = "Alocação de Id {0} já existe na base de dados.";

        private readonly IAlocacaoRepository _baseRepo;
        private readonly IVagaRepository _vagaRepository;
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;

        public AlocacoesService(IAlocacaoRepository alocacaoRepository, IVagaRepository vagaRepository, IVeiculoRepository veiculoRepository,
            IFuncionarioRepository funcionarioRepository)
        {
            _baseRepo = alocacaoRepository;
            _vagaRepository = vagaRepository;
            _veiculoRepository = veiculoRepository;
            _funcionarioRepository = funcionarioRepository;
        }


        public async Task<AlocacaoDTO> AddAlocacao(AlocacaoDTO alocacao)
        {
            try
            {
                //checks if entry already exists
                var alocacaoRegisterExists = await AlocacaoExists(alocacao.Id);
                if (alocacaoRegisterExists)
                {
                    throw new RegisterDuplicatedException(string.Format(ERR_MSG_ALOCACAO_ALREADY_EXISTS, alocacao.Id));
                }

                //retrives Vaga
                var vaga = await _vagaRepository.GetAsync(alocacao.VagaId);
                if (null == vaga) { throw new RegisterNotFoundException(ERR_MSG_VAGA_PARA_ALOCACAO_NOT_FOUND); }

                //retrives Veículo
                var veiculo = await _veiculoRepository.GetAsync(alocacao.VeiculoId);
                if (null == veiculo) { throw new RegisterNotFoundException(ERR_MSG_VEICULO_PARA_ALOCACAO_NOT_FOUND); }


                //retrives Atendente Entrada
                if (0 != alocacao.AtendenteEntradaId)
                {
                    var atendenteEntrada = await _funcionarioRepository.GetAsync(alocacao.AtendenteEntradaId);
                    if (null == atendenteEntrada) { throw new RegisterNotFoundException(ERR_MSG_ATENDENTEENT_PARA_ALOCACAO_NOT_FOUND); }
                }

                //retrives Atendente Saída
                if (0 != alocacao.AtendenteSaidaId)
                {
                    var atendenteSaida = await _funcionarioRepository.GetAsync(alocacao.AtendenteSaidaId);
                    if (null == atendenteSaida) { throw new RegisterNotFoundException(ERR_MSG_ATENDENTESAI_PARA_ALOCACAO_NOT_FOUND); }
                }

                //retrives Manobrista Entrada
                if (0 != alocacao.ManobristaEntradaId)
                {
                    var manobristaEntrada = await _funcionarioRepository.GetAsync(alocacao.ManobristaEntradaId);
                    if (null == manobristaEntrada) { throw new RegisterNotFoundException(ERR_MSG_MANOBRISTAENT_PARA_ALOCACAO_NOT_FOUND); }
                }

                //retrives Manobrista Saída
                if (0 != alocacao.ManobristaSaidaId)
                {
                    var manobristaSaida = await _funcionarioRepository.GetAsync(alocacao.ManobristaSaidaId);
                    if (null == manobristaSaida) { throw new RegisterNotFoundException(ERR_MSG_MANOBRISTASAI_PARA_ALOCACAO_NOT_FOUND); }
                }

                //checks if vehicle is not already currently allocated in another spot
                var alocacaoForVehicleAlreadyExist = await _baseRepo.QueryAll()
                                                                        .Include(f => f.Vaga)
                                                                            .ThenInclude(v => v.Estacionamento)
                                                                        .Include(f => f.Veiculo)
                                                                        .Where(x =>
                                                                                x.Veiculo.Placa == veiculo.Placa &&
                                                                                x.Saida == null
                                                                        )
                                                                        .FirstOrDefaultAsync();

                if (null != alocacaoForVehicleAlreadyExist)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_VEHICLE_ALREADY_ALLOCATED,
                                                                            veiculo.Placa,
                                                                            alocacaoForVehicleAlreadyExist.Vaga.Estacionamento.NomeUnidade,
                                                                            alocacaoForVehicleAlreadyExist.Vaga.Nome
                                                                     )
                    );
                }


                //If everything is fine, register the vehicle
                var alocacaoRegister = new Alocacao
                {
                    VagaId = alocacao.VagaId,
                    Entrada = alocacao.Entrada,
                    VeiculoId = alocacao.VeiculoId,
                    ValorPago = alocacao.ValorPago,
                    Saida = alocacao.Saida,
                    Mensalista = veiculo.Mensalista,

                    AtendenteEntradaId = alocacao.AtendenteEntradaId,
                    AtendenteSaidaId = alocacao.AtendenteSaidaId == 0 ? null : (int?)alocacao.AtendenteSaidaId,

                    ManobristaEntradaId = alocacao.ManobristaEntradaId,
                    ManobristaSaidaId = alocacao.ManobristaSaidaId == 0 ? null : (int?)alocacao.ManobristaSaidaId,

                    EconomiaCarbono = alocacao.EconomiaCarbono
                };

                await _baseRepo.AddAsync(alocacaoRegister);

                //marca vaga como ocupada
                vaga.Ocupada = true;
                _vagaRepository.Update(vaga);


                //salve all changes
                await _baseRepo.SaveAllAsync();

                alocacao.Id = alocacaoRegister.Id;
                return alocacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<AlocacaoDTO> UpdateAlocacao(AlocacaoDTO alocacao)
        {
            try
            {
                //retrives Vaga
                var vaga = await _vagaRepository.GetAsync(alocacao.VagaId);
                if (null == vaga) { throw new RegisterNotFoundException(ERR_MSG_VAGA_PARA_ALOCACAO_NOT_FOUND); }

                //retrives Veículo
                var veiculo = await _veiculoRepository.GetAsync(alocacao.VeiculoId);
                if (null == veiculo) { throw new RegisterNotFoundException(ERR_MSG_VEICULO_PARA_ALOCACAO_NOT_FOUND); }


                //retrives Atendente Entrada
                if (0 != alocacao.AtendenteEntradaId)
                {
                    var atendenteEntrada = await _funcionarioRepository.GetAsync(alocacao.AtendenteEntradaId);
                    if (null == atendenteEntrada) { throw new RegisterNotFoundException(ERR_MSG_ATENDENTEENT_PARA_ALOCACAO_NOT_FOUND); }
                }

                //retrives Atendente Saída
                if (0 != alocacao.AtendenteSaidaId)
                {
                    var atendenteSaida = await _funcionarioRepository.GetAsync(alocacao.AtendenteSaidaId);
                    if (null == atendenteSaida) { throw new RegisterNotFoundException(ERR_MSG_ATENDENTESAI_PARA_ALOCACAO_NOT_FOUND); }
                }

                //retrives Manobrista Entrada
                if (0 != alocacao.ManobristaEntradaId)
                {
                    var manobristaEntrada = await _funcionarioRepository.GetAsync(alocacao.ManobristaEntradaId);
                    if (null == manobristaEntrada) { throw new RegisterNotFoundException(ERR_MSG_MANOBRISTAENT_PARA_ALOCACAO_NOT_FOUND); }
                }

                //retrives Manobrista Saída
                if (0 != alocacao.ManobristaSaidaId)
                {
                    var manobristaSaida = await _funcionarioRepository.GetAsync(alocacao.ManobristaSaidaId);
                    if (null == manobristaSaida) { throw new RegisterNotFoundException(ERR_MSG_MANOBRISTASAI_PARA_ALOCACAO_NOT_FOUND); }
                }


                //checks if alocacao already exists
                var alocacaoRegister = await _baseRepo.GetAsync(alocacao.Id);
                if (null == alocacaoRegister)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_ALOCACAO_NOT_FOUND, alocacao.Id));
                }
                else
                {
                    //guarda ID da vaga antiga para liberação posterior
                    int vagaAntigaId = alocacaoRegister.VagaId;

                    //atualiza registro da alocação
                    alocacaoRegister.VagaId = alocacao.VagaId;
                    alocacaoRegister.Entrada = alocacao.Entrada;
                    alocacaoRegister.ValorPago = alocacao.ValorPago;
                    alocacaoRegister.Saida = alocacao.Saida;
                    alocacaoRegister.Mensalista = veiculo.Mensalista;

                    alocacaoRegister.AtendenteEntradaId = alocacao.AtendenteEntradaId;
                    alocacaoRegister.AtendenteSaidaId = alocacao.AtendenteSaidaId == 0 ? null : (int?)alocacao.AtendenteSaidaId;

                    alocacaoRegister.ManobristaEntradaId = alocacao.ManobristaEntradaId;
                    alocacaoRegister.ManobristaSaidaId = alocacao.ManobristaSaidaId == 0 ? null : (int?)alocacao.ManobristaSaidaId;

                    alocacaoRegister.EconomiaCarbono = alocacao.EconomiaCarbono;

                    _baseRepo.Update(alocacaoRegister);



                    //Atualiza situação vagas
                    vaga.Ocupada = !alocacaoRegister.Saida.HasValue;    //se houver data de saída, a vaga está desocupada, se não houve, o veículo ainda a ocupa
                    _vagaRepository.Update(vaga);

                    //Se houve troca de vagas, libera a antiga
                    if (vagaAntigaId != alocacao.VagaId)
                    {
                        var vagaAntiga = await _vagaRepository.GetAsync(vagaAntigaId);
                        if (null != vagaAntiga)
                        {
                            vagaAntiga.Ocupada = false;
                            _vagaRepository.Update(vagaAntiga);
                        }
                    }

                }

                //salve all changes
                await _baseRepo.SaveAllAsync();

                return alocacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> DeleteAlocacao(int id)
        {
            try
            {
                //checks if entry already exists
                var alocacaoRegisterExists = await AlocacaoExists(id);
                if (!alocacaoRegisterExists)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_ALOCACAO_NOT_FOUND, id));
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


        public async Task<AlocacaoDTO> GetAlocacao(int id)
        {
            try
            {
                var search = _baseRepo.QueryAll()
                                        .Include(f => f.Vaga)
                                            .ThenInclude(v => v.Estacionamento)
                                        .Include(f => f.Veiculo)
                                        .Include(f => f.AtendenteEntrada)
                                        .Include(f => f.AtendenteSaida)
                                        .Include(f => f.ManobristaEntrada)
                                        .Include(f => f.ManobristaSaida);

                var alocacao = await search.SingleOrDefaultAsync(x => x.Id == id);

                if (null == alocacao)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_ALOCACAO_NOT_FOUND, id));
                }

                var alocacaoDTO = new AlocacaoDTO
                {
                    Id = alocacao.Id,

                    VagaId = alocacao.VagaId,
                    VagaNome = alocacao.Vaga.Nome,
                    EstacionamentoId = alocacao.Vaga.EstacionamentoId,
                    EstacionamentoUnidade = alocacao.Vaga.Estacionamento.NomeUnidade,

                    VeiculoId = alocacao.VeiculoId,
                    VeiculoPlaca = alocacao.Veiculo.Placa,

                    Entrada = alocacao.Entrada,
                    ValorPago = alocacao.ValorPago,
                    Mensalista = alocacao.Mensalista,
                    Saida = alocacao.Saida,

                    AtendenteEntradaId = ((int)(alocacao.AtendenteEntradaId == null ? 0 : alocacao.AtendenteEntradaId)),
                    AtendenteEntradaCPF = alocacao.AtendenteEntrada?.CPF,
                    AtendenteEntradaNome = alocacao.AtendenteEntrada?.Nome,

                    AtendenteSaidaId = ((int)(alocacao.AtendenteSaidaId == null ? 0 : alocacao.AtendenteSaidaId)),
                    AtendenteSaidaCPF = alocacao.AtendenteSaida?.CPF,
                    AtendenteSaidaNome = alocacao.AtendenteSaida?.Nome,

                    ManobristaEntradaId = ((int)(alocacao.ManobristaEntradaId == null ? 0 : alocacao.ManobristaEntradaId)),
                    ManobristaEntradaCPF = alocacao.ManobristaEntrada?.CPF,
                    ManobristaEntradaNome = alocacao.ManobristaEntrada?.Nome,

                    ManobristaSaidaId = ((int)(alocacao.ManobristaSaidaId == null ? 0 : alocacao.ManobristaSaidaId)),
                    ManobristaSaidaCPF = alocacao.ManobristaSaida?.CPF,
                    ManobristaSaidaNome = alocacao.ManobristaSaida?.Nome,

                    EconomiaCarbono = alocacao.EconomiaCarbono
                };

                return alocacaoDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<AlocacaoDTO>> GetAlocacoes()
        {
            try
            {
                var alocacoes = await _baseRepo.QueryAll()
                                                    .Include(f => f.Vaga)
                                                        .ThenInclude(v => v.Estacionamento)
                                                    .Include(f => f.Veiculo)
                                                    .Include(f => f.AtendenteEntrada)
                                                    .Include(f => f.AtendenteSaida)
                                                    .Include(f => f.ManobristaEntrada)
                                                    .Include(f => f.ManobristaSaida)
                                                    .ToListAsync();

                var alocacoesDTO = new List<AlocacaoDTO>();
                foreach (var alocacao in alocacoes)
                {
                    var alocacaoDTO = new AlocacaoDTO
                    {
                        Id = alocacao.Id,

                        VagaId = alocacao.VagaId,
                        VagaNome = alocacao.Vaga.Nome,
                        EstacionamentoId = alocacao.Vaga.EstacionamentoId,
                        EstacionamentoUnidade = alocacao.Vaga.Estacionamento.NomeUnidade,

                        VeiculoId = alocacao.VeiculoId,
                        VeiculoPlaca = alocacao.Veiculo.Placa,

                        Entrada = alocacao.Entrada,
                        ValorPago = alocacao.ValorPago,
                        Mensalista = alocacao.Mensalista,
                        Saida = alocacao.Saida,

                        AtendenteEntradaId = ((int)(alocacao.AtendenteEntradaId == null ? 0 : alocacao.AtendenteEntradaId)),
                        AtendenteEntradaCPF = alocacao.AtendenteEntrada?.CPF,
                        AtendenteEntradaNome = alocacao.AtendenteEntrada?.Nome,

                        AtendenteSaidaId = ((int)(alocacao.AtendenteSaidaId == null ? 0 : alocacao.AtendenteSaidaId)),
                        AtendenteSaidaCPF = alocacao.AtendenteSaida?.CPF,
                        AtendenteSaidaNome = alocacao.AtendenteSaida?.Nome,

                        ManobristaEntradaId = ((int)(alocacao.ManobristaEntradaId == null ? 0 : alocacao.ManobristaEntradaId)),
                        ManobristaEntradaCPF = alocacao.ManobristaEntrada?.CPF,
                        ManobristaEntradaNome = alocacao.ManobristaEntrada?.Nome,

                        ManobristaSaidaId = ((int)(alocacao.ManobristaSaidaId == null ? 0 : alocacao.ManobristaSaidaId)),
                        ManobristaSaidaCPF = alocacao.ManobristaSaida?.CPF,
                        ManobristaSaidaNome = alocacao.ManobristaSaida?.Nome,

                        EconomiaCarbono = alocacao.EconomiaCarbono
                    };

                    alocacoesDTO.Add(alocacaoDTO);
                }

                return alocacoesDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<VeiculoAlocadoDTO>> GetCurrentAlocacoesDtoByEstacionamento(int idEstacionamento)
        {
            try
            {
                var alocacoes = await _baseRepo.QueryAll()
                                                       .Include(f => f.Veiculo)
                                                       .Include(f => f.Vaga)
                                                           .ThenInclude(v => v.Estacionamento)
                                                       .Where(x =>
                                                            x.Vaga.EstacionamentoId == idEstacionamento &&
                                                            x.Saida == null
                                                       )
                                                       .ToListAsync();

                var veiculosAlocadosList = new List<VeiculoAlocadoDTO>();

                foreach (var alocacao in alocacoes)
                {
                    //Adicona registro para cada entrada de veículo
                    var veiculoAlocado = new VeiculoAlocadoDTO
                    {
                        Id = alocacao.Id,
                        Entrada = alocacao.Entrada,
                        VagaNome = alocacao.Vaga.Nome,
                        VeiculoPlaca = alocacao.Veiculo.Placa
                    };
                    veiculosAlocadosList.Add(veiculoAlocado);
                }

                var orderedList = await Task.FromResult(veiculosAlocadosList.OrderBy(x => x.Entrada));
                return orderedList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<AlocacaoDTO> GetAlocacaoDtoByVehiclePlate(string plateNumber)
        {
            try
            {
                var alocacao = await _baseRepo.QueryAll()
                                                .Include(f => f.Vaga)
                                                    .ThenInclude(v => v.Estacionamento)
                                                .Include(f => f.Veiculo)
                                                .Include(f => f.AtendenteEntrada)
                                                .Include(f => f.AtendenteSaida)
                                                .Include(f => f.ManobristaEntrada)
                                                .Include(f => f.ManobristaSaida)
                                                .Where(x =>
                                                        x.Veiculo.Placa == plateNumber &&
                                                        x.Saida == null
                                                )
                                                .FirstOrDefaultAsync();

                if (null == alocacao)
                {
                    throw new RegisterNotFoundException(string.Format(ERR_MSG_ALOCACAO_POR_VEICULO_NOT_FOUND, plateNumber));
                }

                var alocacaoDTO = new AlocacaoDTO
                {
                    Id = alocacao.Id,

                    VagaId = alocacao.VagaId,
                    VagaNome = alocacao.Vaga.Nome,
                    EstacionamentoId = alocacao.Vaga.EstacionamentoId,
                    EstacionamentoUnidade = alocacao.Vaga.Estacionamento.NomeUnidade,

                    VeiculoId = alocacao.VeiculoId,
                    VeiculoPlaca = alocacao.Veiculo.Placa,

                    Entrada = alocacao.Entrada,
                    ValorPago = alocacao.ValorPago,
                    Mensalista = alocacao.Mensalista,
                    Saida = alocacao.Saida,

                    AtendenteEntradaId = ((int)(alocacao.AtendenteEntradaId == null ? 0 : alocacao.AtendenteEntradaId)),
                    AtendenteEntradaCPF = alocacao.AtendenteEntrada?.CPF,
                    AtendenteEntradaNome = alocacao.AtendenteEntrada?.Nome,

                    AtendenteSaidaId = ((int)(alocacao.AtendenteSaidaId == null ? 0 : alocacao.AtendenteSaidaId)),
                    AtendenteSaidaCPF = alocacao.AtendenteSaida?.CPF,
                    AtendenteSaidaNome = alocacao.AtendenteSaida?.Nome,

                    ManobristaEntradaId = ((int)(alocacao.ManobristaEntradaId == null ? 0 : alocacao.ManobristaEntradaId)),
                    ManobristaEntradaCPF = alocacao.ManobristaEntrada?.CPF,
                    ManobristaEntradaNome = alocacao.ManobristaEntrada?.Nome,

                    ManobristaSaidaId = ((int)(alocacao.ManobristaSaidaId == null ? 0 : alocacao.ManobristaSaidaId)),
                    ManobristaSaidaCPF = alocacao.ManobristaSaida?.CPF,
                    ManobristaSaidaNome = alocacao.ManobristaSaida?.Nome,

                    EconomiaCarbono = alocacao.EconomiaCarbono
                };

                return alocacaoDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<RptEntradaSaidaVeiculoDTO>> ListParkingMovementByVehicle(int idVeiculo, DateTime startPeriod, DateTime endPeriod)
        {
            try
            {
                var alocacoes = await _baseRepo.QueryAll()
                                                    .Where(x =>
                                                                x.VeiculoId == idVeiculo &&
                                                                (x.Entrada >= startPeriod && (x.Saida <= endPeriod || x.Saida == null))
                                                    )
                                                    .Include(f => f.Veiculo)
                                                    .Include(f => f.AtendenteEntrada)
                                                    .Include(f => f.AtendenteSaida)
                                                    .Include(f => f.ManobristaEntrada)
                                                    .Include(f => f.ManobristaSaida)
                                                    .Include(f => f.Vaga)
                                                        .ThenInclude(v => v.Estacionamento)
                                                    .ToListAsync();

                var entradasSaidasPorVeiculo = new List<RptEntradaSaidaVeiculoDTO>();
                foreach (var alocacao in alocacoes)
                {
                    //Adicona registro para cada entrada de veículo
                    var entradaVeiculo = new RptEntradaSaidaVeiculoDTO
                    {
                        Entrada = true,
                        Momento = alocacao.Entrada,
                        EstacionamentoUnidade = alocacao.Vaga.Estacionamento.NomeUnidade,
                        VagaNome = alocacao.Vaga.Nome,
                        VeiculoPlaca = alocacao.Veiculo.Placa,
                        Mensalista = alocacao.Mensalista,
                        ValorPago = 0
                    };
                    entradasSaidasPorVeiculo.Add(entradaVeiculo);

                    //Caso tenha já uma data de saída, adciona como saída do veículo também
                    if (null != alocacao.Saida)
                    {
                        var saidaVeiculo = new RptEntradaSaidaVeiculoDTO
                        {
                            Entrada = false,
                            Momento = alocacao.Saida.Value,
                            EstacionamentoUnidade = alocacao.Vaga.Estacionamento.NomeUnidade,
                            VagaNome = alocacao.Vaga.Nome,
                            VeiculoPlaca = alocacao.Veiculo.Placa,
                            Mensalista = alocacao.Mensalista,
                            ValorPago = alocacao.ValorPago
                        };

                        entradasSaidasPorVeiculo.Add(saidaVeiculo);
                    }
                }

                var orderedList = await Task.FromResult(entradasSaidasPorVeiculo.OrderBy(x => x.Momento));
                return orderedList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<IEnumerable<RptUsoHoraEstacionamento>> ListParkingUseByHour(DateTime startPeriod, DateTime endPeriod)
        {
            try
            {
                var alocacoesPorEstac = await _baseRepo.QueryAll()
                                                    .Include(f => f.Vaga)
                                                        .ThenInclude(v => v.Estacionamento)
                                                    .Where(x => (x.Saida != null) && (x.Entrada >= startPeriod && x.Saida < endPeriod))
                                                    .ToListAsync();

                var alocacoesPorEstacGrouped = alocacoesPorEstac.GroupBy(x => x.Vaga.Estacionamento.NomeUnidade);


                var usoPorHoraList = new List<RptUsoHoraEstacionamento>();
                foreach (var alocacoes in alocacoesPorEstacGrouped)
                {
                    var usoPorHoraEstacionamento = new RptUsoHoraEstacionamento
                    {
                        NomeEstacionamento = alocacoes.Key
                    };

                    usoPorHoraEstacionamento.Alocacoes_00a01 = alocacoes.Count(x => (x.Entrada.Hour == 00) || (x.Saida.Value.Hour == 00) || (x.Entrada.Hour <= 00 && x.Saida.Value.Hour >= 00));
                    usoPorHoraEstacionamento.Alocacoes_01a02 = alocacoes.Count(x => (x.Entrada.Hour == 01) || (x.Saida.Value.Hour == 01) || (x.Entrada.Hour <= 01 && x.Saida.Value.Hour >= 01));
                    usoPorHoraEstacionamento.Alocacoes_02a03 = alocacoes.Count(x => (x.Entrada.Hour == 02) || (x.Saida.Value.Hour == 02) || (x.Entrada.Hour <= 02 && x.Saida.Value.Hour >= 02));
                    usoPorHoraEstacionamento.Alocacoes_03a04 = alocacoes.Count(x => (x.Entrada.Hour == 03) || (x.Saida.Value.Hour == 03) || (x.Entrada.Hour <= 03 && x.Saida.Value.Hour >= 03));
                    usoPorHoraEstacionamento.Alocacoes_04a05 = alocacoes.Count(x => (x.Entrada.Hour == 04) || (x.Saida.Value.Hour == 04) || (x.Entrada.Hour <= 04 && x.Saida.Value.Hour >= 04));
                    usoPorHoraEstacionamento.Alocacoes_05a06 = alocacoes.Count(x => (x.Entrada.Hour == 05) || (x.Saida.Value.Hour == 05) || (x.Entrada.Hour <= 05 && x.Saida.Value.Hour >= 05));
                    usoPorHoraEstacionamento.Alocacoes_06a07 = alocacoes.Count(x => (x.Entrada.Hour == 06) || (x.Saida.Value.Hour == 06) || (x.Entrada.Hour <= 06 && x.Saida.Value.Hour >= 06));
                    usoPorHoraEstacionamento.Alocacoes_07a08 = alocacoes.Count(x => (x.Entrada.Hour == 07) || (x.Saida.Value.Hour == 07) || (x.Entrada.Hour <= 07 && x.Saida.Value.Hour >= 07));
                    usoPorHoraEstacionamento.Alocacoes_08a09 = alocacoes.Count(x => (x.Entrada.Hour == 08) || (x.Saida.Value.Hour == 08) || (x.Entrada.Hour <= 08 && x.Saida.Value.Hour >= 08));
                    usoPorHoraEstacionamento.Alocacoes_09a10 = alocacoes.Count(x => (x.Entrada.Hour == 09) || (x.Saida.Value.Hour == 09) || (x.Entrada.Hour <= 09 && x.Saida.Value.Hour >= 09));
                    usoPorHoraEstacionamento.Alocacoes_10a11 = alocacoes.Count(x => (x.Entrada.Hour == 10) || (x.Saida.Value.Hour == 10) || (x.Entrada.Hour <= 10 && x.Saida.Value.Hour >= 10));
                    usoPorHoraEstacionamento.Alocacoes_11a12 = alocacoes.Count(x => (x.Entrada.Hour == 11) || (x.Saida.Value.Hour == 11) || (x.Entrada.Hour <= 11 && x.Saida.Value.Hour >= 11));
                    usoPorHoraEstacionamento.Alocacoes_12a13 = alocacoes.Count(x => (x.Entrada.Hour == 12) || (x.Saida.Value.Hour == 12) || (x.Entrada.Hour <= 12 && x.Saida.Value.Hour >= 12));
                    usoPorHoraEstacionamento.Alocacoes_13a14 = alocacoes.Count(x => (x.Entrada.Hour == 13) || (x.Saida.Value.Hour == 13) || (x.Entrada.Hour <= 13 && x.Saida.Value.Hour >= 13));
                    usoPorHoraEstacionamento.Alocacoes_14a15 = alocacoes.Count(x => (x.Entrada.Hour == 14) || (x.Saida.Value.Hour == 14) || (x.Entrada.Hour <= 14 && x.Saida.Value.Hour >= 14));
                    usoPorHoraEstacionamento.Alocacoes_15a16 = alocacoes.Count(x => (x.Entrada.Hour == 15) || (x.Saida.Value.Hour == 15) || (x.Entrada.Hour <= 15 && x.Saida.Value.Hour >= 15));
                    usoPorHoraEstacionamento.Alocacoes_16a17 = alocacoes.Count(x => (x.Entrada.Hour == 16) || (x.Saida.Value.Hour == 16) || (x.Entrada.Hour <= 16 && x.Saida.Value.Hour >= 16));
                    usoPorHoraEstacionamento.Alocacoes_17a18 = alocacoes.Count(x => (x.Entrada.Hour == 17) || (x.Saida.Value.Hour == 17) || (x.Entrada.Hour <= 17 && x.Saida.Value.Hour >= 17));
                    usoPorHoraEstacionamento.Alocacoes_18a19 = alocacoes.Count(x => (x.Entrada.Hour == 18) || (x.Saida.Value.Hour == 18) || (x.Entrada.Hour <= 18 && x.Saida.Value.Hour >= 18));
                    usoPorHoraEstacionamento.Alocacoes_19a20 = alocacoes.Count(x => (x.Entrada.Hour == 19) || (x.Saida.Value.Hour == 19) || (x.Entrada.Hour <= 19 && x.Saida.Value.Hour >= 19));
                    usoPorHoraEstacionamento.Alocacoes_20a21 = alocacoes.Count(x => (x.Entrada.Hour == 20) || (x.Saida.Value.Hour == 20) || (x.Entrada.Hour <= 20 && x.Saida.Value.Hour >= 20));
                    usoPorHoraEstacionamento.Alocacoes_21a22 = alocacoes.Count(x => (x.Entrada.Hour == 21) || (x.Saida.Value.Hour == 21) || (x.Entrada.Hour <= 21 && x.Saida.Value.Hour >= 21));
                    usoPorHoraEstacionamento.Alocacoes_22a23 = alocacoes.Count(x => (x.Entrada.Hour == 22) || (x.Saida.Value.Hour == 22) || (x.Entrada.Hour <= 22 && x.Saida.Value.Hour >= 22));
                    usoPorHoraEstacionamento.Alocacoes_23a24 = alocacoes.Count(x => (x.Entrada.Hour == 23) || (x.Saida.Value.Hour == 23) || (x.Entrada.Hour <= 23 && x.Saida.Value.Hour >= 23));

                    usoPorHoraList.Add(usoPorHoraEstacionamento);
                }

                var orderedList = await Task.FromResult(usoPorHoraList.OrderBy(x => x.NomeEstacionamento));
                return orderedList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private async Task<bool> AlocacaoExists(int id)
        {
            return await _baseRepo.Exists(id);
        }

    }
}
