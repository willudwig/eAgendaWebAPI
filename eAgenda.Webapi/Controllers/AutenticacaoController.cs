﻿using System;
using AutoMapper;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using eAgenda.Dominio.ModuloAutenticacao;
using eAgenda.Aplicacao.ModuloAutenticacao;
using eAgenda.Webapi.ViewModels.ModuloAutenticacao;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/conta")]
    [ApiController]
    public class AutenticacaoController : eAgendaControllerBase
    {
        private readonly ServicoAutenticacao servicoAutenticacao;
        private readonly IMapper mapeadorUsuario;

        public AutenticacaoController(ServicoAutenticacao servicoAutenticacao, IMapper mapeadorUsuario)
        {
            this.servicoAutenticacao = servicoAutenticacao;
            this.mapeadorUsuario = mapeadorUsuario;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> RegistrarUsuario(RegistrarUsuarioViewModel usuarioVM)
        {
            var usuario = mapeadorUsuario.Map<Usuario>(usuarioVM);

            var usuarioResult = await servicoAutenticacao.RegistrarUsuario(usuario, usuarioVM.Senha);

            if (usuarioResult.IsFailed)
                return InternalError(usuarioResult);

            return Ok(new
            {
                sucesso = true,
                dados = usuarioResult.Value
            });
        }


        [HttpPost("autenticar")]
        public async Task<ActionResult> AutenticarUsuario(AutenticarUsuarioViewModel usuarioVM)
        {
            var usuarioResult = await servicoAutenticacao.AutenticarUsuario(usuarioVM.Email, usuarioVM.Senha);

            if (usuarioResult.IsFailed)
                return BadRequest(usuarioResult);

            return Ok(new
            {
                sucesso = true,
                dados = GerarJwt(usuarioResult.Value)
            });
        }

        private TokenViewModel GerarJwt(Usuario usuario)
        {
            var identityClaims = new ClaimsIdentity();

            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, usuario.Email));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.GivenName, usuario.Nome));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SegredoSuperSecretoDoeAgenda");
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "eAgenda",
                Audience = "http://localhost",
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new TokenViewModel
            {
                Chave = encodedToken,
                UsuarioToken = new UsuarioTokenViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                }
            };

            return response;
        }
    }
}
