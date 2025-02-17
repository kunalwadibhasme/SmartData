import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReceiptSharedserviceService {

  constructor() { }

  private receiptdata = new BehaviorSubject<any>(0);
  receiptData$ = this.receiptdata.asObservable();

  GetReceiptData(data : any)
  {
    this.receiptdata.next(data);
  }


}
