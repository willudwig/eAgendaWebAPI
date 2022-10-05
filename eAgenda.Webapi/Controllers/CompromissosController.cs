using AutoMapper;
using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using eAgenda.Webapi.ViewModels.ModuloCompromisso;
using static eAgenda.Webapi.ViewModels.ModuloCompromisso.FormsCompromissoViewModel;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompromissosController : eAgendaControllerBase
    {
        private readonly ServicoCompromisso servicoCompromisso;
        private readonly IMapper mapeadorCompromissos;

        public CompromissosController(ServicoCompromisso servicoCompromisso, IMapper mapeadorCompromissos)
        {
            this.servicoCompromisso = servicoCompromisso;
            this.mapeadorCompromissos = mapeadorCompromissos;
        }

        [HttpGet]
        public ActionResult<List<ListarCompromissoViewModel>> SelecionarTodos()
        {
            var CompromissoResult = servicoCompromisso.SelecionarTodos();

            if (CompromissoResult.IsFailed)
                return InternalError(CompromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromissos.Map<List<ListarCompromissoViewModel>>(CompromissoResult.Value)
            });
        }


        [HttpGet("visualizar-completa/{id:guid}")]
        public ActionResult<VisualizarCompromissoViewModel> SelecionarCompromissoCompletaPorId(Guid id)
        {
            var CompromissoResult = servicoCompromisso.SelecionarPorId(id);

            if (CompromissoResult.IsFailed && RegistroNaoEncontrado(CompromissoResult))
                return NotFound(CompromissoResult);

            if (CompromissoResult.IsFailed)
                return InternalError(CompromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromissos.Map<VisualizarCompromissoViewModel>(CompromissoResult.Value)
            });
        }


        [HttpPost]
        public ActionResult<FormsCompromissoViewModel> Inserir(InserirCompromissoViewModel CompromissoVM)
        {
            var Compromisso = mapeadorCompromissos.Map<Compromisso>(CompromissoVM);

            Compromisso.UsuarioId = UsuarioLogado.Id;

            var CompromissoResult = servicoCompromisso.Inserir(Compromisso);

            if (CompromissoResult.IsFailed)
                return InternalError(CompromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = CompromissoVM
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsCompromissoViewModel> Editar(Guid id, EditarCompromissoViewModel CompromissoVM)
        {
            var CompromissoResult = servicoCompromisso.SelecionarPorId(id);

            if (CompromissoResult.IsFailed && RegistroNaoEncontrado(CompromissoResult))
                return NotFound(CompromissoResult);

            var Compromisso = mapeadorCompromissos.Map(CompromissoVM, CompromissoResult.Value);

            CompromissoResult = servicoCompromisso.Editar(Compromisso);

            if (CompromissoResult.IsFailed)
                return InternalError(CompromissoResult);

            return Ok(new
            {
                sucesso = true,
                dados = CompromissoVM
            });
        }


        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var CompromissoResult = servicoCompromisso.Excluir(id);

            if (CompromissoResult.IsFailed && RegistroNaoEncontrado<Compromisso>(CompromissoResult))
                return NotFound(CompromissoResult);

            if (CompromissoResult.IsFailed)
                return InternalError<Compromisso>(CompromissoResult);

            return NoContent();
        }
    }
}
