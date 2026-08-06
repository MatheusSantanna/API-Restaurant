export interface CreateOrderItemDto {
    productId: number;
    quantity: number;
}

export interface CreateOrderDto {
    tableId: number;
    items: CreateOrderItemDto[];
    orderDate: string; // ISO string, ex: new Date().toISOString()
}