using TiendaApp.Api.Reglas;

namespace TiendaApp.Api.Models;

/// <summary>
/// Raíz de agregación del dominio. Coordina productos y usuarios, valida disponibilidad
/// antes de agregar un ítem al carrito y concreta la compra (descuenta inventario y
/// acumula el total de ventas), igual que en el diagrama del OVA.
/// </summary>
public class Tienda
{
    public decimal TotalVentas { get; private set; }

    private readonly List<Producto> _productos = new();
    private readonly Dictionary<string, Usuario> _usuarios = new();
    private readonly ManejadorReglas _manejadorReglas;

    public Tienda(ManejadorReglas manejadorReglas)
    {
        _manejadorReglas = manejadorReglas;
    }

    public IReadOnlyList<Producto> Productos => _productos;

    public void AgregarProducto(Producto producto) => _productos.Add(producto);

    public Usuario ObtenerOCrearUsuario(string usuarioId, string nombre)
    {
        if (!_usuarios.TryGetValue(usuarioId, out var usuario))
        {
            usuario = new Usuario(usuarioId, nombre);
            _usuarios[usuarioId] = usuario;
        }
        return usuario;
    }

    public Producto? BuscarProducto(string sku) => _productos.FirstOrDefault(p => p.Sku == sku);

    /// <summary>
    /// Agrega un producto al carrito de un usuario, verificando primero que la tienda
    /// tenga unidades suficientes disponibles.
    /// </summary>
    public Item AgregarProductoACarrito(Usuario usuario, Producto producto, decimal cantidad)
    {
        if (!producto.TieneUnidades(cantidad))
            throw new InvalidOperationException($"No hay unidades suficientes de '{producto.Nombre}' ({producto.Sku}).");

        return usuario.AgregarItemACarrito(producto, cantidad, _manejadorReglas);
    }

    public bool EliminarItemDeCarrito(Usuario usuario, Guid itemId) => usuario.BorrarItemDeCarrito(itemId);

    /// <summary>
    /// Concreta la compra: descuenta las unidades vendidas de cada producto,
    /// acumula el valor de la venta en la tienda y vacía el carrito del usuario.
    /// </summary>
    public decimal FinalizarCompra(Usuario usuario)
    {
        decimal totalCompra = usuario.Carrito.CalcularTotal();

        foreach (var item in usuario.Carrito.Items)
        {
            item.Producto.DescontarUnidades(item.Cantidad);
        }

        TotalVentas += totalCompra;
        usuario.Carrito.Vaciar();

        return totalCompra;
    }
}
