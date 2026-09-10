namespace TiendaApp.Api.Models;

/// <summary>
/// Representa un producto de la tienda. El tipo de producto (normal, por peso,
/// descuento especial) no se modela como una propiedad explícita: se infiere a partir
/// del prefijo del SKU (EA, WE, SP), tal como lo define el caso de estudio.
/// La regla de precio asociada a ese tipo la resuelve el ManejadorReglas (Strategy),
/// por lo que Producto no necesita saber cómo se calcula su propio precio de venta.
/// </summary>
public class Producto
{
    public string Sku { get; }
    public string Nombre { get; }
    public string Descripcion { get; }
    public int UnidadesDisponibles { get; private set; }
    public decimal PrecioUnitario { get; }

    public Producto(string sku, string nombre, string descripcion, int unidadesDisponibles, decimal precioUnitario)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("El SKU del producto es obligatorio.", nameof(sku));

        Sku = sku;
        Nombre = nombre;
        Descripcion = descripcion;
        UnidadesDisponibles = unidadesDisponibles;
        PrecioUnitario = precioUnitario;
    }

    /// <summary>Indica si la tienda tiene disponibilidad suficiente para vender "cantidad" unidades.</summary>
    public bool TieneUnidades(decimal cantidad) => UnidadesDisponibles >= cantidad;

    /// <summary>Descuenta unidades del inventario al concretar una compra.</summary>
    public void DescontarUnidades(decimal cantidad)
    {
        if (!TieneUnidades(cantidad))
            throw new InvalidOperationException($"No hay unidades suficientes de {Sku} para descontar {cantidad}.");

        UnidadesDisponibles -= (int)cantidad;
    }
}
