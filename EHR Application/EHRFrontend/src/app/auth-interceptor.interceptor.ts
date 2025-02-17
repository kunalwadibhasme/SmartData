import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const authToken = sessionStorage.getItem('Token');
  
  const router=inject(Router)
  
  if (authToken) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${authToken}`,
      }
    });
    return next(req)
  }

  // router.navigateByUrl('');
  return next(req)};
