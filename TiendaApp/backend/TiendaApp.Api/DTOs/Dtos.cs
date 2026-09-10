namespace TiendaApp.Api.DTOs;

public record ProductoDto(string Sku, string Nombre, string Descripcion, int UnidadesDisponibles, decimal PrecioUnitario);

public record AgregarItemRequest(string Sku, decimal Cantidad);

public record ItemDto(Guid Id, string Sku, string NombreProducto, decimal Cantidad, decimal Total);

public record CarritoDto(IReadOnlyList<ItemDto> Items, decimal Total);

public record CompraDto(decimal TotalCompra, decimal TotalVentasAcumulado);
