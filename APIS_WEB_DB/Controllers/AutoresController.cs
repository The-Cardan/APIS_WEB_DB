using APIS_WEB_DB.Data;
using APIS_WEB_DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIS_WEB_DB.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public AutoresController(BibliotecaContext context)
        {
            _context = context;
        }

        // Obtener todos los autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
        {
            return await _context.Autores.ToListAsync();
        }

        // Obtener un autor por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetAutor(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        // Crear un nuevo autor
        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
            _context.Autores.Add(autor);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAutor),
                new { id = autor.Id },
                autor);
        }

        // Actualizar un autor existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest();
            }

            var existe = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            _context.Entry(autor).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Eliminar un autor existente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            _context.Autores.Remove(autor);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
