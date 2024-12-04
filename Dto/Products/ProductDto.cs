using System.Text.Json.Serialization;
using prueba1.Models;

namespace prueba1.Dto.Products
{
    public class ProductDto
    {
        public int id { get; set; }
        public required string article { get; set; }
        public required string description { get; set; }
        public required string img { get; set; }
        public required double price { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; } = true;
    }
}
