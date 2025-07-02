namespace BicTechBack.src.Core.DTOs
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public string CategoriaNombre { get; set; }
        public string MarcaNombre { get; set; }
        public int Stock { get; set; }
    }
}
