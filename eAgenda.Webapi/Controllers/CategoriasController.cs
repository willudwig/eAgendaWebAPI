using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using eAgenda.Webapi.ViewModels.ModuloCategoria;
using System.Collections.Generic;
using System;
using eAgenda.Aplicacao.ModuloDespesa;
using static eAgenda.Webapi.ViewModels.ModuloCategoria.FormsCategoriaViewModel;
using eAgenda.Dominio.ModuloDespesa;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController : eAgendaControllerBase
    {
        private readonly ServicoCategoria servicoCategoria;
        private readonly IMapper mapeadorCategorias;

        public CategoriasController(ServicoCategoria servicoCategoria, IMapper mapeadorCategorias)
        {
            this.servicoCategoria = servicoCategoria;
            this.mapeadorCategorias = mapeadorCategorias;
        }

        [HttpGet]
        public ActionResult<List<ListarCategoriaViewModel>> SelecionarTodos()
        {
            var CategoriaResult = servicoCategoria.SelecionarTodos();

            if (CategoriaResult.IsFailed)
                return InternalError(CategoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategorias.Map<List<ListarCategoriaViewModel>>(CategoriaResult.Value)
            });
        }


        [HttpGet("visualizar-completa/{id:guid}")]
        public ActionResult<VisualizarCategoriaViewModel> SelecionarCategoriaCompletaPorId(Guid id)
        {
            var CategoriaResult = servicoCategoria.SelecionarPorId(id);

            if (CategoriaResult.IsFailed && RegistroNaoEncontrado(CategoriaResult))
                return NotFound(CategoriaResult);

            if (CategoriaResult.IsFailed)
                return InternalError(CategoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategorias.Map<VisualizarCategoriaViewModel>(CategoriaResult.Value)
            });
        }


        [HttpPost]
        public ActionResult<FormsCategoriaViewModel> Inserir(InserirCategoriaViewModel CategoriaVM)
        {
            var Categoria = mapeadorCategorias.Map<Categoria>(CategoriaVM);

            Categoria.UsuarioId = UsuarioLogado.Id;

            var CategoriaResult = servicoCategoria.Inserir(Categoria);

            if (CategoriaResult.IsFailed)
                return InternalError(CategoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = CategoriaVM
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<FormsCategoriaViewModel> Editar(Guid id, EditarCategoriaViewModel CategoriaVM)
        {
            var CategoriaResult = servicoCategoria.SelecionarPorId(id);

            if (CategoriaResult.IsFailed && RegistroNaoEncontrado(CategoriaResult))
                return NotFound(CategoriaResult);

            var Categoria = mapeadorCategorias.Map(CategoriaVM, CategoriaResult.Value);

            CategoriaResult = servicoCategoria.Editar(Categoria);

            if (CategoriaResult.IsFailed)
                return InternalError(CategoriaResult);

            return Ok(new
            {
                sucesso = true,
                dados = CategoriaVM
            });
        }


        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var CategoriaResult = servicoCategoria.Excluir(id);

            if (CategoriaResult.IsFailed && RegistroNaoEncontrado<Categoria>(CategoriaResult))
                return NotFound(CategoriaResult);

            if (CategoriaResult.IsFailed)
                return InternalError<Categoria>(CategoriaResult);

            return NoContent();
        }
    }

}
