using Microsoft.AspNetCore.Mvc;
using TiendaApp.Api.Services;

namespace TiendaApp.Api.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly ITiendaService _tiendaService;

    public ProductosController(ITiendaService tiendaService)
    {
        _tiendaService = tiendaService;
    }

    /// <summary>GET /api/productos — catálogo de productos disponibles.</summary>
    [HttpGet]
    public IActionResult ObtenerProductos()
    {
        return Ok(_tiendaService.ObtenerProductos());
    }
}
