import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const sellingPriceGreaterThanPurchasePrice: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
const sellingPrice = control.get('sellingPrice')?.value;
const purchasePrice = control.get('purchasePrice')?.value;

return sellingPrice > purchasePrice ? null : { sellingPriceLessThanPurchasePrice: true };
};