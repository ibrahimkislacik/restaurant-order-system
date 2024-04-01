import {Directive, ElementRef, HostListener} from '@angular/core';

@Directive({
    selector: '[decimal]'
})
export class DecimalDirective {
    inputElement: HTMLInputElement;

    decimalSeparator: string = ',';

    constructor(public el: ElementRef) {
        this.inputElement = el.nativeElement;
    }

    @HostListener('keydown', ['$event'])
    onKeyDown(e: KeyboardEvent) {

        if (
            [46, 8, 9, 27, 13].indexOf(e.keyCode) !== -1 || // Allow: Delete, Backspace, Tab, Escape, Enter
            (e.keyCode === 65 && e.ctrlKey === true) || // Allow: Ctrl+A
            (e.keyCode === 67 && e.ctrlKey === true) || // Allow: Ctrl+C
            (e.keyCode === 86 && e.ctrlKey === true) || // Allow: Ctrl+V
            (e.keyCode === 88 && e.ctrlKey === true) || // Allow: Ctrl+X
            (e.keyCode === 65 && e.metaKey === true) || // Allow: Cmd+A (Mac)
            (e.keyCode === 67 && e.metaKey === true) || // Allow: Cmd+C (Mac)
            (e.keyCode === 86 && e.metaKey === true) || // Allow: Cmd+V (Mac)
            (e.keyCode === 88 && e.metaKey === true) || // Allow: Cmd+X (Mac)
            (e.keyCode >= 35 && e.keyCode <= 39)      // Allow: Home, End, Left, Right
        ) {
            // let it happen, don't do anything
            return;
        }

        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) && e.keyCode != 110 && e.keyCode != 188 && e.keyCode != 190) {
            e.preventDefault();
        }
        else {
            // if the input has max value and exceeds the limit prevent input
            if (this.inputElement
                && this.inputElement.maxLength
                && this.inputElement.maxLength > 0
                && this.inputElement.value
                && this.inputElement.value.length >= this.inputElement.maxLength) {
                e.preventDefault();
                return;
            }

            let separator = this.decimalSeparator;

            if (separator == ',' && e.key == '.') {
                e.preventDefault();
                return;
            }
            else if (separator == '.' && e.key == ',') {
                e.preventDefault();
                return;
            }


            //allow . for once
            //if there is already a separator
            let indexOfSeparator = this.inputElement.value ? this.inputElement.value.indexOf(separator) : -1;
            if (indexOfSeparator >= 0) {
                //if the new key is dot, prevent it
                if ((e.keyCode == 188 || e.keyCode == 110 || e.keyCode == 190) && this.inputElement.value.indexOf(
                    separator) >= 0) {
                    e.preventDefault();
                    return;
                }

                let lengthAfterDot = this.inputElement.value.length - indexOfSeparator - 1;
                if (lengthAfterDot >= 2) {
                    if ((this.inputElement.selectionEnd - this.inputElement.selectionStart) > 0) {
                        return;
                    }
                    e.preventDefault();
                    return;
                }
            }
        }

        this.validateInput(e.key, e);
    }

    @HostListener('paste', ['$event'])
    onPaste(event: ClipboardEvent) {
        event.preventDefault();
        const pastedInput: string = event.clipboardData
            .getData('text/plain')
            .replace(/\D/g, ''); // get a digit-only string
        this.validateInput(pastedInput, event, () => {
            document.execCommand('insertText', false, pastedInput);
        });
        // document.execCommand('insertText', false, pastedInput);
    }

    @HostListener('drop', ['$event'])
    onDrop(event: DragEvent) {
        event.preventDefault();
        const textData = event.dataTransfer.getData('text').replace(/\D/g, '');
        this.inputElement.focus();
        this.validateInput(textData, event, () => {
            document.execCommand('insertText', false, textData);
        });
        // document.execCommand('insertText', false, textData);
    }

    private validateInput(data, event: Event, validCallback?: () => void) {
        var reg = new RegExp('^\\d+$');

        if (reg.test(data)) {
            let nextValue = this.inputElement.value + data

            if (this.inputElement.attributes['max']) {
                let maxValue = Number(this.inputElement.attributes['max'].value);

                if (Number(nextValue) > maxValue) {
                    event.preventDefault();
                    this.inputElement.value = maxValue.toString();
                    this.inputElement.dispatchEvent(new Event('input'));
                    return false;
                }
            }

            if (this.inputElement.attributes['min']) {

                let minValue = Number(this.inputElement.attributes['min'].value);


                if (Number(nextValue) < minValue) {
                    event.preventDefault();
                    this.inputElement.value = minValue.toString();
                    this.inputElement.firstElementChild.dispatchEvent(new Event('input'));
                    return false;
                }
            }
        }
        if (validCallback) {
            validCallback();
        }
        return false;
    }
}
