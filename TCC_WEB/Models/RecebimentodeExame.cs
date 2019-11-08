using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_WEB.Models
{
    public class RecebimentodeExame
    {

        public int RecebimentodeExameId { get; set; }

        [Required(ErrorMessage = "Informe a data.")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Informe o tipo de exame.")]
        public int TipodeExameId { get; set; }
        public TipodeExame TipodeExame { get; set; } //

        [Required(ErrorMessage = "Informe o valor.")]
        public string Dado { get; set; }

        [Required(ErrorMessage = "Informe o nome do paciente.")]
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        [Required(ErrorMessage = "Informe se o paciente vai receber ou não o resultado.")]
        public bool recebe { get; set; }
        

    }
}
