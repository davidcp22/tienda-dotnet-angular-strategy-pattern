namespace TiendaApp.Api.Reglas;

/// <summary>
/// Productos con SKU que empieza en "SP" (special): descuento del 20% por cada 3
/// unidades compradas, hasta un descuento máximo del 50%.
/// Ejemplo: 3 unidades -> 20% dto. 6 unidades -> 40% dto. 9 o más -> tope de 50%.
/// </summary>
public class ReglaPrecioEspecial : IReglaPrecio
{
    private const string PrefijoSku = "SP";
    private const decimal DescuentoPorGrupo = 0.20m;
    private const decimal DescuentoMaximo = 0.50m;
    private const int UnidadesPorGrupo = 3;

    public bool EsAplicable(string sku) => sku.StartsWith(PrefijoSku, StringComparison.OrdinalIgnoreCase);

    public decimal CalcularTotal(decimal cantidad, decimal precioUnitario)
    {
        int gruposCompletos = (int)(cantidad / UnidadesPorGrupo);
        decimal descuento = Math.Min(gruposCompletos * DescuentoPorGrupo, DescuentoMaximo);

        decimal subtotal = cantidad * precioUnitario;
        return subtotal * (1 - descuento);
    }
}
