import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../Dtos/LoginRequest';
import { API_BASE_URL } from '../core/config/client-config';
import { AuthResponse } from '../Dtos/AuthResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http:HttpClient){}


  login(request:LoginRequest) : Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${API_BASE_URL}/auth/login`,request);
  }

  setSession(authResponse:AuthResponse){
    localStorage.setItem("token",authResponse.accessToken);
  }

  getToken(){
    return localStorage.getItem("token");
  }
  isLoggedIn(){
    return this.getToken() !=null;
  }

  logout(){
    localStorage.removeItem("token");
  }

  private parseJwt(token:string):any |null{

    const payload=token.split(".")[1];
    const json=atob(payload).replace(/-/g,'+').replace(/_/g,'/');
    return JSON.parse(json);
  

  }

  getUserRoles(){
    const token=this.getToken();
    if(!token) return [];

    const payload=this.parseJwt(token);
    if(!payload) return [];

    const roleClaims=payload.role ?? payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    if(!roleClaims) return [];

    return Array.isArray(roleClaims) ? roleClaims :[roleClaims];
  }

}
