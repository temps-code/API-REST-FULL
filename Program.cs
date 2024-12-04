using Microsoft.EntityFrameworkCore;
using prueba1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure MySQL context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))));

// Configure MongoDbContext
builder.Services.AddSingleton<MongoDbContext>();

// Add controllers to handle requests
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("myApp", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://127.0.0.1:5510") // Origen permitido
                      .AllowAnyHeader()   // Permite cualquier cabecera
                      .AllowAnyMethod()   // Permite cualquier método (GET, POST, etc.)
                      .AllowCredentials(); // Permite credenciales si es necesario (cookies, autenticación)
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("myApp");  // Aplica la política "myApp"

// Map controllers
app.MapControllers();

app.Run();
