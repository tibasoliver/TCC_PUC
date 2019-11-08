using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_WEB.Models
{
    public class Consulta
    {
        public int ConsultaId { get; set; }

        //[Key,Column(Order = 1)]
        [Required(ErrorMessage = "Informe a data.")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Informe o nome do paciente.")]
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        [Required(ErrorMessage = "Informe o nome do médico.")]
        [StringLength(150)]
        public string Médico { get; set; }

        //[Key, Column(Order = 2)]
        [Required(ErrorMessage = "Informe o horário da consulta.")]
        [EnumDataType(typeof(Horario))]
        public Horario horario { get; set; }

        public enum Horario
        {
            [DisplayName("7:00AM")] seteam,
            [DisplayName("8:00AM")] oitoam,
            [DisplayName("9:00AM")] noveam,
            [DisplayName("10:00AM")] dezam,
            [DisplayName("11:00AM")] onzeam,
            [DisplayName("12:00AM")] dozeam,
            [DisplayName("1:00PM")] umapm,
            [DisplayName("2:00PM")] duaspm,
            [DisplayName("3:00PM")] trespm,
            [DisplayName("4:00PM")] quatropm,
            [DisplayName("5:00PM")] cincopm
        }
    }
}
