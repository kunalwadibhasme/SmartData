// import { inject } from '@angular/core';
// import { CanActivateFn, Router } from '@angular/router';
// import { JwtHelperService } from '@auth0/angular-jwt';
// import { ToastrService } from 'ngx-toastr';

// export const authGaurdGuard: 
// CanActivateFn = (route, state) => {

//   const _router = inject(Router);  
//   const toaster = inject(ToastrService);
//   const jwtHelper = inject(JwtHelperService);  

//   let isLoggedIn = sessionStorage.getItem("isLoggedIn");  //variable in sessionstorage to keep track of login or not
//   if(isLoggedIn == 'false')
//   {
//     //alert("Access Denied, Please Login");
//     toaster.warning('Access Denied, Please Login.','Error',{timeOut: 3000})
//     _router.navigate(['/login']);
//   }

//   const token = localStorage.getItem("Token");
//   if(token && !jwtHelper.isTokenExpired(token))
//   {
//     console.log(jwtHelper.decodeToken(token))
//     return true;
//   }

//   const isRefreshSuccess = await this.tryRefreshingTokens(token);
//   if(!isRefreshSuccess){
//     _router.navigate(['/login']);
//   }
//   return isRefreshSuccess;

// };

// private async tryRefreshingTokens(token: string): Promise<boolean> {
//   const refreshToken: string = localStorage.getItem("refreshToken");
//   if (!token || !refreshToken) { 
//     return false;
//   }

//   const credentials = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
//   let isRefreshSuccess: boolean;

//   const refreshRes = await new Promise<AuthenticatedResponse>((resolve, reject) => {
//     this.http.post<AuthenticatedResponse>("https://localhost:5001/api/token/refresh", credentials, {
//       headers: new HttpHeaders({
//         "Content-Type": "application/json"
//       })
//     }).subscribe({
//       next: (res: AuthenticatedResponse) => resolve(res),
//       error: (_) => { reject; isRefreshSuccess = false;}
//     });
//   });

//   localStorage.setItem("jwt", refreshRes.token);
//   localStorage.setItem("refreshToken", refreshRes.refreshToken);
//   isRefreshSuccess = true;

//   return isRefreshSuccess;
// }



import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export const authGaurdGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);
  const jwtHelper = inject(JwtHelperService);
  const http = inject(HttpClient);

  const isLoggedIn = sessionStorage.getItem('isLoggedIn');

  if (isLoggedIn === 'false') {
  console.log("if------")
    toastr.warning('Access Denied, Please Login.', 'Error', { timeOut: 3000 });
    router.navigate(['/login']);
    return false;
  }

  else{
    const token = sessionStorage.getItem('Token');
  if (token && !jwtHelper.isTokenExpired(token)) {
    console.log(jwtHelper.decodeToken(token));
    return true;
  }

  return tryRefreshingTokens(token!, http).pipe(
    map((isRefreshSuccess) => {
      if (!isRefreshSuccess) {
        router.navigate(['/login']);
      }
      return isRefreshSuccess;
    }),
    catchError(() => {
      router.navigate(['/login']);
      return of(false);
    })
  );
  }
};

const tryRefreshingTokens = (accessToken: string, http: HttpClient): Observable<boolean> => {
  const refreshToken = sessionStorage.getItem('refreshToken');
  if (!accessToken || !refreshToken) {
    return of(false);
  }

  const credentials = JSON.stringify({ accessToken, refreshToken });
  return http.post<any>('https://localhost:7176/api/Token', credentials).pipe(
    map((res) => {
      sessionStorage.setItem('Token', res.accessToken);
      sessionStorage.setItem('refreshToken', res.refreshToken);
      return true;
    }),
    catchError(() => of(false))
  );
};


