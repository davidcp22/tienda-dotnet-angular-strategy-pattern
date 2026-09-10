using TiendaApp.Api.Data;
using TiendaApp.Api.Models;
using TiendaApp.Api.Reglas;
using TiendaApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- Registro de estrategias de precio (patrón Strategy) ---
// Para agregar un nuevo tipo de producto/regla de precio, basta con crear una
// clase que implemente IReglaPrecio y registrarla aquí; ManejadorReglas la
// recibirá automáticamente vía IEnumerable<IReglaPrecio>, sin más cambios.
builder.Services.AddSingleton<IReglaPrecio, ReglaPrecioNormal>();
builder.Services.AddSingleton<IReglaPrecio, ReglaPrecioPorPeso>();
builder.Services.AddSingleton<IReglaPrecio, ReglaPrecioEspecial>();

builder.Services.AddSingleton<ManejadorReglas>();

builder.Services.AddSingleton<Tienda>(sp =>
{
    var tienda = new Tienda(sp.GetRequiredService<ManejadorReglas>());
    DatosSemilla.Cargar(tienda);
    return tienda;
});

builder.Services.AddSingleton<ITiendaService, TiendaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");
app.UseAuthorization();
app.MapControllers();

app.Run();
