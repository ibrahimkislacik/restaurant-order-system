import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CategoryModel} from "../../../../models/category.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {CategoryService} from "../../../../services/category.service";
import {ToastService} from "../../../../services/toast-service";

@Component({
    selector: 'app-category-edit',
    templateUrl: './category-edit.component.html',
    styleUrl: './category-edit.component.scss'
})
export class CategoryEditComponent implements OnInit {

    @Input()
    isNew: boolean;

    @Input()
    categoryModel: CategoryModel;

    @Output()
    categoryUpdated: EventEmitter<any> = new EventEmitter();
    @Output()
    categoryAdded: EventEmitter<any> = new EventEmitter();

    get f() {
        return this.formGroup.controls;
    }

    formGroup: FormGroup;
    isSubmitted: boolean = false;
    isButtonSpinnerActive: boolean = false;

    constructor(private formBuilder: FormBuilder,
                public activeModal: NgbActiveModal,
                private categoryService: CategoryService,
                public toastService: ToastService) {

        this.formGroup = this.formBuilder.group({
            name: [null, [Validators.required]],
        });
    }

    ngOnInit(): void {
        if (this.isNew) {
            this.categoryModel = new CategoryModel();
        }
        else {
            this.bindForm();
        }
    }

    bindForm(): void {
        this.formGroup.patchValue({
            name: this.categoryModel.name,
        });

    }

    async onSubmit() {
        this.isSubmitted = true;

        if (this.formGroup.invalid) {
            return;
        }

        this.bindModel();

        if (this.isNew) {
            this.insert();
        }
        else {
            this.update();
        }

    }

    bindModel(): void {
        this.categoryModel.name = this.f['name'].value;
    }

    insert() {
        this.isButtonSpinnerActive = true;
        this.categoryService.insert(this.categoryModel).subscribe({
            next: async response => {
                console.log(response.status);
                this.isButtonSpinnerActive = false;
                this.toastService.success('Process Completed Successfully');
                this.categoryAdded.emit();
            },
            error: async error => {
                console.log(error);
                this.isButtonSpinnerActive = false;
                this.toastService.fake();
                throw error;
            }
        });
    }

    update() {
        this.isButtonSpinnerActive = true;
        this.categoryService.update(this.categoryModel).subscribe({
            next: async response => {
                this.isButtonSpinnerActive = false;
                this.toastService.success('Process Completed Successfully');
                this.categoryUpdated.emit();
            },
            error: async error => {
                this.isButtonSpinnerActive = false;
                this.toastService.fake();
                throw error;
            }
        });
    }


}
