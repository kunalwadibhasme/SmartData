import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'E-CommerceFrontend';

  constructor(private toastr: ToastrService) {}

  onSubmit() {
    this.toastr.success('Hello world!', 'Toastr fun!');
  }


  
}

