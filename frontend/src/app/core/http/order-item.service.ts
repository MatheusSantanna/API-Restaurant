import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { OrderItemDto } from "../../models/order-item";

@Injectable({ providedIn: 'root' })
export class OrderItemService {
    private apiUrl = 'http://localhost:5286/api/orderItem';


    constructor(private http: HttpClient) { }

    create(item: OrderItemDto): Observable<any> {
        return this.http.post(this.apiUrl, item);
    }

    getAll(item: OrderItemDto): Observable<OrderItemDto[]> {
        return this.http.get<OrderItemDto[]>(this.apiUrl);
    }


}