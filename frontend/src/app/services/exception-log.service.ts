import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";
import {ExceptionLog} from "../models/exception-log";


@Injectable({providedIn: 'root'})
export class ExceptionLogService {
    private readonly baseUrl: string;

    constructor(
        private httpClient: HttpClient
    ) {
        this.baseUrl = `${environment.apiUrl}/exception-log`;
    }

    add(exceptionLog: ExceptionLog): Observable<any> {
        return this.httpClient.post<any>(`${this.baseUrl}/add`, exceptionLog);
    }
}
