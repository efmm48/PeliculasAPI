using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Data;
using PeliculasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public PeliculaController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Pelicula>))]
        [ProducesResponseType(400)] //Bad Request
        public async Task<IActionResult> GetPeliculas()
        {
            var lista = await _db.Peliculas.OrderBy(p => p.Nombre).Include(p => p.Categoria).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("{id:int}", Name = "GetPeliculas")]
        [ProducesResponseType(200, Type = typeof(Pelicula))]
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found, si no encuentra ningun objeto.
        public async Task<IActionResult> GetPelicula(int id)
        {
            var obj = await _db.Peliculas.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            return Ok(obj);
        }

        [HttpPost]
        [ProducesResponseType(201)] //Good Request
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(500)] //Error Interno
        public async Task<IActionResult> CrearPelicula([FromBody] Pelicula pelicula)
        {

            if (pelicula == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _db.AddAsync(pelicula);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetPelicula", new { id = pelicula.Id }, pelicula);
        }

        [HttpPut]
        [ProducesResponseType(204)] //Good Request
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(500)] //Error Interno
        public async Task<IActionResult> ActualizarPelicula(int id, Pelicula pelicula)
        {
            if (id == pelicula.Id)
            {
                _db.Entry(pelicula).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete]
        [ProducesResponseType(200)] //Good Request
        [ProducesResponseType(404)] //Not Found
        [ProducesResponseType(500)] //Error Interno
        public async Task<IActionResult> BorrarPelicula(int id)
        {

            var pelicula = await _db.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            _db.Peliculas.Remove(pelicula);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
