import { NgFor } from '@angular/common';
import { ParseSourceFile } from '@angular/compiler';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  private fb:FormBuilder=inject(FormBuilder);
  loginForm:FormGroup;
  errorMessage:string='';

  private authService=inject(AuthService);
  private router=inject(Router);

  ngOnInit(){
    
    this.loginForm=this.fb.group({
      email:['',Validators.required],
      password:['',Validators.required]
    })
  }

  handleLogin(){
    this.authService.login(this.loginForm.value).subscribe(
      {
        next: (response)=>{
        this.authService.setSession(response);
        this.router.navigate(["/"]);
        },
        error:(error)=>{
          this.errorMessage="Invalid Credentials";
        }
      }
    )
  }
}
