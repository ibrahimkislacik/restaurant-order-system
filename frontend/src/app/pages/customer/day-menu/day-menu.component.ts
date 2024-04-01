import {Component, OnInit} from '@angular/core';
import {RouterModule} from "@angular/router";
import {SharedModule} from "../../../theme/shared/shared.module";
import {ProductModel} from "../../../models/product.model";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastService} from "../../../services/toast-service";
import {CustomerService} from "../../../services/customer.service";
import {AddToCartComponent} from "./add-to-cart/add-to-cart.component";
import {PipesModule} from "../../../pipes/pipes.module";
import {MyCartComponent} from "./my-cart/my-cart.component";

@Component({
    selector: 'app-day-menu',
    standalone: true,
    imports: [RouterModule, SharedModule, PipesModule],
    templateUrl: './day-menu.component.html',
    styleUrl: './day-menu.component.scss'
})
export class DayMenuComponent implements OnInit {
    products: ProductModel[];

    constructor(private modalService: NgbModal,
                public toastService: ToastService,
                private customerService: CustomerService) {
    }

    ngOnInit(): void {
        this.getProductsList();
    }

    getProductsList(): void {
        this.customerService.productList().subscribe(products => {
            this.products = products;
        });
    }

    openCartAddModal(productModel: ProductModel): void {
        const modalRef = this.modalService.open(AddToCartComponent,
            {size: 'md', fullscreen: 'md', centered: true});
        modalRef.componentInstance.product = productModel;
        modalRef.componentInstance.cartItemAdded.subscribe(() => {
            modalRef.close();
        });
    }

    openMyCartModal(): void {
        const modalRef = this.modalService.open(MyCartComponent,
            {size: 'md', fullscreen: 'md', centered: true});
        modalRef.componentInstance.orderCreated.subscribe(() => {
            modalRef.close();
        });
    }

}

