import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StripeService {
  private baseUrl = 'https://localhost:7162/api/Payment/create-payment-intent'; 

  constructor(private http: HttpClient) {}

  createPaymentIntent(amount: any) {
    return this.http.post<any>(`${this.baseUrl}`, { amount }).toPromise();
  }
}
