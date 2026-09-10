import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListComponent } from './components/product-list/product-list.component';
import { CartComponent } from './components/cart/cart.component';
import { TiendaService } from './services/tienda.service';
import { Producto } from './models/producto.model';
import { Carrito } from './models/carrito.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, ProductListComponent, CartComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  productos: Producto[] = [];
  carrito: Carrito | null = null;
  error: string | null = null;
  mensajeCompra: string | null = null;

  constructor(private tiendaService: TiendaService) {}

  ngOnInit(): void {
    this.cargarProductos();
    this.cargarCarrito();
  }

  cargarProductos(): void {
    this.tiendaService.obtenerProductos().subscribe({
      next: (productos) => (this.productos = productos),
      error: () => (this.error = 'No se pudo cargar el catálogo de productos.'),
    });
  }

  cargarCarrito(): void {
    this.tiendaService.obtenerCarrito().subscribe({
      next: (carrito) => (this.carrito = carrito),
      error: () => (this.error = 'No se pudo cargar el carrito.'),
    });
  }

  onAgregarAlCarrito(evento: { sku: string; cantidad: number }): void {
    this.error = null;
    this.mensajeCompra = null;
    this.tiendaService.agregarItem(evento.sku, evento.cantidad).subscribe({
      next: (carrito) => {
        this.carrito = carrito;
        this.cargarProductos(); // refresca inventario disponible
      },
      error: (err) => (this.error = err?.error?.mensaje ?? 'No se pudo agregar el producto.'),
    });
  }

  onEliminarItem(itemId: string): void {
    this.tiendaService.eliminarItem(itemId).subscribe({
      next: (carrito) => (this.carrito = carrito),
      error: () => (this.error = 'No se pudo eliminar el ítem.'),
    });
  }

  onFinalizarCompra(): void {
    this.error = null;
    this.tiendaService.finalizarCompra().subscribe({
      next: (compra) => {
        this.mensajeCompra = `Compra realizada por $${compra.totalCompra.toFixed(2)}. Total acumulado en ventas: $${compra.totalVentasAcumulado.toFixed(2)}.`;
        this.cargarCarrito();
        this.cargarProductos();
      },
      error: (err) => (this.error = err?.error?.mensaje ?? 'No se pudo finalizar la compra.'),
    });
  }
}
