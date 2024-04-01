import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {StorageService} from "./storage.service";
import {LoginRequestModel} from "../models/login-request.model";
import {JwtHelperService} from "@auth0/angular-jwt";
import {LoginResponseModel} from "../models/login-response.model";
import {UserModel} from "../models/user.model";
import {Observable, tap} from "rxjs";
import {environment} from "../../environments/environment";


@Injectable({providedIn: 'root'})
export class LoginService {
    private readonly baseUrl: string;

    constructor(private httpClient: HttpClient,
                private storageService: StorageService) {
        this.baseUrl = `${environment.apiUrl}/user`;
    }


    login(loginRequestModel: LoginRequestModel): Observable<LoginResponseModel> {
        const helper = new JwtHelperService();
        return this.httpClient.post<LoginResponseModel>(`${this.baseUrl}/authenticate`, loginRequestModel).pipe(
            tap(result => {
                this.storageService.token = result.accessToken;
                let data = helper.decodeToken(result.accessToken);
                let user =new UserModel();
                user.eMail = data['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
                user.name = data['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
                user.isAdmin = data['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] == 'Admin';
                user.id = data['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
                this.storageService.user = user;
            })
        );

    }

    logout() {
        this.storageService.clear();
    }

}
