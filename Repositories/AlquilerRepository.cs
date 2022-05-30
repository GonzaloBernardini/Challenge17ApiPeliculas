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
    public class AlquilerRepository : GenericRepository<Alquiler>, IAlquilerRepository
    {
        private readonly ApiPeliculasContext _context;

        public AlquilerRepository(ApiPeliculasContext context) : base(context)
        {
            _context = context;
        }

        public void BorrarAlquiler(int IdAlquiler)
        {
            Delete(IdAlquiler);

        }

        public Alquiler BuscarAlquiler(int IdAlquiler)
        {
            //probar si esta bien.
            return _context.Alquileres.Find(IdAlquiler);
        }

        public List<Alquiler> GetAlquileres()
        {
            /*_context.Productos.ToList();*/
            //Probar si esta bien.
            return GetAll().ToList();
        }

        public void InsertAlquiler(Alquiler alquiler_)
        {

            
            Add(alquiler_);
        }

        public Alquiler GetAlquileryPelicula(int id)
        {
            var consulta = _context.Alquileres.Where(x => x.Id == id).Where(x=>x.PeliculaId == id).Include(x => x.Peliculas).FirstOrDefault();
            return consulta;
        }

        public IEnumerable<Alquiler> GetPeliculasYAlquiler()
        {

            var consulta = _context.Alquileres.Include(x => x.Peliculas).ToList();
            
            return consulta;
        }

        public void GuardarCambios()
        {
            Save();
        }
        public IEnumerable<Alquiler> GetTodosAlquiler()
        {
            //lista = _dbcontext.Productos.Include(c => c.oCategoria).ToList();
            var consulta = _context.Alquileres.Include(x => x.Peliculas).ToList();
            return consulta;
        }

        
    }
}
