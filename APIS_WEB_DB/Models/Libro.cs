using System.ComponentModel.DataAnnotations;

namespace APIS_WEB_DB.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Titulo { get; set; }

        [Range(1450, 2100)]
        public int AnioPublicacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Genero { get; set; }

        [Range(1, int.MaxValue)]
        public int NumeroPaginas { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; }

        [Range(1, int.MaxValue)]
        public int AutorId { get; set; }
    }
}