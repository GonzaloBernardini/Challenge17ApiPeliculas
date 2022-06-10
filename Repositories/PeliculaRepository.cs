using Challenge17ApiPeliculas.Data;
using Challenge17ApiPeliculas.Interfaces;
using Challenge17ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Repositories
{
    public class PeliculaRepository : GenericRepository<Pelicula>, IPeliculaRepository
    {
        private readonly ApiPeliculasContext _context;

        public PeliculaRepository(ApiPeliculasContext context) : base(context)
        {
            _context = context;
        }

        public  void BorrarPelicula(int IdPelicula)
        {
            Delete(IdPelicula);

        }

        public Pelicula BuscarPelicula(int IdPelicula)
        {
            //probar si esta bien.
            return _context.Peliculas.Find(IdPelicula);
        }

        public IEnumerable<Pelicula> GetPeliculas()
        {
            /*_context.Productos.ToList();*/
            //Probar si esta bien.
            return GetAll().ToList();
        }

        public void InsertPelicula(Pelicula pelicula_)
        {
            Add(pelicula_);
        }

        public Pelicula GetAlquileryPelicula(int id)
        {
            var consulta = _context.Peliculas.Where(x => x.Id == id).Include(x => x.Alquiler).FirstOrDefault();
            return consulta;
        }

        public IEnumerable<Pelicula> GetPeliculasYAlquiler()
        {
            var consulta = _context.Peliculas.Include(x => x.Alquiler).ToList();
            return consulta;
        }




        public void GuardarCambios()
        {
            Save();
        }

    }
}
