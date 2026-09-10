namespace TiendaApp.Api.Reglas;

/// <summary>Productos con SKU que empieza en "EA": se cobra el precio unitario por la cantidad.</summary>
public class ReglaPrecioNormal : IReglaPrecio
{
    private const string PrefijoSku = "EA";

    public bool EsAplicable(string sku) => sku.StartsWith(PrefijoSku, StringComparison.OrdinalIgnoreCase);

    public decimal CalcularTotal(decimal cantidad, decimal precioUnitario) => cantidad * precioUnitario;
}
