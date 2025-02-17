import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'https://localhost:7162/api/AdminProduct';
  


  constructor(private http: HttpClient) { }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetAllProducts`);
  }


  getProductbyuserid(id : any):Observable<any[]>{
    return this.http.get<any[]>(`${this.apiUrl}/GetProductsByUserAsync?UsertableId=${id}`);
    
    //https://localhost:7162/api/AdminProduct/GetProductsByUserAsync?UsertableId=39

  }

  addProduct(product: any): Observable<any> {
    // return this.http.post<any>(`${this.apiUrl}/AddProduct`, product);
    return this.http.post<any>(`https://localhost:7162/api/AdminProduct/AddProduct`, product);

  }

  // updateProduct(id: number, product: any): Observable<any> {
  //   console.log("id------",id,product)
  //   return this.http.put<any>(`${this.apiUrl}/UpdateProduct/${id}`, product);
  // }

  updateProduct(id: number, product: any) {
    console.log("id------",id,product)
    // return this.http.put<any>(`${this.apiUrl}/UpdateProduct/${id}`, product);
    const url = `${this.apiUrl}/UpdateProduct/${id}`;
    console.log("url======",url)
    return this.http.put(url,product);
  }

  deleteProduct(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/DeleteProduct/${id}`);
  }
}
