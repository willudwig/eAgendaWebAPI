using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.ModuloCategoria;
using eAgenda.Webapi.ViewModels.ModuloDespesa;
using static eAgenda.Webapi.ViewModels.ModuloDespesa.FormsDespesaViewModel;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class DespesaProfile : Profile
    {
        public DespesaProfile()
        {
            ConverterDeEntidadeParaViewModel();
            ConverterDeViewModelParaEntidade();
        }

        private void ConverterDeViewModelParaEntidade()
        {
            CreateMap<InserirDespesaViewModel, Despesa>()
                .ForMember(destino => destino.Categorias, opt => opt.Ignore())
                .AfterMap((viewModel, despesa) =>
                {
                    if (viewModel.Categorias == null)
                        return;

                    foreach (var itemVM in viewModel.Categorias)
                    {
                        var item = new Categoria();

                        item.Titulo = itemVM.Titulo ;

                        despesa.AtribuirCategoria(item);
                    }
                });

            CreateMap<EditarDespesaViewModel, Despesa>()
               .ForMember(destino => destino.Categorias, opt => opt.Ignore());
        }

        private void ConverterDeEntidadeParaViewModel()
        {
            CreateMap<Despesa, ListarDespesaViewModel>()
                .ForMember(destino => destino.FormaPagamento, opt => opt.MapFrom(origem => origem.FormaPagamento.GetDescription()));

            CreateMap<Despesa, VisualizarDespesaViewModel>()
                .ForMember(destino => destino.FormaPagamento, opt => opt.MapFrom(origem => origem.FormaPagamento.GetDescription()));

            CreateMap<Categoria, VisualizarCategoriaViewModel>();
        }
    }
}
