using Challenge17ApiPeliculas.Data;
using Challenge17ApiPeliculas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiPeliculasContext _context;
        public UnitOfWork(ApiPeliculasContext context)
        {
            _context = context;
            Pelicula = new PeliculaRepository(_context);
            Alquiler = new AlquilerRepository(_context);
        }


        public IPeliculaRepository Pelicula { get; private set; }
        public IAlquilerRepository Alquiler { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
