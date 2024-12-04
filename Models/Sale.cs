using System.Text.Json.Serialization;
using prueba1.Models;

namespace prueba1.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int? ClienteId{get;set;}
        public required string Date { get; set; }
        [JsonIgnore]
        public Client? Client { get; set;}
        public bool IsActive { get; set; } = true;
        public ICollection<SaleDetail> SalesDetails { get; set;} 
    }
}
