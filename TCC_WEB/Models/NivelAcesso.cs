using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_WEB.Models
{
    public class NivelAcesso: IdentityRole
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
