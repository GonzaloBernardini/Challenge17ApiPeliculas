using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge17ApiPeliculas.Data;
using Challenge17ApiPeliculas.Models;
using Challenge17ApiPeliculas.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Challenge17ApiPeliculas.Controllers
{
    
    [SwaggerTag("Api de Peliculas")]
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        //private readonly ApiPeliculasContext _context;
        private readonly IUnitOfWork context;

        public PeliculasController(IUnitOfWork context)
        {
            this.context = context;
        }

        // GET: api/Peliculas
        /// <summary>
        /// Este metodo nos devuelve todas las peliculas
        /// </summary>
        /// <remarks>Devuelve una lista de peliculas</remarks>
        
        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public  ActionResult<IEnumerable<Pelicula>> GetPeliculas()
        {
            return context.Pelicula.GetAll().ToList();
        }

        // GET: api/Peliculas/5
        /// <summary>
        /// Devuelve una unica pelicula especificando su ID
        /// </summary>
        /// <remarks>Requiere saber el Id de la pelicula</remarks>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public  ActionResult<Pelicula> GetPelicula(int id)
        {
            var pelicula =  context.Pelicula.Find(id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return pelicula;
        }

        // PUT: api/Peliculas/5
        /// <summary>
        /// Permite modificar una pelicula especifica por su ID
        /// </summary>
        /// <remarks>Requiere un Id de pelicula para funcionar</remarks>

        [HttpPut("{id}")]
        public  IActionResult PutPelicula(int id,[FromBody] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return BadRequest();
            }

            context.Pelicula.Add(pelicula);

            try
            {
                 context.Pelicula.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeliculaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Peliculas
        /// <summary>
        /// Nos permite crear una nueva pelicula
        /// </summary>
        /// <remarks>Creamos pelicula.</remarks>
        [HttpPost]
        public  ActionResult<Pelicula> PostPelicula([FromBody]Pelicula pelicula)
        {
            context.Pelicula.Add(pelicula);
            context.Pelicula.Save();

            return CreatedAtAction("GetPelicula", new { id = pelicula.Id }, pelicula);
        }

        // DELETE: api/Peliculas/5
        /// <summary>
        /// Nos permite Borrar una unica pelicula
        /// </summary>
        
        [HttpDelete("{id}")]
        public  IActionResult DeletePelicula(int id)
        {
            var pelicula =  context.Pelicula.Find(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            context.Pelicula.Delete(id);
             context.Pelicula.Save();

            return NoContent();
        }

        private bool PeliculaExists(int id)
        {
            var result = context.Pelicula.Find(id);
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}
