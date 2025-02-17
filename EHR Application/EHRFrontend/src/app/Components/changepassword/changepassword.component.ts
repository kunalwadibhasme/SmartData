import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginregisterService } from '../../Services/loginregister.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-changepassword',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './changepassword.component.html',
  styleUrl: './changepassword.component.css'
})
export class ChangepasswordComponent {
  isModalVisible: boolean = true;
  showPassword: boolean = false;

  constructor(private router: Router, private toastr: ToastrService, private loginregisterService:LoginregisterService) { }

  ngOnInit(): void {
  }
  changeForm: FormGroup = new FormGroup({
    newpassword: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]),
    confirmpassword: new FormControl('', Validators.required)
  });

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
    }
    


  // passwordMatchValidator(form: FormGroup) {
  //   return form.get('newpassword').value === form.get('confirmpassword').value
  //     ? null : { 'mismatch': true };
  // }

  onSave(): void {
    if (this.changeForm.invalid) {
      this.changeForm.markAllAsTouched();
      return;
    }
    if(this.changeForm.value.newpassword !== this.changeForm.value.confirmpassword)
    {
      this.toastr.error('New Password and Confirm Password must be same')
      return;
    }

    const newPassword = this.changeForm.value.newpassword;
    const email = sessionStorage.getItem('email'); // Replace with the actual email
    this.loginregisterService.sendChangePasswordRequest(email, newPassword).subscribe({
      next: (value:any)=> {
        if(value.status == 200)
        {
          this.toastr.success('Password Changed Successfully');
          this.isModalVisible = false;

          const usertypeid = sessionStorage.getItem('usertypeid');
          if(usertypeid == '2' )
            {
              this.router.navigateByUrl('/patientdashboard');
  
            }
            else{
              this.router.navigateByUrl('/providerdashboard');
            }
          }
        else{
          this.toastr.error('Failed to Change Pasword');
        }
      }

    });
  }



  onCancel(): void {
    this.isModalVisible = false;
    const usertypeId = sessionStorage.getItem('usertypeid');
    if(usertypeId==='2')
    {
      this.router.navigateByUrl('/patientdashboard');
    }
  }
}
