import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginregisterService } from '../../Services/loginregister.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';



@Component({
  selector: 'app-loginregister',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './loginregister.component.html',
  styleUrl: './loginregister.component.css'
})
export class LoginregisterComponent {
  countrydatas: any;
  statedatas: any;
  usertypedata : any;
  Gender:any;
  BloodGroup:any;
  specialization:any;
  IsProvider:boolean = false;
  isLoading:boolean=false;
  isLaunchpage:boolean=true;
  usertypeId:any;



  constructor(private loginregisterService:LoginregisterService, private toastr: ToastrService,
    private router:Router) {}
  // Set maxDate to today's date 
  maxDate = new Date().toISOString().split('T')[0];

  activeForm: string = 'register'; // to toggle between login and register forms

  // loginForm: FormGroup = new FormGroup({
  //   userName: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]),
  //   password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]),
  // });

  forgetpasswordForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$"), Validators.minLength(5), Validators.maxLength(30)]),
  });
  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]),
    email: new FormControl('', [Validators.required, Validators.email, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$"), Validators.maxLength(30)]),
    // usertypeId: new FormControl('', Validators.required),
    dateOfBirth: new FormControl('', Validators.required),
    mobile: new FormControl('', [Validators.required, Validators.pattern("^[0-9]{10}$")]),
    address: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(40)]),
    stateId: new FormControl('', Validators.required),
    countryId: new FormControl('', Validators.required),
    city: new FormControl('', Validators.required),
    pinCode: new FormControl('', [Validators.required, Validators.pattern("^[0-9]{6}$")]),
    genderId: new FormControl('', Validators.required),
    bloodGroupId: new FormControl('', Validators.required),
    qualification: new FormControl(''),
    specializationId: new FormControl(''),
    registerationNo: new FormControl(''),
    visitingCharge: new FormControl('')
  });

  toggleForm(formType: string) {
    //this.loginForm.reset();
    this.registerForm.reset();
    if (formType === 'forgetpassword') {
      this.router.navigate(['/forgetpassword']);
    }    this.activeForm = formType;
    if (this.activeForm === 'register') {
      this.isLaunchpage = true;
      this.BloodGroup();
      this.Gender();
      this.getusertypedata();
      this.getcountrydata();
    }
  }

  showPatientLogin()
  {
    this.isLaunchpage=false;
    this.activeForm='register';
    this.IsProvider=false;
    this.usertypeId='2';
    this.getBloodGroup();
    this.getGenderData();
    this.getusertypedata();
    this.getcountrydata();
  }
  showProviderLogin()
  {
    this.isLaunchpage=false;
    this.activeForm='register';
    this.IsProvider=true;
    this.usertypeId ='1';
    this.getBloodGroup();
    this.getGenderData();
    this.getusertypedata();
    this.getcountrydata();
    this.getSpecializationdata();
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
  


  getusertypedata()
  {
    this.loginregisterService.getUserType().subscribe({
      next: (value:any) =>{
        this.usertypedata = value;
        console.log("usertypedata : ",this.usertypedata);
      }   
    });
  }

  getSpecializationdata()
  {
    this.loginregisterService.getSpecialization().subscribe({
      next: (value:any) =>{
        this.specialization = value;
        console.log("Specialization : ",this.specialization);
      }   
    });
  }

  getGenderData()
  {
    this.loginregisterService.GenderData().subscribe({
      next:(value:any) =>{
        this.Gender = value;
        console.log("GenderData : ",this.Gender);
      }
    })
  }

  getBloodGroup()
  {
    this.loginregisterService.BloodGroupData().subscribe({
      next:(value:any)=>{
        this.BloodGroup = value;
        console.log("BloodGroupData : ",this.BloodGroup);
      }
    })
  }

  getcountrydata()
  {
    this.loginregisterService.getCountry().subscribe({
      next: (value:any) =>{
        this.countrydatas = value;
        console.log("countrydata : ",this.countrydatas);
      }   
    });
  }

  getstatedatabycountryid(id:any)
  {
    const selectedCountryId = parseInt((id.target as HTMLSelectElement).value);
    console.log(selectedCountryId,'selectedCountryId');

    this.loginregisterService.getStateByCountryId(selectedCountryId).subscribe(res => {
      console.log(res, 'status response by country id');
      if (res) {
        this.statedatas = res;
      }else{
        alert('State for selected Country not found');
      }
    });
  }

  selectedFile : File | null=null;  
  
  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onRegisterSubmit() {
    this.isLoading=true;
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.isLoading=false;
      this.toastr.error('Please fill in all required fields correctly.');
      return;
    }
    const formData = new FormData();
      Object.keys(this.registerForm.value).forEach((key) => {
        formData.append(key, this.registerForm.get(key)?.value || '');
      });
      formData.append('profileImage', this.selectedFile!);
      formData.append('usertypeId', this.usertypeId);

    console.log("After registerData", formData)
    console.log("selected file", this.selectedFile)
    this.loginregisterService.register(formData).subscribe({
      next: (res: any) => {
        if (res.statusCode == 200) {
          console.log('Register Form Submitted', res);
          this.registerForm.reset();
          //this.loginForm.reset();
          this.activeForm = 'login';
          this.isLoading=false;
          //this.toastr.success('Registration successful!');
          Swal.fire({
            position: "center",
            icon: "success",
            title: "Registeration Successful!. Check your Email for login details",
            showConfirmButton: false,
            timer: 2500
          });
        }
        else{
          this.isLoading=false;
          this.toastr.error('Registration failed. Email Already Exists');
        }
      },
      error: (err: any) => {
        console.error(err);
        this.isLoading=false;
        this.toastr.error('Registration failed. Email Already Exists.');
      },
    });
  }


  // onLoginSubmit() {
  //   this.isLoading=true;
  //   if (this.loginForm.invalid) {
  //     this.isLoading=false;
  //     this.loginForm.markAllAsTouched();
  //     //this.toastr.error('Please fill in all required fields correctly.');
  //     return;
  //   }
  //   const loginData = this.loginForm.value;
  //   this.loginregisterService.login(loginData.userName, loginData.password).subscribe({
  //     next:(res:any)=>{
  //       console.log(res)
  //       if(res.status==200){
  //         sessionStorage.setItem('Token', res.token);
  //         sessionStorage.setItem('userid',res.data.id)
  //         sessionStorage.setItem('usertypeid',res.data.usertypeId);
  //         sessionStorage.setItem('email',res.data.email);
  //         console.log('Login Form Submitted', loginData);
  //         // this.router.navigate(['verify']);
  //         this.isLoading=false;
  //         this.router.navigateByUrl('/verify');
  //         // Example of what you might do after form submission
  //         this.toastr.success('Verification OTP sent to Mail');
  //       }
  //       else if(res.status == 404)
  //       {
  //         this.toastr.error('Invalid UserName or Password');
  //         this.isLoading=false;
  //       }
  //     }
  //   });
  // }

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
          this.activeForm = 'login';
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
