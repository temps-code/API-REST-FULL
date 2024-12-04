using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using prueba1.Models;

namespace prueba1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public UsersController(MongoDbContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.Find(_ => true).ToListAsync();
            return Ok(users);
        }

        // Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound("Usuario no encontrado.");

            return Ok(user);
        }

        // Crear un nuevo usuario
        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            // Verificar si ya existe un usuario con el mismo nombre
            var existingUser = await _context.Users.Find(u => u.Name == user.Name).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return Conflict("Ya existe un usuario con el mismo nombre.");
            }

            await _context.Users.InsertOneAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Actualizar un usuario existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User updatedUser)
        {
            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound("Usuario no encontrado.");

            // Lista para almacenar los campos que se actualizaron
            var updatedFields = new List<string>();

            // Crear un objeto de definición de actualización
            var updateDefinitions = new List<UpdateDefinition<User>>();

            // Solo actualizamos los campos si hay cambios
            if (!string.IsNullOrEmpty(updatedUser.Name) && updatedUser.Name != user.Name)
            {
                updateDefinitions.Add(Builders<User>.Update.Set(u => u.Name, updatedUser.Name));
                updatedFields.Add("Name");
            }

            if (!string.IsNullOrEmpty(updatedUser.Email) && updatedUser.Email != user.Email)
            {
                updateDefinitions.Add(Builders<User>.Update.Set(u => u.Email, updatedUser.Email));
                updatedFields.Add("Email");
            }

            if (updatedUser.IsActive != user.IsActive)
            {
                updateDefinitions.Add(Builders<User>.Update.Set(u => u.IsActive, updatedUser.IsActive));
                updatedFields.Add("IsActive");
            }

            // Si no se actualizaron campos, devolvemos un BadRequest
            if (updatedFields.Count == 0)
            {
                return BadRequest("No se proporcionaron cambios para actualizar.");
            }

            // Combinar todas las definiciones de actualización
            var combinedUpdate = Builders<User>.Update.Combine(updateDefinitions);

            // Actualizar el usuario en la base de datos
            await _context.Users.UpdateOneAsync(u => u.Id == id, combinedUpdate);

            // Retornar un mensaje con los campos actualizados
            return Ok(new
            {
                message = "Usuario actualizado correctamente.",
                updatedFields
            });
        }



        // Desactivar un usuario (no eliminarlo)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
                return NotFound("Usuario no encontrado.");

            // Si el usuario ya está inactivo, retornar mensaje de éxito
            if (!user.IsActive)
                return Ok(new { message = $"El usuario con ID '{id}' ya está desactivado." });

            // Cambiar el estado de IsActive a false para desactivar el usuario
            var updateDefinition = Builders<User>.Update.Set(u => u.IsActive, false);

            // Realizar la actualización en la base de datos
            await _context.Users.UpdateOneAsync(u => u.Id == id, updateDefinition);

            return Ok(new { message = $"El usuario con ID '{id}' ha sido desactivado correctamente." });
        }

    }
}
