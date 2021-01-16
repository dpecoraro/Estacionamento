using GreenVille.Application.Services;
using GreenVille.Domain;
using GreenVille.Domain.Interfaces.IRepositories;
using GreenVille.Domain.Interfaces.IServices;
using GreenVille.Repository.Context;
using GreenVille.Repository.Repositories;
using GreenVille.Security.SecurityHandler;
using GreenVille.Security.SecurityHandler.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Text;

namespace GreenVille.API
{
    public static class ConfigureResources
    {
        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("Appsettings").Get<Appsettings>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionDetail.DataSource,
                    providerOptions => providerOptions.EnableRetryOnFailure());
            });

        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DataContext>();

            services.AddScoped<IEstacionamentoRepository, EstacionamentoRepository>();
            services.AddScoped<IAlocacaoRepository, AlocacaoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            services.AddScoped<IVagaRepository, VagaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();

            services.AddScoped<IGreenVilleService, GreenVilleService>();
            services.AddScoped<IFuncionariosService, FuncionariosService>();
            services.AddScoped<IEstacionamentosService, EstacionamentosService>();
            services.AddScoped<IUsuariosService, UsuariosService>();
            services.AddScoped<IVagasService, VagasService>();
            services.AddScoped<IClientesService, ClientesService>();
            services.AddScoped<IAlocacoesService, AlocacoesService>();
            services.AddScoped<IVeiculosService, VeiculosService>();

            services.AddScoped<IAuthService, AuthService>();

        }

        public static void ConfigurePackages(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Appsettings>(configuration);
            var settings = configuration.GetSection("Appsettings").Get<Appsettings>();

            services.AddSingleton<Appsettings>(settings);


            #region [ Swagger Config ]

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.ReportApiVersions = true;
                x.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "GreenVille", Version = "v1" });

                config.OperationFilter<RemoveVersionFromParameter>();

                config.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                var xmlFile = $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}.XML";

                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);

                config.IncludeXmlComments(xmlPath);
            });

            #endregion

            #region [ JWT Config ]
                        
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = settings.JwtSettings.validIssuer,
                    ValidAudience = settings.JwtSettings.validAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSettings.securityKey))
                };
            });

            #endregion
        }
    }

    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");

            operation.Parameters.Remove(versionParameter);
        }
    }

    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();

            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
            }

            swaggerDoc.Paths = paths;
        }
    }

}
