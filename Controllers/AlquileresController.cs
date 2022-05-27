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
        public  ActionResult<IEnumerable<Alquiler>> GetAlquileres()
        {
            var alquiler = context.Alquiler.GetPeliculasYAlquiler().ToList();
            return Ok(alquiler);
            
        }

        // GET: api/Alquileres/5
        [HttpGet("{id}")]
        public  ActionResult<Alquiler> GetAlquiler(int id)
        {
            var alquiler =  context.Alquiler.GetAlquileryPelicula(id);

            if (alquiler == null)
            {
                return NotFound();
            }

            return alquiler;
        }

        // PUT: api/Alquileres/5
        
        [HttpPut("{id}")]
        public  IActionResult PutAlquiler(int id, [FromBody]Alquiler alquiler)
        {
            if (id != alquiler.Id)
            {
                return BadRequest();
            }

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
        public  ActionResult<Alquiler> PostAlquiler([FromBody]Alquiler alquiler)
        {
            context.Alquiler.Add(alquiler);
             context.Alquiler.Save();

            return CreatedAtAction("GetAlquiler", new { id = alquiler.Id }, alquiler);
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
