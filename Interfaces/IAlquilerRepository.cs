using Challenge17ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Interfaces
{
    public interface IAlquilerRepository : IGenericRepository<Alquiler>
    {
        IEnumerable<Alquiler> GetPeliculasYAlquiler();
        Alquiler GetAlquileryPelicula(int id);

        IEnumerable<Alquiler> GetTodosAlquiler();

        
    }
}
