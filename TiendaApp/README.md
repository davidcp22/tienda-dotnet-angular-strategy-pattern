# Tienda App — Diseño e implementación

Implementación web (backend **.NET 8 Web API** + frontend **Angular**) del caso de
estudio de la tienda de comercio, manteniendo el diseño orientado a objetos planteado
en el OVA (Strategy para las reglas de precio, con inyección de dependencias como
mecanismo de "fábrica/registro").

## Estructura del repositorio

```
TiendaApp/
├── backend/                     .NET 8 Web API
│   └── TiendaApp.Api/
│       ├── Models/              Producto, Item, Carrito, Usuario, Tienda
│       ├── Reglas/               IReglaPrecio + 3 estrategias + ManejadorReglas
│       ├── Services/            ITiendaService / TiendaService (fachada de aplicación)
│       ├── DTOs/                 Contratos de la API
│       ├── Controllers/          ProductosController, CarritoController
│       └── Data/                 Datos de ejemplo (seed)
└── frontend/                    Angular (standalone components)
    └── src/app/
        ├── models/
        ├── services/tienda.service.ts
        └── components/product-list, cart
```

## Patrones y principios aplicados

- **Strategy** — `IReglaPrecio` define el algoritmo de cálculo de precio; cada tipo de
  producto (`ReglaPrecioNormal`, `ReglaPrecioPorPeso`, `ReglaPrecioEspecial`) es una
  estrategia intercambiable. `Item` y `Carrito` no conocen las reglas concretas.
- **Factory / Registry vía Inyección de Dependencias** — `ManejadorReglas` recibe todas
  las `IReglaPrecio` registradas (`IEnumerable<IReglaPrecio>`) y resuelve cuál aplica
  según el prefijo del SKU. Agregar una nueva regla = crear la clase + una línea en
  `Program.cs`; no se toca nada más (principio abierto/cerrado, que es justo lo que pide
  el caso de estudio).
- **Fachada / capa de aplicación** — `ITiendaService` desacopla los `Controllers` (HTTP)
  del modelo de dominio (`Tienda`, `Usuario`, `Carrito`), para que el diseño OO sea
  independiente del mecanismo de transporte.
- **Inversión de dependencias** — el dominio depende de la abstracción `IReglaPrecio`,
  nunca de las clases concretas.
- **Responsabilidad única** — `Producto` sólo gestiona su propio inventario
  (`TieneUnidades`/`DescontarUnidades`); el cálculo de precio vive en las estrategias;
  la orquestación de la compra vive en `Tienda`.

## Decisiones de implementación

- **Persistencia**: en memoria (singleton `Tienda` inyectado por DI), para mantener el
  foco del ejercicio en el diseño OO, tal como lo aclara el enunciado ("no representa
  una aplicación real"). Si se requiere persistencia real, `TiendaService` es el único
  punto que debería cambiar para hablar con una base de datos en lugar del objeto
  `Tienda` en memoria — el dominio no se vería afectado.
- **Identificación de usuario**: por simplicidad el frontend usa un `usuarioId` fijo
  (`demo-user`); no se implementó autenticación real porque no forma parte del caso de
  estudio.
- **Cantidad de productos "por peso"**: se asume que el usuario ingresa la cantidad en
  kilogramos y que `precioUnitario` está en pesos por gramo (como indica el enunciado),
  por lo que la conversión ocurre dentro de `ReglaPrecioPorPeso`.

## Cómo ejecutar

### Backend (requiere .NET 8 SDK)

```bash
cd backend/TiendaApp.Api
dotnet restore
dotnet run
```

La API queda en `http://localhost:5080` (Swagger en `http://localhost:5080/swagger`).

### Frontend (requiere Node.js 18+ y Angular CLI)

```bash
cd frontend
npm install
npm start
```

La app queda en `http://localhost:4200` y consume la API en `http://localhost:5080/api`
(configurable en `src/environments/environment.ts`).

## Endpoints principales

| Método | Ruta                                              | Descripción                     |
|--------|---------------------------------------------------|----------------------------------|
| GET    | `/api/productos`                                   | Catálogo de productos           |
| GET    | `/api/usuarios/{usuarioId}/carrito`                 | Ver carrito                     |
| POST   | `/api/usuarios/{usuarioId}/carrito/items`           | Agregar `{ sku, cantidad }`     |
| DELETE | `/api/usuarios/{usuarioId}/carrito/items/{itemId}`  | Quitar ítem                     |
| POST   | `/api/usuarios/{usuarioId}/carrito/finalizar`       | Concretar la compra             |

## Para el video de entrega

Puntos sugeridos a cubrir (según la rúbrica del enunciado):

1. Repaso rápido del diagrama de clases del OVA y cómo se mapea a las carpetas
   `Models/` y `Reglas/`.
2. Demo en vivo: agregar productos de los 3 tipos al carrito y mostrar cómo cambia el
   cálculo (normal, por peso, descuento escalonado).
3. Mostrar cómo se agregaría una **cuarta regla de precio** (ej. "2x1") sin modificar
   `Item`, `Carrito`, `Tienda` ni `ManejadorReglas` — solo una clase nueva + una línea en
   `Program.cs`. Esa es la prueba de que el diseño OO cumplió su propósito.
4. Conclusión personal: ¿el diseño OO valió la pena en una app web moderna (REST +
   SPA), donde el "transporte" no es orientado a objetos? Ventajas y costos observados.
