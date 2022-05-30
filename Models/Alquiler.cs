using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Models
{
    public class Alquiler
    {
        [Key]
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public DateTime Fecha { get; set; }
        public int Precio { get; set; }
        public IEnumerable<Pelicula> Peliculas { get; set; }
    }
}
