import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CategoryRoutingModule} from "./category-routing.module";
import {CategoriesComponent} from "./categories/categories.component";
import {CategoryEditComponent} from "./categories/category-edit/category-edit.component";
import {SharedModule} from "../../theme/shared/shared.module";

@NgModule({
    declarations: [
        CategoriesComponent,
        CategoryEditComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        CategoryRoutingModule
    ],
})
export class CategoryModule {
}
