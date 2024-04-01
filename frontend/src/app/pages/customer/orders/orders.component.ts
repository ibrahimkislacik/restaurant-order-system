import {Component, OnInit} from '@angular/core';
import {PipesModule} from "../../../pipes/pipes.module";
import {CustomerService} from "../../../services/customer.service";
import {OrderModel} from "../../../models/order.model";
import {RouterModule} from "@angular/router";
import {SharedModule} from "../../../theme/shared/shared.module";

@Component({
    selector: 'app-orders',
    imports: [RouterModule, SharedModule, PipesModule],
    standalone: true,
    templateUrl: './orders.component.html',
    styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit {
    orders: OrderModel[];

    constructor(private customerService: CustomerService) {
    }

    ngOnInit(): void {
        this.getOrdersList();
    }

    getOrdersList(): void {
        this.customerService.myOrders().subscribe(orders => {
            this.orders = orders;
        });
    }
}
