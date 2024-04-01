import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError, finalize, of, Subject, takeUntil } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import {Router} from "@angular/router";
import {StorageService} from "../services/storage.service";
import {Logger} from "../services/logger.service";

@Injectable()
export class CustomHttpInterceptor implements HttpInterceptor {
    private isRefreshing = false;
    private cancelRequest$: Subject<void> = new Subject<void>();


    constructor(private router: Router, private storageService: StorageService) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${this.storageService.token}`,
                    'Content-Type': 'application/json; charset=utf-8',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, DELETE',
                    'Access-Control-Allow-Headers': 'Content-Type, Authorization'
                },
                withCredentials: true
            });
            
        // @ts-ignore
        return next.handle(request).pipe(
            catchError(error => {
                if (error instanceof HttpErrorResponse && error.status === 401) {
                    Logger.warn('HttpInterceptor', 'Request failed 401 ' + request.url);
                    return this.handle401Error(request, next);
                }
                else {
                    Logger.error('HttpInterceptor', 'Request failed ' + request.url)
                    return throwError(error);
                }
            }),
            takeUntil(this.cancelRequest$) // Add takeUntil operator to cancel the request when needed
        );
    }

    private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
        this.router.navigateByUrl(`/auth/signin`);
        return new Observable();
    }
}
