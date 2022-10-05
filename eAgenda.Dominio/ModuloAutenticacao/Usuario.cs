﻿using System;
using Microsoft.AspNetCore.Identity;

namespace eAgenda.Dominio.ModuloAutenticacao
{
    public class Usuario : IdentityUser<Guid>
    {
        //informações customizadas
        public string Nome { get; set; }

    }
}
