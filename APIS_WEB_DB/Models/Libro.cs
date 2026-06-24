namespace APIS_WEB_DB.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int AnioPublicacion { get; set; }
        public string Genero { get; set; }
        public int NumeroPaginas { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
    }
}