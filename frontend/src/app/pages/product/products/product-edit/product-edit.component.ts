import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CategoryModel} from "../../../../models/category.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {CategoryService} from "../../../../services/category.service";
import {ToastService} from "../../../../services/toast-service";
import {ProductModel} from "../../../../models/product.model";
import {ProductService} from "../../../../services/product.service";
import {UtilService} from "../../../../services/util.service";

@Component({
    selector: 'app-product-edit',
    templateUrl: './product-edit.component.html',
    styleUrl: './product-edit.component.scss'
})
export class ProductEditComponent implements OnInit {
    @Input()
    isNew: boolean;

    @Input()
    productModel: ProductModel;

    @Output()
    productUpdated: EventEmitter<any> = new EventEmitter();
    @Output()
    productAdded: EventEmitter<any> = new EventEmitter();

    categories: CategoryModel[];

    get f() {
        return this.formGroup.controls;
    }


    formGroup: FormGroup;
    isSubmitted: boolean = false;
    isButtonSpinnerActive: boolean = false;

    get currency() {
        return this.utilService.getCurrency();
    }

    constructor(private formBuilder: FormBuilder,
                public activeModal: NgbActiveModal,
                private categoryService: CategoryService,
                private productService: ProductService,
                private utilService: UtilService,
                public toastService: ToastService) {

        this.formGroup = this.formBuilder.group({
            categoryId:[],
            name: [null, [Validators.required]],
            description: [null],
            price: [null, [Validators.required]],
            isActiveOnMonday: [true],
            isActiveOnTuesday: [true],
            isActiveOnWednesday: [true],
            isActiveOnThursday: [true],
            isActiveOnFriday: [true],
            isActiveOnSaturday: [true],
            isActiveOnSunday: [true],
        });
    }

    ngOnInit(): void {
        if (this.isNew) {
            this.productModel = new ProductModel();
            this.getCategoriesList();
        }
        else {
            this.bindForm();
        }
    }

    getCategoriesList(): void {
        this.categoryService.list().subscribe(categories => {
            this.categories = categories;
        });
    }

    bindForm(): void {
        this.formGroup.patchValue({
            name: this.productModel.name,
            description: this.productModel.description,
            price: this.utilService.formatDecimal(this.productModel.price),
            isActiveOnMonday: this.productModel.isActiveOnMonday,
            isActiveOnTuesday: this.productModel.isActiveOnTuesday,
            isActiveOnWednesday: this.productModel.isActiveOnWednesday,
            isActiveOnThursday: this.productModel.isActiveOnThursday,
            isActiveOnFriday: this.productModel.isActiveOnFriday,
            isActiveOnSaturday: this.productModel.isActiveOnSaturday,
            isActiveOnSunday: this.productModel.isActiveOnSunday
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
        this.productModel.name = this.f['name'].value;
        this.productModel.description = this.f['description'].value;
        this.productModel.price = this.utilService.parseFloatCustom(this.f['price'].value);
        this.productModel.isActiveOnMonday = this.f['isActiveOnMonday'].value;
        this.productModel.isActiveOnTuesday = this.f['isActiveOnTuesday'].value;
        this.productModel.isActiveOnWednesday = this.f['isActiveOnWednesday'].value;
        this.productModel.isActiveOnThursday = this.f['isActiveOnThursday'].value;
        this.productModel.isActiveOnFriday = this.f['isActiveOnFriday'].value;
        this.productModel.isActiveOnSaturday = this.f['isActiveOnSaturday'].value;
        this.productModel.isActiveOnSunday = this.f['isActiveOnSunday'].value;
        if (this.isNew) {
            this.productModel.categoryId = this.f['categoryId'].value;
        }
    }

    insert() {
        this.isButtonSpinnerActive = true;
        this.productService.insert(this.productModel).subscribe({
            next: async response => {
                console.log(response.status);
                this.isButtonSpinnerActive = false;
                this.toastService.success('Process Completed Successfully');
                this.productAdded.emit();
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
        this.productService.update(this.productModel).subscribe({
            next: async response => {
                this.isButtonSpinnerActive = false;
                this.toastService.success('Process Completed Successfully');
                this.productUpdated.emit();
            },
            error: async error => {
                this.isButtonSpinnerActive = false;
                this.toastService.fake();
                throw error;
            }
        });
    }

}
