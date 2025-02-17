import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddtoCartService {
  validatePayment(credentials: any) {
    return this.http.post<any>(`${this.baseUrl}/`,credentials);
  }
  
  constructor(private http: HttpClient) { }

  private baseUrl = 'https://localhost:7162/api/Cart'; 

  CartMasterDetail(usertableId:any, productId:any, quantity:any):Observable<any>{
   const body={usertableId, productId, quantity}
    return this.http.post<any>(`${this.baseUrl}/AddMasterDetails`,body);
  }

  getProductById(Id:any):Observable<any>
  {
    return this.http.get<any>(`https://localhost:7162/api/AdminProduct/GetProductById/${Id}`);
  }

  getCartItems(Id:any):Observable<any>
  {
    return this.http.get<any>(`${this.baseUrl}/FetchUserAddedProduct?Id=${Id}`)
  }

  deleteCartItem(ProductId: any, userTableId:any):Observable<any>
  {
    return this.http.delete<any>(`${this.baseUrl}/ReducefromMastertables?productId=${ProductId}&userTableId=${userTableId}`);
  }

  Updatesalesdetail(UserId: any, total: any): Observable<any>
  {
    return this.http.delete<any>(`${this.baseUrl}/Updatesalesdetails?UserId=${UserId}&total=${total}`);
      // https://localhost:7162/api/Cart/Updatesalesdetails?UserId=22&total=220
  }
  Updatesalesdetail2(UserId: any, total: any): Observable<any>
  {
    return this.http.delete<any>(`${this.baseUrl}/Updatesalesdetails2?UserId=${UserId}&total=${total}`);
      // https://localhost:7162/api/Cart/Updatesalesdetails?UserId=22&total=220
  }
  
  ValidatePayment(cardNumber:any, expirtDate:any, cvv:any)
  {
    const body={cardNumber, expirtDate, cvv};
    return this.http.post<any>(`${this.baseUrl}/ValidateDetails`,body);
  }

  GenerateReceipt(UserId : any)
  {
    return this.http.get<any>(`${this.baseUrl}/GetSalesDetail?UserId=${UserId}`);
  }

  // https://localhost:7162/api/Cart/GetSalesDetail?UserId=22

}

//https://localhost:7162/api/Cart/ValidateDetails


// https://localhost:7162/api/Cart/AddMasterDetails
// https://localhost:7162/api/Cart/ReducefromMastertables?productId=6&userTableId=21
