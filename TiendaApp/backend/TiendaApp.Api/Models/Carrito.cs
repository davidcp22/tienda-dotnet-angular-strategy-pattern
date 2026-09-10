namespace TiendaApp.Api.Models;

/// <summary>
/// Carrito de compras de un usuario. Mantiene la composición Carrito ◆— Item indicada
/// en el diagrama: los ítems no existen sin el carrito.
/// </summary>
public class Carrito
{
    private readonly List<Item> _items = new();
    public IReadOnlyList<Item> Items => _items;

    public void AgregarItem(Item item) => _items.Add(item);

    public bool BorrarItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item is null) return false;
        return _items.Remove(item);
    }

    public decimal CalcularTotal() => _items.Sum(i => i.CalcularTotal());

    public void Vaciar() => _items.Clear();
}
