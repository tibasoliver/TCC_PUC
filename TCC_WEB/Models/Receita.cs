using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_WEB.Models
{
    public class Receita
    {
        public int ReceitaId { get; set; }

        [Required(ErrorMessage = "Informe a data.")]
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Informe o nome genérico do medicamento.")]
        public int MedicamentoId { get; set; }
        public Medicamento Medicamento { get; set; }

        [Required(ErrorMessage = "Informe a dose adequada.")]
        [StringLength(100)]
        public string Dose { get; set; }

        [Required(ErrorMessage = "Informe o nome do paciente.")]
        public int PacienteId { get; set; }
        
        public Paciente Paciente { get; set; }

        [Required(ErrorMessage = "Informe o nome do médico.")]
        [StringLength(150)]
        public string Médico { get; set; }
    }
}
