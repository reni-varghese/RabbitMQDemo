import { Component, inject } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { AuthService } from '../../../core/auth/services/auth-service';

@Component({
  selector: 'app-main-nav',
  imports: [RouterLink],
  templateUrl: './main-nav.html',
  styleUrl: './main-nav.css',
})
export class MainNav {

  private authService=inject(AuthService);
  private router=inject(Router);

  isLoggedIn(){
    return this.authService.isLoggedIn();
  }
  isAdmin(){
    return this.authService.getUserRoles()
      .includes('Admin');
  }

  logout(){
    this.authService.logout();
    this.router.navigate(["/login"])
  }

}
