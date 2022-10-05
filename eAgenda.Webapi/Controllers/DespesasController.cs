using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Aplicacao.ModuloDespesa;
using Microsoft.AspNetCore.Authorization;
using eAgenda.Webapi.ViewModels.ModuloDespesa;
using static eAgenda.Webapi.ViewModels.ModuloDespesa.FormsDespesaViewModel;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DespesasController : eAgendaControllerBase
    {
        private readonly ServicoDespesa servicoDespesa;
        private readonly IMapper mapeadorDespesas;

        public DespesasController(ServicoDespesa servicoDespesa, IMapper mapeadorDespesas)
        {
            this.servicoDespesa = servicoDespesa;
            this.mapeadorDespesas = mapeadorDespesas;
        }

        [HttpGet]
        public ActionResult<List<ListarDespesaViewModel>> SelecionarTodos()
        {
            var DespesaResult = servicoDespesa.SelecionarTodos();

            if (DespesaResult.IsFailed)
                return InternalError(DespesaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorDespesas.Map<List<ListarDespesaViewModel>>(DespesaResult.Value)
            });
        }

        [HttpGet("visualizar-completa/{id:guid}")]
        public ActionResult<VisualizarDespesaViewModel> SelecionarDespesaCompletaPorId(Guid id)
        {
            var DespesaResult = servicoDespesa.SelecionarPorId(id);

            if (DespesaResult.IsFailed && RegistroNaoEncontrado(DespesaResult))
                return NotFound(DespesaResult);

            if (DespesaResult.IsFailed)
                return InternalError(DespesaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorDespesas.Map<VisualizarDespesaViewModel>(DespesaResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<FormsDespesaViewModel> Inserir(InserirDespesaViewModel DespesaVM)
        {
            var Despesa = mapeadorDespesas.Map<Despesa>(DespesaVM);

            Despesa.UsuarioId = UsuarioLogado.Id;

            var DespesaResult = servicoDespesa.Inserir(Despesa);

            if (DespesaResult.IsFailed)
                return InternalError(DespesaResult);

            return Ok(new
            {
                sucesso = true,
                dados = DespesaVM
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsDespesaViewModel> Editar(Guid id, EditarDespesaViewModel DespesaVM)
        {
            var DespesaResult = servicoDespesa.SelecionarPorId(id);

            if (DespesaResult.IsFailed && RegistroNaoEncontrado(DespesaResult))
                return NotFound(DespesaResult);

            var Despesa = mapeadorDespesas.Map(DespesaVM, DespesaResult.Value);

            DespesaResult = servicoDespesa.Editar(Despesa);

            if (DespesaResult.IsFailed)
                return InternalError(DespesaResult);

            return Ok(new
            {
                sucesso = true,
                dados = DespesaVM
            });
        }


        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var DespesaResult = servicoDespesa.Excluir(id);

            if (DespesaResult.IsFailed && RegistroNaoEncontrado<Despesa>(DespesaResult))
                return NotFound(DespesaResult);

            if (DespesaResult.IsFailed)
                return InternalError<Despesa>(DespesaResult);

            return NoContent();
        }
    }
}
