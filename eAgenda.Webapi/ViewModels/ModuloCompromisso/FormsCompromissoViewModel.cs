using System;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Webapi.ViewModels.ModuloContato;

namespace eAgenda.Webapi.ViewModels.ModuloCompromisso
{
    public class FormsCompromissoViewModel
    {
        public string Assunto { get; set; }
        public string Local { get; set; }
        public DateTime Data { get ; set; }
        public TipoLocalizacaoCompromissoEnum TipoLocal { get; set; }
        public FormsContatoViewModel Contato { get; set; }


        public class InserirCompromissoViewModel : FormsCompromissoViewModel
        {

        }

        public class EditarCompromissoViewModel : FormsCompromissoViewModel
        {

        }
    }
}
