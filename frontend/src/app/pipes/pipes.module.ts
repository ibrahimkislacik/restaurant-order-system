import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DecimalDisplayPipe} from "./decimal-display.pipe";

@NgModule({
    declarations: [DecimalDisplayPipe],
    imports: [CommonModule],
    exports: [DecimalDisplayPipe]
})
export class PipesModule {
}
