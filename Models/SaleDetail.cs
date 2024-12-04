using System.Text.Json.Serialization;
using prueba1.Models;

namespace prueba1.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int? SaleId{get;set;}
        public int? ProductID{get;set;}
        public int Amount { get; set; }
        public required double Price { get; set; }
        public double SubTotal { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
