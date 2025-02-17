import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginregisterService {

  constructor(private http : HttpClient) { }

  private baseUrl = 'https://localhost:7162/api/LoginRegister'; 

  getUserType(): Observable<any> 
  { 
    return this.http.get<any>(`${this.baseUrl}/UserType`); 
  }

  getCountry(): Observable<any> 
  { 
    return this.http.get<any>(`${this.baseUrl}/CountryList`); 
  } 

  getStateByCountryId(id: any): Observable<any> 
  { 
    return this.http.get<any>(`${this.baseUrl}/StateList?byCountryId=${id}`); 
  }

  GetuserbyId(id : any): Observable<any>
  {
    return this.http.get<any>(`${this.baseUrl}/GetUserById?Id=${id}`);
    // https://localhost:7162/api/LoginRegister/GetUserById?Id=
  }

  login(userName: string, password: string): Observable<any> 
  { 
    const body = { userName, password };
    return this.http.post<any>(`${this.baseUrl}/LoginUser`, body, { responseType: 'json' }); 
  }

  ForgetPassword(email : string): Observable<any>
  {
    return this.http.put<any>(`${this.baseUrl}/ForgetPassword?email=${email}`, { responseType: 'json' });
  }
  // https://localhost:7162/api/LoginRegister/ForgetPassword

  register(data: any): Observable<any> 
  { 
    return this.http.post<any>(`${this.baseUrl}/RegisterUser`, data); 
  }
  
  Update(data : any): Observable<any>
  {
    return this.http.put<any>(`${this.baseUrl}/UpdateUser`, data);
  }

  Otp(email : string, otp:string):Observable<any>
  {
    const body ={email, otp};
    return this.http.post<any>(`${this.baseUrl}/Verifyotp`, body, { responseType: 'json' }); 
  }
  sendChangePasswordRequest(email : any, password : string): Observable<any>
  {
    const body = {email, password};
    return this.http.post<any>(`${this.baseUrl}/ChangePassword`, body, { responseType:'json' })
  }
}
