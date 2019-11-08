using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_WEB.Models
{
    public class TipodeExame
    {
        public int TipodeExameId { get; set; }

        [Required(ErrorMessage = "Informe o tipo do exame.")]
        [Display(Name = "Nome do Exame")]
        [StringLength(70)]
        public string NomedoExame { get; set; }

        public ICollection<SolicitacaodeExame> SolicitacoesdeExame { get; set; }

        public ICollection<RecebimentodeExame> RecebimentosdeExame { get; set; }
    }
}
