import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Carrito } from '../../models/carrito.model';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent {
  @Input() carrito: Carrito | null = null;
  @Input() mensajeCompra: string | null = null;
  @Output() eliminarItem = new EventEmitter<string>();
  @Output() finalizarCompra = new EventEmitter<void>();
}
