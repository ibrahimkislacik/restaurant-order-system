import {ErrorHandler, Injectable, Injector} from '@angular/core';
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";
import {ToastService} from "../services/toast-service";
import {Logger} from "../services/logger.service";
import {ExceptionLog} from "../models/exception-log";
import {ExceptionLogService} from "../services/exception-log.service";

@Injectable()
export class CustomErrorHandler implements ErrorHandler {
    constructor(private router: Router,
                public toastService: ToastService,
                private injector: Injector) {
    }

    handleError(error: any): void {
        /* for server side logging
        if (error instanceof HttpErrorResponse && error.url.includes('api/exceptionLog')) {
            Logger.error('Network error', error);
            return;
        }
        const exceptionLog = {
            exceptionMessage: error.message || `Unknown exception occured`,
            exceptionStackTrace: error.stack || '',
        } as ExceptionLog;
        const exceptionLogService = this.injector.get(ExceptionLogService);
        exceptionLogService.add(exceptionLog).subscribe();
        */

        if (error.promise && error.rejection) {
            // Promise rejection wrapped by zone.js
            error = error.rejection;
        }
        switch (error.status) {
            case 400: {
                const key = error.error ? error.error.message :'Model Error Occured';
                this.toastService.warning(key);
                break;
            }
            case 401: {
                this.router.navigate(["/auht/signin"]).then();
                break;
            }
            case 404: {
                this.router.navigate(["/"]).then();
                this.toastService.error('Error Occured');
                break;
            }
            case 500: {
                this.toastService.error('Error Occured');
                break;
            }
            default: {
                this.toastService.error('Error Occured');
                break;
            }
        }
    }


}
