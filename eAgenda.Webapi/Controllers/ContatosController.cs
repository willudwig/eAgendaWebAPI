using AutoMapper;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Webapi.ViewModels.ModuloContato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static eAgenda.Webapi.ViewModels.ModuloContato.FormsContatoViewModel;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContatosController : eAgendaControllerBase
    {
        private readonly ServicoContato servicoContato;
        private readonly IMapper mapeadorContatos;

        public ContatosController(ServicoContato servicoContato, IMapper mapeadorContatos)
        {
            this.servicoContato = servicoContato;
            this.mapeadorContatos = mapeadorContatos;
        }

        [HttpGet]
        public ActionResult<List<ListarContatoViewModel>> SelecionarTodos()
        {
            var ContatoResult = servicoContato.SelecionarTodos();

            if (ContatoResult.IsFailed)
                return InternalError(ContatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<List<ListarContatoViewModel>>(ContatoResult.Value)
            });
        }


        [HttpGet("visualizar-completa/{id:guid}")]
        public ActionResult<VisualizarContatoViewModel> SelecionarContatoCompletaPorId(Guid id)
        {
            var ContatoResult = servicoContato.SelecionarPorId(id);

            if (ContatoResult.IsFailed && RegistroNaoEncontrado(ContatoResult))
                return NotFound(ContatoResult);

            if (ContatoResult.IsFailed)
                return InternalError(ContatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<VisualizarContatoViewModel>(ContatoResult.Value)
            });
        }


        [HttpPost]
        public ActionResult<FormsContatoViewModel> Inserir(InserirContatoViewModel ContatoVM)
        {
            var Contato = mapeadorContatos.Map<Contato>(ContatoVM);

            Contato.UsuarioId = UsuarioLogado.Id;

            var ContatoResult = servicoContato.Inserir(Contato);

            if (ContatoResult.IsFailed)
                return InternalError(ContatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = ContatoVM
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsContatoViewModel> Editar(Guid id, EditarContatoViewModel ContatoVM)
        {
            var ContatoResult = servicoContato.SelecionarPorId(id);

            if (ContatoResult.IsFailed && RegistroNaoEncontrado(ContatoResult))
                return NotFound(ContatoResult);

            var Contato = mapeadorContatos.Map(ContatoVM, ContatoResult.Value);

            ContatoResult = servicoContato.Editar(Contato);

            if (ContatoResult.IsFailed)
                return InternalError(ContatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = ContatoVM
            });
        }


        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var ContatoResult = servicoContato.Excluir(id);

            if (ContatoResult.IsFailed && RegistroNaoEncontrado<Contato>(ContatoResult))
                return NotFound(ContatoResult);

            if (ContatoResult.IsFailed)
                return InternalError<Contato>(ContatoResult);

            return NoContent();
        }

    }
}
