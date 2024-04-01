import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {ProductModel} from "../models/product.model";

@Injectable({providedIn: 'root'})
export class ProductService {

    private readonly baseUrl: string;

    constructor(private httpClient: HttpClient) {
        this.baseUrl = `${environment.apiUrl}/product`;
    }

    list(day: any): Observable<ProductModel[]> {
        return this.httpClient.get<ProductModel[]>(`${this.baseUrl}/list/${day}`);
    }

    insert(product: ProductModel): Observable<any> {
        return this.httpClient.post(`${this.baseUrl}`, product, {responseType: 'text'});
    }

    update(product: ProductModel): Observable<any> {
        return this.httpClient.put(`${this.baseUrl}`, product);
    }

    delete(id: string): Observable<any> {
        return this.httpClient.delete(`${this.baseUrl}/${id}`);
    }
}
