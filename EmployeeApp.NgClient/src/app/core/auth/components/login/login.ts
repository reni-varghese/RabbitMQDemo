import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {

  loginForm:any;
  private fb:FormBuilder=inject(FormBuilder);
  private authService=inject(AuthService);
  private router=inject(Router);

errorMessage=signal('');

  ngOnInit(): void {
    this.loginForm=this.fb.group({
      email:['',Validators.required],
      password:['',Validators.required]
    })
  }

  handleLogin(){
    console.log(this.loginForm.value);

    this.authService.login(this.loginForm.value)
    .subscribe({
      next:(response)=>{
        console.log(response)
        this.authService.setSession(response);
        this.router.navigate(["/"])
      }
      ,error:(err)=>{
        this.errorMessage.set("Invalid Credentials")
      }
    })
    
  }
}
