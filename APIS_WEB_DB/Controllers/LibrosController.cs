using APIS_WEB_DB.Data;
using APIS_WEB_DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIS_WEB_DB.Controllers
{
    [Route("api/libros")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }


        //Optener todos los libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
        {
            return await _context.Libros.ToListAsync();
        }

        // Obtener libros paginados con búsqueda y ordenamiento
        [HttpGet("paginado")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPaginados(
            int pagina = 1,
            int tamanoPagina = 10,
            string? buscar = null,
            string? ordenarPor = "titulo",
            string? direccion = "asc")
        {
            var consulta = _context.Libros.AsQueryable();

            // Buscar por título
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(l => l.Titulo.Contains(buscar));
            }

            // Ordenamiento
            switch (ordenarPor?.ToLower())
            {
                case "precio":
                    consulta = direccion == "desc"
                        ? consulta.OrderByDescending(l => l.Precio)
                        : consulta.OrderBy(l => l.Precio);
                    break;

                case "aniopublicacion":
                    consulta = direccion == "desc"
                        ? consulta.OrderByDescending(l => l.AnioPublicacion)
                        : consulta.OrderBy(l => l.AnioPublicacion);
                    break;

                case "numeropaginas":
                    consulta = direccion == "desc"
                        ? consulta.OrderByDescending(l => l.NumeroPaginas)
                        : consulta.OrderBy(l => l.NumeroPaginas);
                    break;

                default:
                    consulta = direccion == "desc"
                        ? consulta.OrderByDescending(l => l.Titulo)
                        : consulta.OrderBy(l => l.Titulo);
                    break;
            }

            var libros = await consulta
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            return Ok(libros);
        }

        //Optener un libro por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }


        //Crear un nuevo libro-Post
        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(Libro libro)
        {
            _context.Libros.Add(libro);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetLibro),
                new { id = libro.Id },
                libro);
        }

        //Actualizar un libro existente-Put
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }

            var existe = await _context.Libros.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            _context.Entry(libro).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Eliminar un libro existente-Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Obtener todos los libros de un autor
        [HttpGet("{id}/libros")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorAutor(int id)
        {
            var autor = await _context.Autores
                .Include(a => a.Libros)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor.Libros);
        }

    }
}
