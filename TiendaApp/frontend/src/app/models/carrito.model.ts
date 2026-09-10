export interface Item {
  id: string;
  sku: string;
  nombreProducto: string;
  cantidad: number;
  total: number;
}

export interface Carrito {
  items: Item[];
  total: number;
}

export interface Compra {
  totalCompra: number;
  totalVentasAcumulado: number;
}
