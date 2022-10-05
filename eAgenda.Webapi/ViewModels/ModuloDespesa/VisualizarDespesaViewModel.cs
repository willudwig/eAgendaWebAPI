using System;
using System.Collections.Generic;
using eAgenda.Webapi.ViewModels.ModuloCategoria;

namespace eAgenda.Webapi.ViewModels.ModuloDespesa
{
    public class VisualizarDespesaViewModel
    {
        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public string FormaPagamento { get; set; } //FormaPgtoDespesaEnum

        public List<VisualizarCategoriaViewModel> Categorias { get; set; }

    }
}
