import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {CartService} from "../../../../services/cart.service";
import {ToastService} from "../../../../services/toast-service";
import {OrderInsertRequestModel} from "../../../../models/order-insert-request.model";
import {PipesModule} from "../../../../pipes/pipes.module";
import {CustomerService} from "../../../../services/customer.service";
import {CartItemModel} from "../../../../models/cart-item.model";

@Component({
    selector: 'app-my-cart',
    standalone: true,
    imports: [
        PipesModule
    ],
    templateUrl: './my-cart.component.html',
    styleUrl: './my-cart.component.scss'
})
export class MyCartComponent implements OnInit {

    cartItems: CartItemModel[];
    isButtonSpinnerActive: boolean = false;

    @Output()
    orderCreated: EventEmitter<any> = new EventEmitter();

    constructor(public activeModal: NgbActiveModal,
                private cartService: CartService,
                private customerService: CustomerService,
                public toastService: ToastService) {
    }

    ngOnInit(): void {
        this.getCartItems();
    }

    getCartItems(): void {
        this.cartItems = this.cartService.getCartItems();
    }

    removeCartItem(cartItem: CartItemModel): void {
        this.cartService.removeFromCart(cartItem);
        this.getCartItems();
    }

    createOrder(): void {
        let orderInsertRequestModel = new OrderInsertRequestModel();
        orderInsertRequestModel.orderProducts = [];
        this.cartItems.forEach(function (cartItem) {
            orderInsertRequestModel.orderProducts.push({
                productId: cartItem.productId,
                note: cartItem.note,
                quantity: cartItem.quantity
            });
        });
        this.isButtonSpinnerActive = true;
        this.customerService.insertOrder(orderInsertRequestModel).subscribe({
            next: async response => {
                console.log(response.status);
                this.isButtonSpinnerActive = false;
                this.toastService.success('Process Completed Successfully');
                this.cartService.clearCart();
                this.orderCreated.emit();
            },
            error: async error => {
                console.log(error);
                this.isButtonSpinnerActive = false;
                this.toastService.fake();
                throw error;
            }
        });
    }

}
