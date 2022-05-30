using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Models
{
    public class Alquiler
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Fecha { get; set; }
        public int Precio { get; set; }
        [JsonIgnore]
        public List<Pelicula> Peliculas { get; set; } = new();
    }
}
