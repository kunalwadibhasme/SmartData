import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginregisterService } from '../../Services/loginregister.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verifyotp',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './verifyotp.component.html',
  styleUrls: ['./verifyotp.component.css']
})
export class VerifyotpComponent implements OnInit {
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
          this.toastr.success('OTP verified successfully!'); // Add your verification logic here
          const usertypeid = sessionStorage.getItem('usertypeid');
          this.isLoading=false;
          if(usertypeid == '2' )
          {
            this.router.navigateByUrl('/dashboard');

          }
          else{
            this.router.navigateByUrl('/products');
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
