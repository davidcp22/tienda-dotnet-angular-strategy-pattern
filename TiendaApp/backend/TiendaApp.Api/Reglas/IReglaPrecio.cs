namespace TiendaApp.Api.Reglas;

/// <summary>
/// Patrón Strategy: cada regla de precio encapsula un algoritmo distinto para calcular
/// el total de un ítem. Esto permite agregar o modificar reglas sin tocar Item, Carrito
/// ni Tienda (principio abierto/cerrado), que es exactamente lo que exige el caso de
/// estudio: "las reglas... son susceptibles de cambios... de forma desacoplada".
/// </summary>
public interface IReglaPrecio
{
    /// <summary>Indica si esta regla aplica para el SKU dado (según su prefijo).</summary>
    bool EsAplicable(string sku);

    /// <summary>Calcula el total a cobrar por "cantidad" unidades a "precioUnitario".</summary>
    decimal CalcularTotal(decimal cantidad, decimal precioUnitario);
}
