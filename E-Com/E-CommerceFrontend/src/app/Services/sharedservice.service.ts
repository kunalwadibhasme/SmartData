import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedserviceService {

  private totalQuantitySource = new BehaviorSubject<number>(0);
  totalQuantity$ = this.totalQuantitySource.asObservable();

  updateTotalQuantity(quantity: number) {
    this.totalQuantitySource.next(quantity);
  }
}
