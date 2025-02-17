import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { LoginregisterService } from '../../Services/loginregister.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgetpassword',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './forgetpassword.component.html',
  styleUrl: './forgetpassword.component.css'
})
export class ForgetpasswordComponent {

  isLoading:boolean=false;
  isLaunchpage:boolean=true;

  constructor(private loginregisterService:LoginregisterService, private toastr: ToastrService,
    private router:Router) {}
  
  forgetpasswordForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$"), Validators.minLength(5), Validators.maxLength(30)]),
  });
 

  toggleForm(formType: string) {
    this.forgetpasswordForm.reset();
    if (formType === 'register') {
      this.router.navigate(['/login']);
    }
    if (formType === 'login') {
      this.router.navigate(['/login1']);
    }
  }

  
  ngOnInit()
  {
    sessionStorage.setItem('isLoggedIn', 'false');
  }

  goBack() {
    // Logic to go back to the previous page
    this.router.navigateByUrl('/login1');
}


  omit_special_char(event: any) {
    var k;
    k = event.keyCode;  //         k = event.keyCode;  (Both can be used)
    if (k >= 48 && k <= 57) {
      return true
    }
    else {
      return false
    }
  }

  omit_special_char_alphabets(event: any) {
    var k;
    k = event.keyCode || event.which; 
    // Check for alphabets (A-Z and a-z)
    if ((k >= 65 && k <= 90) || (k >= 97 && k <= 122)) {
      return true;
    } else {
      return false;
    }
  }
  
  onForgetPasswordSubmit(){
    this.isLoading=true;
    console.log("this====",this.forgetpasswordForm.value);
    if(this.forgetpasswordForm.invalid){
      this.isLoading=false;
      this.forgetpasswordForm.markAllAsTouched();
      //this.toastr.error('Please fill the Email Address Correctly');
      return;
    }
    this.loginregisterService.ForgetPassword(this.forgetpasswordForm.value.email).subscribe({
      next:(res:any)=>{
        if(res.status == 200){
          this.isLoading=false;
          this.toastr.success('New Password Sent Successfully on your mail');
          this.router.navigate(['/login1']);
          this.forgetpasswordForm.reset();
        }
        else{
          this.isLoading=false;
          this.toastr.error('Incorrect Mail mail');
        }
      }
    })
  }
}



