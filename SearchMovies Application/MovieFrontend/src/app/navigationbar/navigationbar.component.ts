import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-navigationbar',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './navigationbar.component.html',
  styleUrl: './navigationbar.component.css'
})
export class NavigationbarComponent {
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
