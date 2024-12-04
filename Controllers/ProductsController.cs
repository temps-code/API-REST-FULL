using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prueba1.Dto.Products;
using prueba1.Mappers.Products;
using prueba1.Models;

namespace prueba1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }


        [HttpGet("ActiveProductsList")]
        public async Task<ActionResult<IEnumerable<Product>>> GetActiveProducts()
        {
            var products = await _context.Products.Where(c => c.IsActive).ToListAsync();
            return Ok(products);
        }

        [HttpGet("InactiveProductsList")]
        public async Task<ActionResult<IEnumerable<Product>>> GetInactiveProducts()
        {
            var products = await _context.Products
                .Where(c => !c.IsActive)
                .ToListAsync();
            return Ok(products);
        }

        [HttpGet("ArticulosActivos")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetActiveProductsDto()
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .Select(p => p.toProductDto())
                .ToListAsync();
            return Ok(products);
        }

        [HttpGet("ArticulosInactivos")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetInactiveProductsDto()
        {
            var products = await _context.Products
                .Where(p => !p.IsActive)
                .Select(p => p.toProductDto())
                .ToListAsync();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Producto no encontrado.");

            return Ok(product);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<Product>> GetProductByName(string name)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == name);

            if (product == null)
                return NotFound("Producto no encontrado.");

            return Ok(product);
        }


        [HttpPost("crear/name/{name}/description/{description}/price/{price}/stock/{stock}/imagepath/{imagePath}")]
        public async Task<ActionResult<Product>> CreateProduct([FromRoute] string name, [FromRoute] string description, [FromRoute] double price, [FromRoute] int stock, [FromRoute] string imagePath)
        {
            // Decodificar el imagePath
            string decodedImagePath = Uri.UnescapeDataString(imagePath);

            // Verificar si ya existe un producto con el mismo nombre
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == name);

            if (existingProduct != null)
            {
                // Si el producto ya existe, devolver un error con mensaje adecuado
                return Conflict("Ya existe un producto con el mismo nombre.");
            }

            // Crear el nuevo producto
            Product product = new()
            {
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                ImagePath = decodedImagePath,  // Usar el valor decodificado
                IsActive = true
            };

            // Agregar el producto a la base de datos
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Devolver el producto creado con el código de respuesta 201
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, string name, string description, string imagePath, double price, bool isActive)
        {
            // Verificar si ya existe un producto con el mismo nombre (y que no sea el mismo producto)
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == name && p.Id != id);

            if (existingProduct != null)
            {
                // Si el producto ya existe, devolver un error con mensaje adecuado
                return Conflict("Ya existe un producto con el mismo nombre.");
            }

            // Buscar el producto por ID
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Producto no encontrado.");

            // Actualizar las propiedades del producto solo si hay cambios
            var updatedFields = new List<string>(); // Lista para almacenar los campos actualizados

            if (product.Name != name)
            {
                product.Name = name;
                updatedFields.Add("Name");
            }

            if (product.Description != description)
            {
                product.Description = description;
                updatedFields.Add("Description");
            }

            if (product.ImagePath != imagePath)
            {
                product.ImagePath = imagePath;
                updatedFields.Add("ImagePath");
            }

            if (product.Price != price)
            {
                product.Price = price;
                updatedFields.Add("Price");
            }

            if (product.IsActive != isActive)
            {
                product.IsActive = isActive;
                updatedFields.Add("IsActive");
            }

            // Si no hay cambios, devolver un BadRequest
            if (updatedFields.Count == 0)
            {
                return BadRequest("No se proporcionaron cambios para actualizar.");
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devolver el producto actualizado con los campos modificados
            return Ok(new
            {
                message = "Producto actualizado correctamente.",
                updatedFields,
                product
            });
        }
    
        [HttpPut("name/{name}")]
        public async Task<IActionResult> UpdateProductByName(string name, string newName, string description, string imagePath, double price, bool isActive)
        {
            // Verificar si ya existe un producto con el mismo nombre (y que no sea el mismo producto)
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == newName && p.Name != name);

            if (existingProduct != null)
            {
                // Si el producto ya existe, devolver un error con mensaje adecuado
                return Conflict("Ya existe un producto con el mismo nombre.");
            }

            // Buscar el producto por el nombre original
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == name);

            if (product == null)
                return NotFound("Producto no encontrado.");

            // Lista para almacenar los campos actualizados
            var updatedFields = new List<string>();

            // Actualizar las propiedades solo si hay cambios
            if (product.Name != newName)
            {
                product.Name = newName;
                updatedFields.Add("Name");
            }

            if (product.Description != description)
            {
                product.Description = description;
                updatedFields.Add("Description");
            }

            if (product.ImagePath != imagePath)
            {
                product.ImagePath = imagePath;
                updatedFields.Add("ImagePath");
            }

            if (product.Price != price)
            {
                product.Price = price;
                updatedFields.Add("Price");
            }

            if (product.IsActive != isActive)
            {
                product.IsActive = isActive;
                updatedFields.Add("IsActive");
            }

            // Si no hay cambios, devolver un BadRequest
            if (updatedFields.Count == 0)
            {
                return BadRequest("No se proporcionaron cambios para actualizar.");
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devolver el producto actualizado con los campos modificados
            return Ok(new
            {
                message = "Producto actualizado correctamente.",
                updatedFields = updatedFields,
                product = product
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Producto no encontrado.");

            if (!product.IsActive)
                return Ok(new { message = $"El producto '{product.Name}' ya está desactivado." });

            product.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"El producto '{product.Name}' ha sido desactivado correctamente." });
        }


        [HttpDelete("name/{name}")]
        public async Task<IActionResult> DeleteProductByName(string name)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == name);

            if (product == null)
                return NotFound("Producto no encontrado.");

            if (!product.IsActive)
                return Ok(new { message = $"El producto '{name}' ya está desactivado." });

            product.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"El producto '{name}' ha sido desactivado correctamente." });
        }

    }
}