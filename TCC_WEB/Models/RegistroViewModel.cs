using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace TCC_WEB.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Informe o seu nome.")]
        [Display(Name = "Nome")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o seu sobrenome.")]
        [Display(Name = "Sobrenome")]
        [StringLength(100)]
        public string SobreNome { get; set; }

        [Required(ErrorMessage = "Informe o seu apelido.")]
        [Display(Name = "NomeUsuario")]
        [StringLength(50)]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "Informe uma idade válida.")]
        [Range(0, 120, ErrorMessage = "Entre um valor entre 0 e 120.")]
        [Display(Name = "Idade")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "Informe o email.")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
            ErrorMessage = "O email não possui um formato correto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe uma senha que contenha no mínimo 6 caracteres,tenha algum digito alfanumérico,dígito especial, letra maiusculas e minúscula.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
