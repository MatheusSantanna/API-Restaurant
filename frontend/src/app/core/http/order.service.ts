import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Order } from "../../models/order";
import { CreateOrderDto } from "../../models/DTO/createOrder.dto";

@Injectable({ providedIn: 'root' })
export class OrderService {
    private apiUrl = 'https://localhost:7283/api/order';


    constructor(private http: HttpClient) { }

    createOrder(dto: CreateOrderDto): Observable<any> {
        return this.http.post<Order>(this.apiUrl, dto);
    }

    getAll(): Observable<Order[]> {
        return this.http.get<Order[]>(this.apiUrl)
    }

    getById(id: number): Observable<Order> {
        return this.http.get<Order>(`${this.apiUrl}/${id}`);
    }

    closeOrder(order: number): Observable<void> {
        return this.http.patch<void>(`${this.apiUrl}/${order}/close`, {});
    }

    getOrdersDay(): Observable<Order[]> {
        return this.http.get<Order[]>(`${this.apiUrl}/today`);
    }
}