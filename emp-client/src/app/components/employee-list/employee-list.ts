import { Component, inject } from '@angular/core';
import { Employee } from '../../employee';
import { HttpClient } from '@angular/common/http';
import { EmployeeService } from '../../services/employee-service';

@Component({
  selector: 'app-employee-list',
  imports: [],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css',
})
export class EmployeeList {

  employees:Employee[]=[];

  private employeeService=inject(EmployeeService)

  ngOnInit(){
    this.employeeService.getAll().subscribe({
      next: (data)=>{
        this.employees=data;
      },
      error:(err)=>{
        console.log("Something went wrong");
      }

    })
  }
  

}
