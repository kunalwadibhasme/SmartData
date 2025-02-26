import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  

  constructor(private http : HttpClient) { }
  private baseUrl='https://localhost:7176/api/User';

  private baseUrl2 = 'https://localhost:7176/api/Movie';
  login(email: string, password: string): Observable<any> 
  { 
    const body = { email, password };
    return this.http.post<any>(`${this.baseUrl}/login`, body); 
  }

  register(loginData: any) {
    return this.http.post<any>(`${this.baseUrl}/register`, loginData); 
  }

  getUserType(): Observable<any>
  {
    return this.http.get<any>(`${this.baseUrl}/Usertype`);
  }
  AddMovie(data : any): Observable<any>
  {
    return this.http.post<any>(`${this.baseUrl2}/AddMovie`,data)
  }
  update(id : any, data : any) : Observable<any>
  {
    return this.http.put<any>(`${this.baseUrl2}/UpdateMovie?Id=${id}`,data)
  }

  GetMovies(searchname : any, apikey: any): Observable<any>
  {
    return this.http.get<any>(`${this.baseUrl2}/GetAllMovies?search=${searchname}&apikey=${apikey}`);
  }

  Deletemovie(id : any)
  {
    return this.http.delete<any>(`${this.baseUrl2}/DeleteMovie?movieid=${id}`);
  }

  GetAllMovie()
  {
    return this.http.get<any>(`${this.baseUrl2}/GetMovie`)
  }
}

