import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from "../../theme/shared/shared.module";
import {CustomerRoutingModule} from "./customer-routing.module";
import {PipesModule} from "../../pipes/pipes.module";

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        SharedModule,
        CustomerRoutingModule,
        PipesModule
    ],
})
export class CustomerModule {
}
