using prueba1.Models;

namespace prueba1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double Price { get; set; }
        public required int Stock { get; set; }
        public required string ImagePath { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<SaleDetail> SalesDetails { get; set;} 
        public ICollection<ProductFile> ProductFiles { get; set;} 
    }
}
