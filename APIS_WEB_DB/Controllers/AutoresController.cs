using APIS_WEB_DB.Data;
using APIS_WEB_DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIS_WEB_DB.Security;

namespace APIS_WEB_DB.Controllers
{
    [Route("api/autores")]
    [ApiController]
    [ApiKey]
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

        // Obtener autores paginados con búsqueda y ordenamiento
        [HttpGet("paginado")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutoresPaginados(
            int pagina = 1,
            int tamanoPagina = 10,
            string? buscar = null,
            string? ordenarPor = "nombre",
            string? direccion = "asc")
        {
            var consulta = _context.Autores.AsQueryable();

            // Buscar por nombre
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(a => a.Nombre.Contains(buscar));
            }

            // Ordenamiento
            switch (ordenarPor?.ToLower())
            {
                case "anionacimiento":
                    consulta = direccion == "desc"
                        ? consulta.OrderByDescending(a => a.AnioNacimiento)
                        : consulta.OrderBy(a => a.AnioNacimiento);
                    break;

                default:
                    consulta = direccion == "desc"
                        ? consulta.OrderByDescending(a => a.Nombre)
                        : consulta.OrderBy(a => a.Nombre);
                    break;
            }

            var autores = await consulta
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            return Ok(autores);
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
