using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenVille.Repository.Migrations
{
    public partial class AzureDBTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: false),
                    Telefone = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estacionamentos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeUnidade = table.Column<string>(nullable: false),
                    ValorHora = table.Column<double>(nullable: false),
                    GeracaoCreditosCarbonoHora = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estacionamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(nullable: false),
                    Modelo = table.Column<string>(nullable: true),
                    Cor = table.Column<string>(nullable: true),
                    Ano = table.Column<int>(maxLength: 4, nullable: false),
                    Mensalista = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPF = table.Column<string>(maxLength: 14, nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    CargoId = table.Column<int>(nullable: false),
                    EstacionamentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcionarios_Cargos_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funcionarios_Estacionamentos_EstacionamentoId",
                        column: x => x.EstacionamentoId,
                        principalTable: "Estacionamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vagas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: false),
                    Ocupada = table.Column<bool>(nullable: false),
                    Interditada = table.Column<bool>(nullable: false),
                    EstacionamentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vagas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vagas_Estacionamentos_EstacionamentoId",
                        column: x => x.EstacionamentoId,
                        principalTable: "Estacionamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VeiculosClientes",
                columns: table => new
                {
                    VeiculoId = table.Column<int>(nullable: false),
                    ClienteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeiculosClientes", x => new { x.ClienteId, x.VeiculoId });
                    table.ForeignKey(
                        name: "FK_VeiculosClientes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VeiculosClientes_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Senha = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    DataHoraLogin = table.Column<DateTime>(nullable: true),
                    DataHoraLogout = table.Column<DateTime>(nullable: true),
                    FuncionarioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alocacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MomentoEntrada = table.Column<DateTime>(nullable: false),
                    MomentoSaida = table.Column<DateTime>(nullable: true),
                    ValorPago = table.Column<float>(nullable: false),
                    Mensalista = table.Column<bool>(nullable: false),
                    VagaId = table.Column<int>(nullable: false),
                    VeiculoId = table.Column<int>(nullable: false),
                    AtendenteEntradaId = table.Column<int>(nullable: true),
                    AtendenteSaidaId = table.Column<int>(nullable: true),
                    ManobristaEntradaId = table.Column<int>(nullable: true),
                    ManobristaSaidaId = table.Column<int>(nullable: true),
                    EconomiaCarbono = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alocacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alocacoes_Funcionarios_AtendenteEntradaId",
                        column: x => x.AtendenteEntradaId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alocacoes_Funcionarios_AtendenteSaidaId",
                        column: x => x.AtendenteSaidaId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alocacoes_Funcionarios_ManobristaEntradaId",
                        column: x => x.ManobristaEntradaId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alocacoes_Funcionarios_ManobristaSaidaId",
                        column: x => x.ManobristaSaidaId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alocacoes_Vagas_VagaId",
                        column: x => x.VagaId,
                        principalTable: "Vagas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alocacoes_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_AtendenteEntradaId",
                table: "Alocacoes",
                column: "AtendenteEntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_AtendenteSaidaId",
                table: "Alocacoes",
                column: "AtendenteSaidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_Id",
                table: "Alocacoes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_ManobristaEntradaId",
                table: "Alocacoes",
                column: "ManobristaEntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_ManobristaSaidaId",
                table: "Alocacoes",
                column: "ManobristaSaidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_VagaId",
                table: "Alocacoes",
                column: "VagaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_VeiculoId",
                table: "Alocacoes",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_Id",
                table: "Cargos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Id",
                table: "Clientes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Estacionamentos_Id",
                table: "Estacionamentos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_CargoId",
                table: "Funcionarios",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_EstacionamentoId",
                table: "Funcionarios",
                column: "EstacionamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_Id",
                table: "Funcionarios",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FuncionarioId",
                table: "Usuarios",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Id",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_EstacionamentoId",
                table: "Vagas",
                column: "EstacionamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_Id",
                table: "Vagas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Id",
                table: "Veiculos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VeiculosClientes_VeiculoId",
                table: "VeiculosClientes",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_VeiculosClientes_ClienteId_VeiculoId",
                table: "VeiculosClientes",
                columns: new[] { "ClienteId", "VeiculoId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alocacoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "VeiculosClientes");

            migrationBuilder.DropTable(
                name: "Vagas");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "Estacionamentos");
        }
    }
}
