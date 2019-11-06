using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_WEB.Models
{
    public class Medicamento
    {

        public int MedicamentoId { get; set; }

        [Required(ErrorMessage = "Informe o nome genérico do medicamento.")]
        [Display(Name = "Nome Genérico")]
        [StringLength(150)]
        public string NomeGenerico { get; set; }

        [Required(ErrorMessage = "Informe o nome de fábrica do medicamento.")]
        [Display(Name = "Nome de Fábrica")]
        [StringLength(150)]
        public string NomedeFabrica { get; set; }

        [Required(ErrorMessage = "Informe o nome do fabricante.")]
        [Display(Name = "Fabricante")]
        [StringLength(80)]
        public string Fabricante { get; set; }

        public ICollection<Receita> Receitas { get; set; }
    }
}
