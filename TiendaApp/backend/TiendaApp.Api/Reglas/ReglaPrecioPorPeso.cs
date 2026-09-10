namespace TiendaApp.Api.Reglas;

/// <summary>
/// Productos con SKU que empieza en "WE" (weight): se venden por peso. El precio
/// unitario del producto está dado por gramo, pero la cantidad que ingresa el usuario
/// se expresa en kilogramos, así que se convierte antes de calcular el total.
/// </summary>
public class ReglaPrecioPorPeso : IReglaPrecio
{
    private const string PrefijoSku = "WE";
    private const decimal GramosPorKilogramo = 1000m;

    public bool EsAplicable(string sku) => sku.StartsWith(PrefijoSku, StringComparison.OrdinalIgnoreCase);

    public decimal CalcularTotal(decimal cantidadKg, decimal precioUnitarioPorGramo)
        => cantidadKg * GramosPorKilogramo * precioUnitarioPorGramo;
}
