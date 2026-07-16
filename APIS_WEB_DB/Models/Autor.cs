using System.ComponentModel.DataAnnotations;

namespace APIS_WEB_DB.Models
{
    public class Autor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Nacionalidad { get; set; } = string.Empty;

        [Range(1500, 2100)]
        public int AnioNacimiento { get; set; }
        public ICollection<Libro> Libros { get; set; } = new List<Libro>();

    }
}