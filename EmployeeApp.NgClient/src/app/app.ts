import { Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MainNav } from "./shared/components/main-nav/main-nav";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MainNav],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
 
}
