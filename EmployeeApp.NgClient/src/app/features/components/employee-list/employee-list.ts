import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { Employee } from '../../../employee/employee';
import { EmployeeService } from '../../../employee/employee-service';

@Component({
  selector: 'app-employee-list',
  imports: [],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css',
})
export class EmployeeList {

  private service=inject(EmployeeService);
  employees=signal<Employee[]>([]);

  ngOnInit(){

    this.service.getAll()
    .subscribe({
      next:(data)=>{
        this.employees.set(data);
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }

  
}
