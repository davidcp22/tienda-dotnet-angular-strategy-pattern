using Microsoft.AspNetCore.Mvc;
using TiendaApp.Api.DTOs;
using TiendaApp.Api.Services;

namespace TiendaApp.Api.Controllers;

/// <summary>
/// Todas las operaciones de carrito se hacen "por usuario" (usuarioId en la ruta),
/// ya que en el dominio cada Usuario tiene su propio Carrito.
/// </summary>
[ApiController]
[Route("api/usuarios/{usuarioId}/carrito")]
public class CarritoController : ControllerBase
{
    private readonly ITiendaService _tiendaService;

    public CarritoController(ITiendaService tiendaService)
    {
        _tiendaService = tiendaService;
    }

    /// <summary>GET /api/usuarios/{usuarioId}/carrito</summary>
    [HttpGet]
    public IActionResult ObtenerCarrito(string usuarioId)
    {
        return Ok(_tiendaService.ObtenerCarrito(usuarioId));
    }

    /// <summary>POST /api/usuarios/{usuarioId}/carrito/items — agrega un producto al carrito.</summary>
    [HttpPost("items")]
    public IActionResult AgregarItem(string usuarioId, [FromBody] AgregarItemRequest request)
    {
        try
        {
            return Ok(_tiendaService.AgregarItemACarrito(usuarioId, request));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>DELETE /api/usuarios/{usuarioId}/carrito/items/{itemId}</summary>
    [HttpDelete("items/{itemId:guid}")]
    public IActionResult EliminarItem(string usuarioId, Guid itemId)
    {
        return Ok(_tiendaService.EliminarItemDeCarrito(usuarioId, itemId));
    }

    /// <summary>POST /api/usuarios/{usuarioId}/carrito/finalizar — concreta la compra.</summary>
    [HttpPost("finalizar")]
    public IActionResult FinalizarCompra(string usuarioId)
    {
        try
        {
            return Ok(_tiendaService.FinalizarCompra(usuarioId));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
