using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Webapi.ViewModels.ModuloContato;
using System;

namespace eAgenda.Webapi.ViewModels.ModuloCompromisso
{
    public class ListarCompromissoViewModel
    {
        public string Assunto { get; set; }
        public string Local { get; set; }
        public DateTime Data { get; set; }
        public string TipoLocal { get; set; }
        public FormsContatoViewModel Contato { get; set; }
    }
}
