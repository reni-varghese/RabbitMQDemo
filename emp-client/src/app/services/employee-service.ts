import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../core/config/client-config';
import { Employee } from '../employee';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {

  private http=inject(HttpClient);

  getAll() : Observable<Employee[]>{
    return this.http.get<Employee[]>(`${API_BASE_URL}/employees`);
  }
}
