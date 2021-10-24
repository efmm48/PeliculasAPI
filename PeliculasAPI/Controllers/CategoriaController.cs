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
    public class CategoriaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoriaController(ApplicationDbContext db)
        {
            _db = db;    
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(List<Categoria>))]
        [ProducesResponseType(400)] //Bad Request
        public async Task<IActionResult> GetCategorias()
        {
            var lista = await _db.Categorias.OrderBy(c => c.Nombre).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("{id:int}", Name= "GetCategoria")]
        [ProducesResponseType(200, Type = typeof(Categoria))]
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found, si no encuentra ningun objeto.
        public async Task<IActionResult> GetCategoria(int id)
        {
            var obj = await _db.Categorias.FirstOrDefaultAsync(c => c.Id == id);
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
        public async Task<IActionResult> CrearCategorias([FromBody] Categoria categoria )
        {
            
            if (categoria == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _db.AddAsync(categoria);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetCategoria", new { id = categoria.Id }, categoria);
        }

    }
}
