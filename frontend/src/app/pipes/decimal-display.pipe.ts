import {Pipe, PipeTransform} from '@angular/core';
import {UtilService} from "../services/util.service";

@Pipe({name: 'decimalDisplay'})
export class DecimalDisplayPipe implements PipeTransform {

    constructor(private utilService: UtilService) {

    }

    transform(value: number): string {
        return this.utilService.formatDecimal(value);
    }
    
}
