import {Injectable} from "@angular/core";
import {UserModel} from "../models/user.model";

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

    clear() {
        localStorage.clear();
    }
}
