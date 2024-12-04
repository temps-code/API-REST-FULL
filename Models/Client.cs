using prueba1.Models;

namespace prueba1.Models
{
    public class Client
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Pass { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Sale> Sales { get; set;} 
    }
}
