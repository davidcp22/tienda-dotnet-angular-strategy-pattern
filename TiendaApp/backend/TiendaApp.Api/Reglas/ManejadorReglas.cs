namespace TiendaApp.Api.Reglas;

/// <summary>
/// Actúa como fábrica/registro de estrategias: recibe todas las IReglaPrecio conocidas
/// (inyectadas por el contenedor de DI) y, dado un SKU, devuelve la que corresponda.
/// Para agregar una nueva regla de precio (por ejemplo, un cuarto tipo de producto)
/// basta con crear una nueva clase que implemente IReglaPrecio y registrarla en
/// Program.cs; no hay que modificar ManejadorReglas, Item, Carrito ni Tienda.
/// </summary>
public class ManejadorReglas
{
    private readonly IReadOnlyList<IReglaPrecio> _reglas;

    public ManejadorReglas(IEnumerable<IReglaPrecio> reglas)
    {
        _reglas = reglas.ToList();

        if (_reglas.Count == 0)
            throw new InvalidOperationException("No hay reglas de precio registradas.");
    }

    public IReglaPrecio ObtenerRegla(string sku)
    {
        var regla = _reglas.FirstOrDefault(r => r.EsAplicable(sku));

        if (regla is null)
            throw new InvalidOperationException($"No existe una regla de precio para el SKU '{sku}'.");

        return regla;
    }
}
