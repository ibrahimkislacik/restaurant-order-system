import {Injectable} from "@angular/core";
import {UserModel} from "../models/user.model";
import {OrderInsertRequestModel} from "../models/order-insert-request.model";

@Injectable({providedIn: 'root'})
export class StorageService {

    private _user: UserModel;
    public get user(): UserModel {
        if (!this._user) {
            let storageValue = localStorage.getItem('user');
            if (!storageValue) {
                return null;
            }
            this._user = JSON.parse(storageValue);
        }
        return this._user;
    }

    public set user(user: UserModel) {
        localStorage.setItem('user', JSON.stringify(user));
        this._user = user;
    }

    private _token: string;
    public get token(): string {
        if (!this._token) {
            let storageValue = localStorage.getItem('token');
            if (!storageValue) {
                return null;
            }
            this._token = storageValue;
        }
        return this._token;
    }

    public set token(token: string) {
        localStorage.setItem('token', token);
        this._token = token;
    }

    private _cart: OrderInsertRequestModel;
    public get cart(): OrderInsertRequestModel {
        if (!this._cart) {
            let storageValue = localStorage.getItem('cart');
            if (!storageValue) {
                return null;
            }
            this._user = JSON.parse(storageValue);
        }
        return this._cart;
    }

    public set cart(cart: OrderInsertRequestModel) {
        localStorage.setItem('cart', JSON.stringify(cart));
        this._cart = cart;
    }
    

    clear() {
        localStorage.clear();
    }
}
