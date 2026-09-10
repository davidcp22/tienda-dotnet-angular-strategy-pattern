using TiendaApp.Api.DTOs;
using TiendaApp.Api.Models;

namespace TiendaApp.Api.Services;

/// <summary>
/// Implementación de ITiendaService. Se registra como singleton para que el estado
/// en memoria (productos, usuarios, ventas) persista mientras la API esté corriendo.
/// </summary>
public class TiendaService : ITiendaService
{
    private readonly Tienda _tienda;

    public TiendaService(Tienda tienda)
    {
        _tienda = tienda;
    }

    public IEnumerable<ProductoDto> ObtenerProductos() =>
        _tienda.Productos.Select(MapProducto);

    public CarritoDto ObtenerCarrito(string usuarioId)
    {
        var usuario = _tienda.ObtenerOCrearUsuario(usuarioId, usuarioId);
        return MapCarrito(usuario.Carrito);
    }

    public CarritoDto AgregarItemACarrito(string usuarioId, AgregarItemRequest request)
    {
        var usuario = _tienda.ObtenerOCrearUsuario(usuarioId, usuarioId);
        var producto = _tienda.BuscarProducto(request.Sku)
            ?? throw new KeyNotFoundException($"No existe el producto con SKU '{request.Sku}'.");

        _tienda.AgregarProductoACarrito(usuario, producto, request.Cantidad);

        return MapCarrito(usuario.Carrito);
    }

    public CarritoDto EliminarItemDeCarrito(string usuarioId, Guid itemId)
    {
        var usuario = _tienda.ObtenerOCrearUsuario(usuarioId, usuarioId);
        _tienda.EliminarItemDeCarrito(usuario, itemId);
        return MapCarrito(usuario.Carrito);
    }

    public CompraDto FinalizarCompra(string usuarioId)
    {
        var usuario = _tienda.ObtenerOCrearUsuario(usuarioId, usuarioId);
        decimal totalCompra = _tienda.FinalizarCompra(usuario);
        return new CompraDto(totalCompra, _tienda.TotalVentas);
    }

    private static ProductoDto MapProducto(Producto p) =>
        new(p.Sku, p.Nombre, p.Descripcion, p.UnidadesDisponibles, p.PrecioUnitario);

    private static CarritoDto MapCarrito(Carrito carrito)
    {
        var items = carrito.Items
            .Select(i => new ItemDto(i.Id, i.Producto.Sku, i.Producto.Nombre, i.Cantidad, i.CalcularTotal()))
            .ToList();

        return new CarritoDto(items, carrito.CalcularTotal());
    }
}
