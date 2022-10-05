using eAgenda.Dominio;
using eAgenda.Infra.Orm;
using eAgenda.Infra.Configs;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Aplicacao.ModuloAutenticacao;
using Microsoft.Extensions.DependencyInjection;
using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Infra.Orm.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infra.Orm.ModuloDespesa;
using eAgenda.Aplicacao.ModuloDespesa;

namespace eAgenda.Webapi.Config
{
    public static class DependencyInjectionConfig
    {
        public static void ConfigurarInjecaoDependencia(this IServiceCollection services)
        {
            services.AddSingleton((x) => new ConfiguracaoAplicacaoeAgenda().ConnectionStrings);

            services.AddScoped<eAgendaDbContext>();

            services.AddScoped<IContextoPersistencia, eAgendaDbContext>();

            services.AddScoped<IRepositorioTarefa, RepositorioTarefaOrm>();
            services.AddScoped<IRepositorioContato, RepositorioContatoOrm>();
            services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoOrm>();
            services.AddScoped<IRepositorioDespesa, RepositorioDespesaOrm>();
            services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();

            services.AddTransient<ServicoTarefa>();
            services.AddTransient<ServicoContato>();
            services.AddTransient<ServicoCompromisso>();
            services.AddTransient<ServicoDespesa>();
            services.AddTransient<ServicoCategoria>();
            services.AddTransient<ServicoAutenticacao>();
        }

    }
}
