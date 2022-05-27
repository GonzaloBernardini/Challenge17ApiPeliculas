using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Models
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public DateTime FechaBaja { get; set; }
        public Alquiler? Alquiler { get; set; }
    }
}
