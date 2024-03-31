import {Component, OnInit} from '@angular/core';
import {CategoryModel} from "../../../models/category.model";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {CategoryService} from "../../../services/category.service";
import {CategoryEditComponent} from "./category-edit/category-edit.component";
import Swal from 'sweetalert2';
import {ToastService} from "../../../services/toast-service";

@Component({
    selector: 'app-categories',
    templateUrl: './categories.component.html',
    styleUrl: './categories.component.scss'
})
export class CategoriesComponent implements OnInit {
    categories: CategoryModel[];

    constructor(private modalService: NgbModal,
                public toastService: ToastService,
                private categoryService: CategoryService) {
    }

    ngOnInit(): void {
        this.getCategoriesList();
    }

    getCategoriesList(): void {
        this.categoryService.list().subscribe(categories => {
            this.categories = categories;
        });
    }

    openAddModal(): void {
        const modalRef = this.modalService.open(CategoryEditComponent,
            {size: 'md', fullscreen: 'md', centered: true});
        modalRef.componentInstance.isNew = true;
        modalRef.componentInstance.categoryAdded.subscribe(() => {
            this.getCategoriesList();
            modalRef.close();
        });
    }

    openEditModal(category: CategoryModel): void {

        const modalRef = this.modalService.open(CategoryEditComponent,
            {size: 'md', fullscreen: 'md', centered: true});
        modalRef.componentInstance.isNew = false;
        modalRef.componentInstance.categoryModel = category;
        modalRef.componentInstance.categoryUpdated.subscribe(() => {
            this.getCategoriesList();
            modalRef.close();
        });
    }

    openDeleteConfirm(category: CategoryModel): void {

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
                this.delete(category.id);
            }
        });
    }

    delete(id: string): void {
        this.categoryService.delete(id).subscribe({
            next: async response => {
                this.toastService.success('Process Completed Successfully');
                this.getCategoriesList();
            },
            error: async error => {
                this.toastService.fake();
                throw error;
            }
        });
    }


}
