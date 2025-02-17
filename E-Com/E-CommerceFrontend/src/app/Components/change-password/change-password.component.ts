import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginregisterService } from '../../Services/loginregister.service';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})

export class ChangePasswordComponent {

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
          this.router.navigateByUrl('/dashboard');
        }
        else{
          this.toastr.error('Failed to Change Pasword');
        }
      }

    });
  }



  onCancel(): void {
    this.isModalVisible = false;
    const Id = sessionStorage.getItem('usertypeid');

    if(Id == '2'){
      this.router.navigateByUrl('/dashboard') // Discard changes
    }
    else{
      this.router.navigateByUrl('/products') // Discard changes

    }
  }
}



