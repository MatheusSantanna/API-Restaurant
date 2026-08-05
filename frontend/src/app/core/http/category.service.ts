import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Category } from "../../models/category";

@Injectable({ providedIn: 'root'})
export class CategoryService {
    private apiUrl = 'http://localhost:5286/api/category';


    constructor(private http: HttpClient) {}

    getAll(): Observable<Category[]>{
        return this.http.get<Category[]>(this.apiUrl)
    }

    getById(id: number): Observable<Category>{
        return this.http.get<Category>(`${this.apiUrl}/${id}`);
    }
}