import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Order } from "../../models/order";

@Injectable({ providedIn: 'root' })
export class OrderService {
    private apiUrl = 'http://localhost:5286/api/order';


    constructor(private http: HttpClient) { }

    getAll(): Observable<Order[]> {
        return this.http.get<Order[]>(this.apiUrl)
    }

    getById(id: number): Observable<Order> {
        return this.http.get<Order>(`${this.apiUrl}/${id}`);
    }

    closeOrder(order: number): Observable<void> {
        return this.http.patch<void>(`${this.apiUrl}/${order}/close`, {});
    }

    getOrderDay(): Observable<Order> {
        return this.http.get<Order>(`${this.apiUrl}/today`);
    }
}