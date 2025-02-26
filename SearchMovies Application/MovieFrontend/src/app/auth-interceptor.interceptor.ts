import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export const authInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const authToken = sessionStorage.getItem('Token');
  console.log(authToken);
  const modifiedReq = authToken
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${authToken}`,
        },
      })
    : req;

  return next(modifiedReq).pipe(
    catchError((error) => {
      if (error.status === 401 || error.status === 403) {
        const router = inject(Router); 
        sessionStorage.clear(); 
        router.navigate(['/login']); 
      }
      return throwError(() => new Error(error.message));
    })
  );
};