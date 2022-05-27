using Challenge17ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Interfaces
{
    public interface IPeliculaRepository : IGenericRepository<Pelicula>
    {
        IEnumerable<Pelicula> GetPeliculasYAlquiler();
        Pelicula GetAlquileryPelicula(int id);
    }
}
