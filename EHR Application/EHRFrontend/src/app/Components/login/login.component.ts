import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginregisterService } from '../../Services/loginregister.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  
  
  IsProvider:boolean = false;
  isLoading:boolean=false;
  isLaunchpage:boolean=true;
  usertypeId:any;



  constructor(private loginregisterService:LoginregisterService, private toastr: ToastrService,
    private router:Router) {}
  // Set maxDate to today's date 
  maxDate = new Date().toISOString().split('T')[0];

 // activeForm: string = 'register'; // to toggle between login and register forms

  loginForm: FormGroup = new FormGroup({
    userName: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]),
  });

 

  toggleForm(formType: string) {
    this.loginForm.reset();
    if (formType === 'forgetpassword') {
      this.router.navigate(['/forgetpassword']);
    }    
    if (formType === 'register') { 
      this.router.navigate(['/login']);
    }
  }

  

  ngOnInit()
  {
    sessionStorage.setItem('isLoggedIn', 'false');
  }

  goBack() {
    // Logic to go back to the previous page
    this.isLaunchpage=true;
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
  
  onLoginSubmit() {
    this.isLoading=true;
    if (this.loginForm.invalid) {
      this.isLoading=false;
      this.loginForm.markAllAsTouched();
      //this.toastr.error('Please fill in all required fields correctly.');
      return;
    }
    const loginData = this.loginForm.value;
    this.loginregisterService.login(loginData.userName, loginData.password).subscribe({
      next:(res:any)=>{
        console.log(res)
        if(res.status==200){
          sessionStorage.setItem('Token', res.token);
          sessionStorage.setItem('userid',res.data.id)
          sessionStorage.setItem('usertypeid',res.data.usertypeId);
          sessionStorage.setItem('email',res.data.email);
          console.log('Login Form Submitted', loginData);
          // this.router.navigate(['verify']);
          this.isLoading=false;
          this.router.navigateByUrl('/verify');
          // Example of what you might do after form submission
          this.toastr.success('Verification OTP sent to Mail');
        }
        else if(res.status == 404)
        {
          this.toastr.error('Invalid UserName or Password');
          this.isLoading=false;
        }
      }
    });
  }
}

