import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {CategoryModel} from "../models/category.model";
import {Injectable} from "@angular/core";
import {OrderModel} from "../models/order.model";

@Injectable({providedIn: 'root'})
export class OrderService {

    private readonly baseUrl: string;

    constructor(
        private httpClient: HttpClient
    ) {
        this.baseUrl = `${environment.apiUrl}/order`;
    }

    list(): Observable<OrderModel[]> {
        return this.httpClient.get<OrderModel[]>(`${this.baseUrl}/list`);
    }

    get(id: string): Observable<OrderModel> {
        return this.httpClient.get<OrderModel>(`${this.baseUrl}/${id}`);
    }

    insert(category: OrderModel): Observable<any> {
        return this.httpClient.post(`${this.baseUrl}`, category, {responseType: 'text'});
    }


}
