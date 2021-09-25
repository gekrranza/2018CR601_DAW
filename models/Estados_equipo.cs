using System;
using System.ComponentModel.DataAnnotations;
namespace apiPracticas.Models
{
    public class Estado_equipo
    {
        [Key]
        public int id_estado_equipo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }        
    }
}