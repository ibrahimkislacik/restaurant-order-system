import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {DayMenuComponent} from "./day-menu/day-menu.component";
import {OrdersComponent} from "./orders/orders.component";

const routes: Routes = [
    {
        path: 'menu',
        component: DayMenuComponent
    },
    {
        path: 'orders',
        component: OrdersComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomerRoutingModule {}
