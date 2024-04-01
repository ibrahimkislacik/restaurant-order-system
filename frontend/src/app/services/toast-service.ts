import {Injectable, TemplateRef} from '@angular/core';

@Injectable({providedIn: 'root'})
export class ToastService {
    toasts: any[] = [];
    delay: number = 2000;

    show(textOrTpl: string | TemplateRef<any>, options: any = {}) {
        this.toasts.push({textOrTpl, ...options});
    }

    success(textOrTpl: string | TemplateRef<any>) {
        this.show(textOrTpl, {classname: 'bg-success text-light', delay: this.delay});
    }

    error(textOrTpl: string | TemplateRef<any>) {
        this.show(textOrTpl, {classname: 'bg-danger text-light', delay: this.delay});
    }

    info(textOrTpl: string | TemplateRef<any>) {
        this.show(textOrTpl, {classname: 'bg-info text-light', delay: this.delay});
    }

    warning(textOrTpl: string | TemplateRef<any>) {
        this.show(textOrTpl, {classname: 'bg-warning text-dark', delay: this.delay});
    }

    fake() {
        this.show('', {classname: 'bg-light text-light', delay: 1});
    }

    remove(toast: any) {
        this.toasts = this.toasts.filter(t => t !== toast);
    }

    clear() {
        this.toasts.splice(0, this.toasts.length);
    }
}
