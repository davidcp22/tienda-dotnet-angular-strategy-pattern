import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Producto } from '../models/producto.model';
import { Carrito, Compra } from '../models/carrito.model';

/**
 * Encapsula toda la comunicación HTTP con el backend .NET. Los componentes
 * nunca llaman a HttpClient directamente, sino a este servicio, lo cual permite
 * cambiar la fuente de datos sin tocar la UI.
 */
@Injectable({ providedIn: 'root' })
export class TiendaService {
  private readonly baseUrl = environment.apiUrl;
  private readonly usuarioId = 'demo-user';

  constructor(private http: HttpClient) {}

  obtenerProductos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(`${this.baseUrl}/productos`);
  }

  obtenerCarrito(): Observable<Carrito> {
    return this.http.get<Carrito>(`${this.baseUrl}/usuarios/${this.usuarioId}/carrito`);
  }

  agregarItem(sku: string, cantidad: number): Observable<Carrito> {
    return this.http.post<Carrito>(`${this.baseUrl}/usuarios/${this.usuarioId}/carrito/items`, {
      sku,
      cantidad,
    });
  }

  eliminarItem(itemId: string): Observable<Carrito> {
    return this.http.delete<Carrito>(
      `${this.baseUrl}/usuarios/${this.usuarioId}/carrito/items/${itemId}`
    );
  }

  finalizarCompra(): Observable<Compra> {
    return this.http.post<Compra>(
      `${this.baseUrl}/usuarios/${this.usuarioId}/carrito/finalizar`,
      {}
    );
  }
}
