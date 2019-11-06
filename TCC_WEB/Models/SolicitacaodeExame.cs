using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_WEB.Models
{
    public class SolicitacaodeExame
    {
        public int SolicitacaodeExameId { get; set; }

        [Required(ErrorMessage = "Informe a data.")]
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Informe o tipo de exame.")]
        public int TipodeExameId { get; set; }
        //[Required(ErrorMessage = "Informe o tipo de exame.")]
        public TipodeExame TipodeExame { get; set; } //

        [Required(ErrorMessage = "Informe o nome do médico.")]
        [StringLength(150)]
        public string Médico { get; set; }

        [Required(ErrorMessage = "Informe o nome do paciente.")]
        public int PacienteId { get; set; }
        //[Required(ErrorMessage = "Informe o nome do paciente.")]
        public Paciente Paciente { get; set; }
    }
}
