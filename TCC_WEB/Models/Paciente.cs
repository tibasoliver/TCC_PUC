using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_WEB.Models
{
    public class Paciente
    {
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "Informe o seu nome.")]
        [Display(Name = "Nome")]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o seu sobrenome.")]
        [Display(Name = "Sobrenome")]
        [StringLength(100)]
        public string SobreNome { get; set; }

        [Required(ErrorMessage = "Informe uma idade válida.")]
        [Range(0, 120, ErrorMessage = "Entre um valor entre 0 e 120.")]
        [Display(Name = "Idade")]
        public int Idade { get; set; }

        public ICollection<Receita> Receitas { get; set; }
        public ICollection<SolicitacaodeExame> SolicitacoesdeExame { get; set; }
        public ICollection<Consulta> Consultas { get; set; }
    }
}
