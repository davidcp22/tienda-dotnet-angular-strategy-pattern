import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Producto } from '../../models/producto.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent {
  @Input() productos: Producto[] = [];
  @Output() agregarAlCarrito = new EventEmitter<{ sku: string; cantidad: number }>();

  cantidades: Record<string, number> = {};

  cantidadPara(sku: string): number {
    return this.cantidades[sku] ?? 1;
  }

  actualizarCantidad(sku: string, valor: number): void {
    this.cantidades[sku] = valor;
  }

  agregar(producto: Producto): void {
    const cantidad = this.cantidadPara(producto.sku);
    if (cantidad <= 0) return;
    this.agregarAlCarrito.emit({ sku: producto.sku, cantidad });
  }

  tipoProducto(sku: string): string {
    if (sku.startsWith('EA')) return 'Normal';
    if (sku.startsWith('WE')) return 'Por peso (kg)';
    if (sku.startsWith('SP')) return 'Descuento especial';
    return 'Desconocido';
  }
}
