import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {ProductModel} from "../models/product.model";
import {OrderModel} from "../models/order.model";
import {OrderInsertRequestModel} from "../models/order-insert-request.model";

@Injectable({providedIn: 'root'})
export class CustomerService {

    private readonly baseUrl: string;

    constructor(private httpClient: HttpClient) {
        this.baseUrl = `${environment.apiUrl}`;
    }

    productList(): Observable<ProductModel[]> {
        return this.httpClient.get<ProductModel[]>(`${this.baseUrl}/product/list/${new Date().getDay()}`);
    }

    insertOrder(orderInsertRequestModel: OrderInsertRequestModel): Observable<any> {
        return this.httpClient.post(`${this.baseUrl}/order`, orderInsertRequestModel, {responseType: 'text'});
    }
    myOrders(): Observable<OrderModel[]> {
        return this.httpClient.get<OrderModel[]>(`${this.baseUrl}/order/my-orders`);
    }
}
