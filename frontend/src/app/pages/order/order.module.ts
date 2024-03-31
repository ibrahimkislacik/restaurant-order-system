import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {OrderRoutingModule} from "./order-routing.module";
import {SharedModule} from "../../theme/shared/shared.module";
import {OrdersComponent} from "./orders/orders.component";
import {OrderDetailComponent} from "./orders/order-detail/order-detail.component";
import {DirectivesModule} from "../../directives/directives.module";
import {PipesModule} from "../../pipes/pipes.module";


@NgModule({
    declarations: [
        OrdersComponent,
        OrderDetailComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        OrderRoutingModule,
        DirectivesModule,
        PipesModule
    ]
})
export class OrderModule {
}
