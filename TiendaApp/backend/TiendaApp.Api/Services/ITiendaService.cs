using TiendaApp.Api.DTOs;

namespace TiendaApp.Api.Services;

/// <summary>
/// Fachada de aplicación entre los Controllers (HTTP) y el modelo de dominio.
/// Los controllers nunca hablan directamente con Tienda/Usuario/Carrito: así el
/// diseño orientado a objetos queda igual de válido sin importar el mecanismo de
/// transporte (REST hoy, podría ser gRPC o CLI mañana sin tocar el dominio).
/// </summary>
public interface ITiendaService
{
    IEnumerable<ProductoDto> ObtenerProductos();
    CarritoDto ObtenerCarrito(string usuarioId);
    CarritoDto AgregarItemACarrito(string usuarioId, AgregarItemRequest request);
    CarritoDto EliminarItemDeCarrito(string usuarioId, Guid itemId);
    CompraDto FinalizarCompra(string usuarioId);
}
