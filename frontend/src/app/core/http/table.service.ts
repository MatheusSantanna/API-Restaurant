import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Table } from "../../models/table";

@Injectable({ providedIn: 'root' })
export class TableService {
    private apiUrl = 'https://localhost:7283/api/table';


    constructor(private http: HttpClient) { }

    getAll(): Observable<Table[]> {
        return this.http.get<Table[]>(this.apiUrl)
    }

    getById(id: number): Observable<Table> {
        return this.http.get<Table>(`${this.apiUrl}/${id}`);
    }
}