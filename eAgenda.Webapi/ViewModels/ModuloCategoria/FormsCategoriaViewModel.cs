using eAgenda.Webapi.ViewModels.ModuloCategoria;

namespace eAgenda.Webapi.ViewModels.ModuloCategoria
{
    public class FormsCategoriaViewModel
    {
        public string Titulo { get; set; }

        public class InserirCategoriaViewModel : FormsCategoriaViewModel
        {

        }

        public class EditarCategoriaViewModel : FormsCategoriaViewModel
        {

        }
    }
}
