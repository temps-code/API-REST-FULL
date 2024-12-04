using System.Text.Json.Serialization;
using prueba1.Models;

namespace prueba1.Models
{
    public class File
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Size { get; set; }
        public string WebPath { get; set; }
        public string SystemPath { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<ProductFile> FilesProduct { get; set;} 
    }
}
