import {Component, Input} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {CategoryService} from "../../../../services/category.service";
import {ToastService} from "../../../../services/toast-service";
import {CategoryModel} from "../../../../models/category.model";
import {OrderModel} from "../../../../models/order.model";
import {UtilService} from "../../../../services/util.service";

@Component({
    selector: 'app-order-detail',
    templateUrl: './order-detail.component.html',
    styleUrl: './order-detail.component.scss'
})
export class OrderDetailComponent {
    @Input()
    orderModel: OrderModel;

    get currency() {
        return this.utilService.getCurrency();
    }

    constructor(public activeModal: NgbActiveModal,
                private utilService: UtilService) {
    }

}
