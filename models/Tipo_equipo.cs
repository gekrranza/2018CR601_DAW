using System;
using System.ComponentModel.DataAnnotations;
namespace apiPracticas.Models
{
    public class Tipo_equipo
    {
        [Key]
        public int id_tipo_equipo { get; set; }
        public string descripcio { get; set; }
        public string estado { get; set; }        
    }
}