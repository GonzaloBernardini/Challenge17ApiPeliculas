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

namespace Challenge17ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquileresController : ControllerBase
    {
        //private readonly ApiPeliculasContext _context;
        private readonly IUnitOfWork context;
        public AlquileresController(IUnitOfWork context)
        {
            this.context = context;
        }

        // GET: api/Alquileres
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
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Alquiler))]
        [ProducesResponseType(400)]
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
        
        [HttpPut("{id}")]
        public  IActionResult PutAlquiler([FromBody]Alquiler alquiler, int id)
        {
            var busqueda = context.Alquiler.Find(id);
            if (busqueda == null)
            {
                return BadRequest();
            }
            //pelicula = context.Pelicula.Find(peli);
            
            //alquiler.Peliculas.Add(pelicula);
            //alquiler.Peliculas.Add(pelicula);
            //_context.Entry(alquiler).State = EntityState.Modified;
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public  ActionResult<Alquiler> PostAlquiler([FromBody]Alquiler alquiler,[FromQuery]int idpelicula)
        {
            var pelicula = context.Pelicula.Find(idpelicula);

            //List<Pelicula> list = new List<Pelicula>();
            //list.Add(pelicula);
            alquiler.Peliculas.Add(pelicula);
            context.Alquiler.Add(alquiler);
            context.Alquiler.Save();

            //CreatedAtAction("GetAlquiler", new { id = alquiler.Id }, alquiler);
            return Ok("Creado un Alquiler satisfactoriamente!");
        }

        // DELETE: api/Alquileres/5
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
