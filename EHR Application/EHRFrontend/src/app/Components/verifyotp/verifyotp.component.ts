import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginregisterService } from '../../Services/loginregister.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-verifyotp',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './verifyotp.component.html',
  styleUrl: './verifyotp.component.css'
})
export class VerifyotpComponent {

  showOtpPopup: boolean = true;
  otpForm!: FormGroup;
  isLoading:boolean=false;


  constructor(
    private fb: FormBuilder, private toastr: ToastrService,
    private loginregisterService: LoginregisterService, private router: Router) { }

  ngOnInit(): void {
    this.otpForm = this.fb.group({
      email: new FormControl(sessionStorage.getItem('email'), [Validators.required, Validators.email]),
      otp: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{6}$')])
    });

    console.log("otp: ", this.otpForm.value.email);
  }

  get otp() {
    return this.otpForm.get('otp');
  }

  onSubmit() {
    this.isLoading=true;
    if (this.otpForm.invalid) {
      this.isLoading=false;
      this.toastr.error('Incorrect OTP');
      return;
    }

    const verify = this.otpForm.value;
    this.loginregisterService.Otp(this.otpForm.value.email, this.otpForm.value.otp).subscribe({
      next: (res: any) => {
        if (res.status === 200) {
          sessionStorage.setItem('isLoggedIn','true');
          Swal.fire({
            icon: 'success',
            title: 'Logined',
            text: 'Login Successful!',
            showConfirmButton: false,
            timer: 2000
          })
           const usertypeid = sessionStorage.getItem('usertypeid');
          this.isLoading=false;
          if(usertypeid == '2' )
          {
            this.router.navigateByUrl('/patientdashboard');

          }
          else{
            this.router.navigateByUrl('/providerdashboard');
          }
          this.showOtpPopup = false; // Hide popup after verification
        }
        else{
          this.isLoading=false;
          this.toastr.error('Incorrect OTP');
        }
      },
      error: (err: any) => {
        this.isLoading=false;
        this.toastr.error('Failed to verify OTP');
        console.error(err);
      }
    });
  }
}
