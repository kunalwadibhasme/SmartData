import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const _router = inject(Router);                         //injecting Router to navigate from one page to another
  let isLoggedIn = sessionStorage.getItem("isLoggedIn");  //variable in sessionstorage to keep track of login or not
  const toaster = inject(ToastrService);

  if(isLoggedIn == 'false')
  {
    //alert("Access Denied, Please Login");
    toaster.warning('Access Denied, Please Login.','Error',{timeOut: 3000})

    _router.navigate(['']);
    return false;
  }
  return true;};
