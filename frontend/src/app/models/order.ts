
export interface Order {

  id: number;
  tableId: number;   // <-- adiciona esse
  nTable: number;
  orderDate: string;
  quantities: number[];
  orderStatus: OrderStatus;
  productNames: string[];
  unitPrice: number[];
  totalAmount: number;
}

export enum OrderStatus {
  Preparing = 0,
  Ready = 1,
  Finalized = 2
}
