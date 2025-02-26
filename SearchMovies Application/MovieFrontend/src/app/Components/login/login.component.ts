import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { LoginService } from '../../Services/login.service';
import { JwtHelperService } from '@auth0/angular-jwt';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  
  isLoading:boolean=false;
  isLaunchpage:boolean=true;



  constructor(private loginregisterService:LoginService, private toastr: ToastrService,
    private router:Router, private jwtHelper: JwtHelperService) {}
  // Set maxDate to today's date 
  maxDate = new Date().toISOString().split('T')[0];

 // activeForm: string = 'register'; // to toggle between login and register forms

  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$"), Validators.maxLength(30)]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]),
  });

 

  toggleForm(formType: string) {
    this.loginForm.reset();
    if (formType === 'forgetpassword') {
      this.router.navigate(['/forgetpassword']);
    }    
    if (formType === 'register') { 
      this.router.navigate(['/register']);
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
    this.loginregisterService.login(loginData.email, loginData.password).subscribe({
      next:(res:any)=>{
        console.log(res)
        if(res.status==200){
          const decodedToken = this.jwtHelper.decodeToken(res.token);
          console.log("Decoded token is :" ,decodedToken);
          sessionStorage.setItem("ApiKey", decodedToken.Api);
          sessionStorage.setItem('Token', res.token);
          sessionStorage.setItem('refreshToken',res.refreshToken);
          sessionStorage.setItem('usertypeid',res.role.userTypeId);
          sessionStorage.setItem('isLoggedIn','true');
          console.log('Login Form Submitted', loginData);
          
          this.isLoading=false;
          if(res.role.userTypeId == 1)
          {
            this.router.navigateByUrl('/dashboard');
          }
          else{
            this.router.navigateByUrl('/landingpage');

          }
          this.toastr.success('Successfully Logged In');
        }
        else if(res.status == 401)
        {
          this.isLoading=false;
          this.toastr.error('Invalid UserName or Password');
        }
      }
    });
  }
}


