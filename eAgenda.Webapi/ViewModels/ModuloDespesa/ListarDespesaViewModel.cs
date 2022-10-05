using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.ModuloCategoria;
using System.Collections.Generic;
using System;

namespace eAgenda.Webapi.ViewModels.ModuloDespesa
{
    public class ListarDespesaViewModel
    {
        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public string FormaPagamento { get; set; } //FormaPgtoDespesaEnum

    }
}
