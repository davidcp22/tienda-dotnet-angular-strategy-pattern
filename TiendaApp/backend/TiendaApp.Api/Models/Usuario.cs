using TiendaApp.Api.Reglas;

namespace TiendaApp.Api.Models;

/// <summary>
/// Usuario de la tienda. Cada usuario tiene su propio carrito de compras (1 a 1),
/// tal como lo muestra el diagrama (Tienda 1—* Usuario, Usuario 1—1 Carrito).
/// </summary>
public class Usuario
{
    public string Id { get; }
    public string Nombre { get; }
    public Carrito Carrito { get; } = new();

    public Usuario(string id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }

    public Item AgregarItemACarrito(Producto producto, decimal cantidad, ManejadorReglas manejadorReglas)
    {
        var item = new Item(producto, cantidad, manejadorReglas);
        Carrito.AgregarItem(item);
        return item;
    }

    public bool BorrarItemDeCarrito(Guid itemId) => Carrito.BorrarItem(itemId);
}
