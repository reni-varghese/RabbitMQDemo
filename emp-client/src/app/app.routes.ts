import { Routes } from '@angular/router';
import { EmployeeList } from './components/employee-list/employee-list';
import { AddEmployee } from './components/add-employee/add-employee';
import { Login } from './components/login/login';
import { authGuardGuard } from './guards/auth-guard-guard';

export const routes: Routes = [

    {path:"employees",component:EmployeeList,canActivate:[authGuardGuard]},
    {path:"employees/new",component:AddEmployee,canActivate:[authGuardGuard]},
    {path:"login",component: Login},
    {path:"",redirectTo:"/employees",pathMatch:"full"},
    {path:"**",redirectTo:"/employees",pathMatch:"full"}


];
