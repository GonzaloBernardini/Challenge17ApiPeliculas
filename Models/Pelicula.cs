using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Models
{
    public class Pelicula
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FechaBaja { get; set; }
        [JsonIgnore]
        public Alquiler? Alquiler { get; set; }
    }
}
