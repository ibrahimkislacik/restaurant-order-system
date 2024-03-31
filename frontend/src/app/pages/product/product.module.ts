import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from "../../theme/shared/shared.module";
import {ProductsComponent} from "./products/products.component";
import {ProductEditComponent} from "./products/product-edit/product-edit.component";
import {ProductRoutingModule} from "./product-routing.module";
import {DirectivesModule} from "../../directives/directives.module";
import {PipesModule} from "../../pipes/pipes.module";

@NgModule({
    declarations: [
        ProductsComponent,
        ProductEditComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        ProductRoutingModule,
        DirectivesModule,
        PipesModule
    ]
})
export class ProductModule {
}
