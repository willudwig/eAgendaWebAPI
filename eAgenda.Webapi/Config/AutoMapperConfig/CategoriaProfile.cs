using AutoMapper;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.ModuloCategoria;
using static eAgenda.Webapi.ViewModels.ModuloCategoria.FormsCategoriaViewModel;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            ConverterDeEntidadeParaViewModel();
            ConverterDeViewModelParaEntidade();
        }

        private void ConverterDeViewModelParaEntidade()
        {
            CreateMap<InserirCategoriaViewModel, Categoria>();

            CreateMap<EditarCategoriaViewModel, Categoria>();
        }

        private void ConverterDeEntidadeParaViewModel()
        {
            CreateMap<Categoria, ListarCategoriaViewModel>();

            CreateMap<Categoria, VisualizarCategoriaViewModel>();
        }
    }
}
