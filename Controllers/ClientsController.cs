using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prueba1.Models;

namespace prueba1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearCliente(string name, string email, string pass)
        {
            var client = new Client { Name = name, Email = email, Pass = pass, IsActive = true };  // Por defecto, el cliente está activo
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return Created("", client);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> VerCliente(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (client == null)
            {
                return NoContent();
            }
            return Ok(client);
        }

        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<Client>>> ListaClientes()
        {
            var clients = await _context.Clients.Where(c => c.IsActive).ToListAsync();
            if (clients == null || !clients.Any())
                return NoContent();
            return Ok(clients);
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarCliente(int id, string name, string email, string pass)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (client == null)
                return NoContent();

            client.Name = name;
            client.Email = email;
            client.Pass = pass;
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("borrar/{id}")]
        public async Task<IActionResult> BorrarCliente(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (client == null)
                return NoContent();

            // Marcar como inactivo en lugar de eliminar
            client.IsActive = false;
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorNombre(string name)
        {
            var clients = await _context.Clients
                .Where(c => c.Name.Contains(name) && c.IsActive)
                .ToListAsync();

            if (clients == null || !clients.Any())
                return NoContent();

            return Ok(clients);
        }
    }
}
