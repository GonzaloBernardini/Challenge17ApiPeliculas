using Challenge17ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge17ApiPeliculas.Data
{
    public class ApiPeliculasContext : DbContext
    {
        public ApiPeliculasContext(DbContextOptions<ApiPeliculasContext> options ) : base(options)
        {

        }
        public DbSet<Alquiler> Alquileres { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
    }
}
