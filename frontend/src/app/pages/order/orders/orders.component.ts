import { Component } from '@angular/core';
import {CategoryModel} from "../../../models/category.model";
import {CategoryEditComponent} from "../../category/categories/category-edit/category-edit.component";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastService} from "../../../services/toast-service";
import {CategoryService} from "../../../services/category.service";
import {OrderModel} from "../../../models/order.model";
import {OrderService} from "../../../services/order.service";
import {OrderDetailComponent} from "./order-detail/order-detail.component";

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent {
  orders: OrderModel[];

  constructor(private modalService: NgbModal,
              public toastService: ToastService,
              private orderService: OrderService) {
  }

  ngOnInit(): void {
    this.getOrdersList();
  }

  getOrdersList(): void {
    this.orderService.list().subscribe(orders => {
      this.orders = orders;
    });
  }

  openDetailModal(order: OrderModel): void {

    const modalRef = this.modalService.open(OrderDetailComponent,
        {size: 'md', fullscreen: 'md', centered: true});
    modalRef.componentInstance.orderModel = order;
  }
  
}
