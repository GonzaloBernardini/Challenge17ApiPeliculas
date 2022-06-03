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
using Challenge17ApiPeliculas.IdentityAuth;

namespace Challenge17ApiPeliculas.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [SwaggerTag("Api de Peliculas")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlquileresController : ControllerBase
    {
        
        private readonly IUnitOfWork context;
        public AlquileresController(IUnitOfWork context)
        {
            this.context = context;
        }

        // GET: api/Alquileres        
        /// <summary>
        /// Obtenemos un listado de todos los Alquileres con sus respectivas peliculas
        /// </summary>        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Alquiler>))]
        public  ActionResult<IEnumerable<Alquiler>> GetAlquileres()
        {

            var alquiler = context.Alquiler.GetTodosAlquiler();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(alquiler);
            
        }

        // GET: api/Alquileres/5        
        /// <summary>
        /// Obtenemos un Alquiler especifico por su Id
        /// </summary>
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Alquiler))]
        [ProducesResponseType(404)]
        public  ActionResult<Alquiler> GetAlquiler(int id)
        {
            var alquiler =  context.Alquiler.GetAlquileryPelicula(id);

            if (alquiler == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(alquiler);
        }

        // PUT: api/Alquileres/5        
        /// <summary>
        /// Nos permite actualizar un alquiler por su ID
        /// </summary>
        /// <remarks>Para actualizar especificar el Id del alquiler</remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public  IActionResult PutAlquiler([FromBody]Alquiler alquiler, int id)
        {
            var busqueda = context.Alquiler.Find(id);
            if (busqueda == null)
            {
                return BadRequest();
            }
            
            context.Alquiler.Update(alquiler);

            try
            {
                context.Alquiler.Save();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlquilerExists(id))
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

        // POST: api/Alquileres        
        /// <summary>
        /// Nos permite crear un nuevo alquiler
        /// </summary>
        /// <remarks>Para crearlo debemos especificar un ID de la pelicula a asignar</remarks>
        [HttpPost]
        
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public  ActionResult<Alquiler> PostAlquiler([FromBody]Alquiler alquiler,[FromQuery]int idpelicula)
        {
            var pelicula = context.Pelicula.Find(idpelicula);

            
            alquiler.Peliculas.Add(pelicula);
            context.Alquiler.Add(alquiler);
            context.Alquiler.Save();

            //devuelve http response201 , usarlo solo si es asincronico
            //CreatedAtAction("GetAlquiler", new { id = alquiler.Id }, alquiler);
            return Ok("Creado un Alquiler satisfactoriamente!");
        }

        // DELETE: api/Alquileres/5
        /// <summary>
        /// Nos permite Borrar un unico alquiler por su ID
        /// </summary>
        [HttpDelete("{id}")]
        public  IActionResult DeleteAlquiler(int id)
        {
            var alquiler =  context.Alquiler.Find(id);
            if (alquiler == null)
            {
                return NotFound();
            }

            context.Alquiler.Delete(id);
             context.Alquiler.Save();

            return NoContent();
        }

        private bool AlquilerExists(int id)
        {
            var result = context.Alquiler.Find(id);
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}
