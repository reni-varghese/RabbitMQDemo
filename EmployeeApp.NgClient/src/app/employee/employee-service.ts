import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/config';
import { Employee } from './employee';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {


  private http:HttpClient=inject(HttpClient)

  getAll():Observable<Employee[]>{

    return  this.http.get<Employee[]>(`${API_BASE_URL}/employees`)

  }



}
