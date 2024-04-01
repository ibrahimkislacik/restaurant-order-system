// angular import
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

// project import
import {CardComponent} from './components/card/card.component';
import {BreadcrumbsComponent} from './components/breadcrumbs/breadcrumbs.component';
import {SpinnerComponent} from './components/spinner/spinner.component';

// bootstrap import
import {NgbModule, NgbToastModule} from '@ng-bootstrap/ng-bootstrap';
import {NgbCollapseModule} from '@ng-bootstrap/ng-bootstrap';

// third party
import {NgScrollbarModule} from 'ngx-scrollbar';
import {ToastsContainerComponent} from "./components/toast/toasts-container.component";

@NgModule({
    declarations: [
        SpinnerComponent,
        ToastsContainerComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        CardComponent,
        NgbModule,
        NgbToastModule,
        NgScrollbarModule,
        NgbCollapseModule,
        BreadcrumbsComponent,
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        CardComponent,
        SpinnerComponent,
        NgbModule,
        NgScrollbarModule,
        NgbCollapseModule,
        BreadcrumbsComponent,
        ToastsContainerComponent
    ],
})
export class SharedModule {
}
