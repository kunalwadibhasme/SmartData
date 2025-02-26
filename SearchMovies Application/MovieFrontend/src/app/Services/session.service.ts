import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  private _session: boolean = false;
  
    getSession(): boolean {
      return this._session;
    }
  
    setSession(value: boolean) {
      this._session = value;
    }
}
