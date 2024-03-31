import {Component} from '@angular/core';
import {ProductModel} from "../../../models/product.model";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastService} from "../../../services/toast-service";
import {ProductService} from "../../../services/product.service";
import {ProductEditComponent} from "./product-edit/product-edit.component";
import Swal from "sweetalert2";
import {DayOfWeek} from 'src/app/models/enums';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrl: './products.component.scss'
})
export class ProductsComponent {
    products: ProductModel[];
    selectedDay;
    DayOfWeek = DayOfWeek;

    constructor(private modalService: NgbModal,
                public toastService: ToastService,
                private productService: ProductService) {
    }

    onChange(newValue: any) {
        this.selectedDay = newValue.target.value;
        if (this.selectedDay) {
            this.getProductsList();
        }
    }

    ngOnInit(): void {

    }

    getProductsList(): void {
        this.productService.list(this.selectedDay).subscribe(products => {
            this.products = products;
        });
    }

    openAddModal(): void {
        const modalRef = this.modalService.open(ProductEditComponent, // Adjusted modal component
            {size: 'md', centered: true});
        modalRef.componentInstance.isNew = true;
        modalRef.componentInstance.productAdded.subscribe(() => {
            this.getProductsList();
            modalRef.close();
        });
    }

    openEditModal(product: ProductModel): void {
        const modalRef = this.modalService.open(ProductEditComponent, // Adjusted modal component
            {size: 'md', centered: true});
        modalRef.componentInstance.isNew = false;
        modalRef.componentInstance.productModel = product;
        modalRef.componentInstance.productUpdated.subscribe(() => {
            this.getProductsList();
            modalRef.close();
        });
    }

    openDeleteConfirm(product: ProductModel): void {
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                actions: 'justify-content-end mx-4',
                confirmButton: 'btn btn-danger ms-2 justify-content-end',
                cancelButton: 'btn btn-light justify-content-end'
            },
            buttonsStyling: false
        });

        swalWithBootstrapButtons.fire({
            title: 'Delete Confirmation',
            text: 'Delete Confirmation',
            icon: 'question',
            showCancelButton: true,
            cancelButtonText: 'No',
            confirmButtonText: 'Yes',
            reverseButtons: true
        }).then(result => {
            if (result.value) {
                this.delete(product.id);
            }
        });
    }

    delete(id: string): void {
        this.productService.delete(id).subscribe({
            next: async response => {
                this.toastService.success('Process Completed Successfully');
                this.getProductsList();
            },
            error: async error => {
                this.toastService.fake();
                throw error;
            }
        });
    }
}
