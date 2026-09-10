using TiendaApp.Api.Reglas;

namespace TiendaApp.Api.Models;

/// <summary>
/// Un ítem del carrito: un producto junto con la cantidad que el usuario quiere llevar.
/// Item no implementa las reglas de precio directamente (eso violaría el principio de
/// responsabilidad única): delega el cálculo en el ManejadorReglas, que aplica el patrón
/// Strategy para escoger la IReglaPrecio correcta según el SKU del producto.
/// </summary>
public class Item
{
    public Guid Id { get; } = Guid.NewGuid();
    public Producto Producto { get; }
    public decimal Cantidad { get; }

    private readonly ManejadorReglas _manejadorReglas;

    public Item(Producto producto, decimal cantidad, ManejadorReglas manejadorReglas)
    {
        Producto = producto;
        Cantidad = cantidad;
        _manejadorReglas = manejadorReglas;
    }

    /// <summary>Calcula el total de este ítem aplicando la regla de precio que corresponda al SKU.</summary>
    public decimal CalcularTotal()
    {
        IReglaPrecio regla = _manejadorReglas.ObtenerRegla(Producto.Sku);
        return regla.CalcularTotal(Cantidad, Producto.PrecioUnitario);
    }
}
