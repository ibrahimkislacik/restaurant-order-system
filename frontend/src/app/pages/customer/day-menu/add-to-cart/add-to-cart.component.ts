import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastService} from "../../../../services/toast-service";
import {CartService} from "../../../../services/cart.service";
import {ProductModel} from "../../../../models/product.model";
import {CartItemModel} from "../../../../models/cart-item.model";

@Component({
    selector: 'app-add-to-cart',
    imports: [
        NgIf,
        ReactiveFormsModule
    ],
    templateUrl: './add-to-cart.component.html',
    standalone: true,
    styleUrl: './add-to-cart.component.scss'
})
export class AddToCartComponent implements OnInit {

    @Input()
    product: ProductModel;

    @Output()
    cartItemAdded: EventEmitter<any> = new EventEmitter();

    cartItemModel: CartItemModel;

    get f() {
        return this.formGroup.controls;
    }

    formGroup: FormGroup;
    isSubmitted: boolean = false;
    isButtonSpinnerActive: boolean = false;

    constructor(private formBuilder: FormBuilder,
                public activeModal: NgbActiveModal,
                private cartService: CartService,
                public toastService: ToastService) {

        this.formGroup = this.formBuilder.group({
            quantity: [null, [Validators.required]],
            note: [null],
        });
    }

    ngOnInit(): void {
        this.cartItemModel = new CartItemModel();
    }

    async onSubmit() {
        this.isSubmitted = true;

        if (this.formGroup.invalid) {
            return;
        }

        this.bindModel();
        this.insert();
    }

    bindModel(): void {
        this.cartItemModel.cartId = new Date().getTime().toString();
        this.cartItemModel.productId = this.product.id;
        this.cartItemModel.name = this.product.name;
        this.cartItemModel.price = this.product.price;
        this.cartItemModel.quantity = this.f['quantity'].value;
        this.cartItemModel.note = this.f['note'].value;
    }

    insert() {
        this.cartService.addToCart(this.cartItemModel);
        this.toastService.success('Process Completed Successfully');
        this.cartItemAdded.emit();
    }


}
