import { Component } from '@angular/core';
import { LoginService } from '../../Services/login.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
 isLoading:boolean=false;
  isLaunchpage:boolean=true;
  usertypedata: any;


  constructor(private loginregisterService:LoginService, private toastr: ToastrService,
    private router:Router) {}
  maxDate = new Date().toISOString().split('T')[0];


  registerForm: FormGroup = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]),
    userTypeId: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$"), Validators.maxLength(30)]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]),
  });

 

  toggleForm(formType: string) {
    this.registerForm.reset();
    if (formType === 'forgetpassword') {
      this.router.navigate(['/forgetpassword']);
    }    
    if (formType === 'login') { 
      this.router.navigate(['/login']);
    }
  }

  

  ngOnInit()
  {
    this.getusertypedata();
    sessionStorage.setItem('isLoggedIn', 'false');
  }

  getusertypedata()
  {
    this.loginregisterService.getUserType().subscribe({
      next: (value:any) =>{
        this.usertypedata = value.data;
        console.log("usertypedata : ",this.usertypedata);
      }   
    });
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
  
  onRegisterSubmit() {
    this.isLoading=true;
    console.log("register data : ", this.registerForm.value);
    if (this.registerForm.invalid) {
      this.isLoading=false;
      this.registerForm.markAllAsTouched();
      //this.toastr.error('Please fill in all required fields correctly.');
      return;
    }
    const loginData = this.registerForm.value;
    this.loginregisterService.register(loginData).subscribe({
      next:(res:any)=>{
        console.log(res)
        if(res.status==200){
          
          console.log('Login Form Submitted', loginData);
          // this.router.navigate(['verify']);
          this.isLoading=false;
          this.router.navigateByUrl('/login');
          // Example of what you might do after form submission
          this.toastr.success('Registeration Successfull');
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


