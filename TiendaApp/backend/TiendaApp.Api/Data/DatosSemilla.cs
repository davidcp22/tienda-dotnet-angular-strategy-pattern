using TiendaApp.Api.Models;

namespace TiendaApp.Api.Data;

/// <summary>Carga un catálogo inicial de productos de ejemplo, uno por cada tipo (EA, WE, SP).</summary>
public static class DatosSemilla
{
    public static void Cargar(Tienda tienda)
    {
        tienda.AgregarProducto(new Producto("EA001", "Cepillo de dientes", "Cepillo de dientes suave", 100, 3500m));
        tienda.AgregarProducto(new Producto("EA002", "Cuaderno A5", "Cuaderno cuadriculado de 100 hojas", 200, 4800m));
        tienda.AgregarProducto(new Producto("WE001", "Café en grano", "Café en grano, precio por gramo", 50, 45m));
        tienda.AgregarProducto(new Producto("WE002", "Queso costeño", "Queso fresco, precio por gramo", 30, 22m));
        tienda.AgregarProducto(new Producto("SP001", "Jabón artesanal", "Jabón artesanal de lavanda", 60, 6000m));
        tienda.AgregarProducto(new Producto("SP002", "Vela aromática", "Vela de soya aromática", 40, 15000m));
    }
}
