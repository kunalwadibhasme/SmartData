import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class sessionService {
    private _session: boolean = false;
  
    getSession(): boolean {
      return this._session;
    }
  
    setSession(value: boolean) {
      this._session = value;
    }
  }