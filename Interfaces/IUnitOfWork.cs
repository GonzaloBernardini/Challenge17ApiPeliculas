using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPeliculaRepository Pelicula { get; }
        IAlquilerRepository Alquiler { get; }
        int Complete();
    }
}
