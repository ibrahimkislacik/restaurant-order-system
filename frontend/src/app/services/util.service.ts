import {Injectable} from "@angular/core";

@Injectable({providedIn: 'root'})
export class UtilService {
    decimalSeparator: string = ',';

    getCurrency(): string {
        return 'TL';
    }

    formatDecimal(value: number, returnZeroWhenUndefined = false) {
        if (isNaN(value)) {
            return '';
        }
        if (value == undefined) {
            if (returnZeroWhenUndefined) {
                return '0' + this.decimalSeparator + '00';
            }
            else {
                return '';
            }
        }
        let formatter: Intl.NumberFormat;
        if (this.decimalSeparator == '.') {
            formatter = new Intl.NumberFormat('en-US', {
                style: 'decimal',
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
        }
        else {
            formatter = new Intl.NumberFormat('tr-TR', {
                style: 'decimal',
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
        }
        return formatter.format(value);
    }

    parseFloatCustom(text: string): number {
        if (!text) {
            return 0;
        }
        let decimalSeparator = this.decimalSeparator;
        if (decimalSeparator == ',') {
            text = text.toString().replace(',', '.');
        }
        return parseFloat(text);
    }

}
