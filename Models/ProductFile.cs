using System.Text.Json.Serialization;
using prueba1.Models;

namespace prueba1.Models
{
    public class ProductFile
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? FileId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
