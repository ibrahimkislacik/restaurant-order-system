import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {CategoryModel} from "../models/category.model";
import {Injectable} from "@angular/core";

@Injectable({providedIn: 'root'})
export class CategoryService {

    private readonly baseUrl: string;

    constructor(
        private httpClient: HttpClient
    ) {
        this.baseUrl = `${environment.apiUrl}/category`;
    }

    list(): Observable<CategoryModel[]> {
        return this.httpClient.get<CategoryModel[]>(`${this.baseUrl}/list`);
    }

    insert(category: CategoryModel): Observable<any> {
        return this.httpClient.post(`${this.baseUrl}`, category, {responseType: 'text'});
    }

    update(category: CategoryModel): Observable<any> {
        return this.httpClient.put(`${this.baseUrl}`, category);
    }

    delete(id: string): Observable<any> {
        return this.httpClient.delete(`${this.baseUrl}/${id}`);
    }

}
