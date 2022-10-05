using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloCompromisso;
using eAgenda.Webapi.ViewModels.ModuloContato;
using static eAgenda.Webapi.ViewModels.ModuloCompromisso.FormsCompromissoViewModel;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class CompromissoProfile : Profile
    {
        public CompromissoProfile()
        {
            ConverterDeEntidadeParaViewModel();
            ConverterDeViewModelParaEntidade();
        }

        private void ConverterDeViewModelParaEntidade()
        {
            CreateMap<InserirCompromissoViewModel, Compromisso>()
                .ForMember(destino => destino.Contato, opt => opt.Ignore())
                .AfterMap((viewModel, compromisso) =>
                {
                    if (viewModel.Contato == null)
                        return;

                    var contato = new Contato();

                    contato.Nome = viewModel.Contato.Nome;
                    contato.Email = viewModel.Contato.Email;
                    contato.Telefone = viewModel.Contato.Telefone;
                    contato.Empresa = viewModel.Contato.Empresa;
                    contato.Cargo = viewModel.Contato.Cargo;

                    compromisso.Contato = contato;

                });


            CreateMap<EditarCompromissoViewModel, Compromisso>()
               .ForMember(destino => destino.Contato, opt => opt.Ignore());
        }

        private void ConverterDeEntidadeParaViewModel()
        {
            CreateMap<Compromisso, ListarCompromissoViewModel>()
               .ForMember(destino => destino.TipoLocal, opt => opt.MapFrom(origem => origem.TipoLocal.GetDescription()));

            CreateMap<Compromisso, VisualizarCompromissoViewModel>()
               .ForMember(destino => destino.TipoLocal, opt => opt.MapFrom(origem => origem.TipoLocal.GetDescription()));

            CreateMap<Contato, VisualizarContatoViewModel>();
        }
    }
}
