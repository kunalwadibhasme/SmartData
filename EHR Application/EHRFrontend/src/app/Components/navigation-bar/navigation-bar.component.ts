import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-navigation-bar',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './navigation-bar.component.html',
  styleUrl: './navigation-bar.component.css'
})
export class NavigationBarComponent {

constructor(private router:Router){}

  profile(){
    this.router.navigateByUrl('/profile')
  }

  changepassword()
  {
    this.router.navigateByUrl('/changepassword');
  }

  OnHome()
  {
    const usertypeId = sessionStorage.getItem('usertypeid');
    if(usertypeId==="2")
    {
      this.router.navigateByUrl('/patientdashboard');
    }
    else{
      this.router.navigateByUrl('/providerdashboard');

    }
  }
  Logout()
  {
    sessionStorage.clear();
    this.router.navigateByUrl('/');
  }
}
