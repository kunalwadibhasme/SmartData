import { Component, Input } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { SharedserviceService } from '../../Services/sharedservice.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navigationbar',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './navigationbar.component.html',
  styleUrl: './navigationbar.component.css'
})
export class NavigationbarComponent {
  totalQuantity: number = 0;
  typeid : boolean = false;

  constructor(private Sharedservice: SharedserviceService, private router: Router) {
    const usertypeid = sessionStorage.getItem('usertypeid');
    if(usertypeid =='1')
    {
      this.typeid = true;
    }
   }

  ngOnInit() {
    this.Sharedservice.totalQuantity$.subscribe(quantity => {
      this.totalQuantity = quantity;
    });
  }

  backtoFirstpage()
  {
    const Id = sessionStorage.getItem('usertypeid');

    if(Id == '2'){
      this.router.navigateByUrl('/dashboard') // Discard changes
    }
    else{
      this.router.navigateByUrl('/products') // Discard changes

    }

  }

  Logout() {
    sessionStorage.clear();
    this.router.navigateByUrl('/login')
  }

  profile(){
    this.router.navigateByUrl('/userprofile')
  }

  productdetail()
  {
    this.router.navigateByUrl('/cartDetail');
  }

  Change() {
    this.router.navigateByUrl('/changepassword');
  }
}
