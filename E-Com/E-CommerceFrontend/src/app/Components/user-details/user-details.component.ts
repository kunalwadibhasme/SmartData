import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginregisterService } from '../../Services/loginregister.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.css'
})
export class UserDetailsComponent {
  isEditing: boolean = false; // Toggle state for forms
  viewForm!: FormGroup; // Form to display user details
  statedatas: any;
  countrydatas: any;
  usertypedata: any;
  Userdata: any;
  formType: 'view' | 'edit' = 'view';
  maxDate = new Date().toISOString().split('T')[0];



  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]),
    email: new FormControl('', [Validators.required, Validators.email, Validators.minLength(5), Validators.maxLength(30)]),
    usertypeId: new FormControl('', Validators.required),
    dateOfBirth: new FormControl('', Validators.required),
    mobile: new FormControl('', [Validators.required, Validators.pattern("^[0-9]{10}$")]),
    address: new FormControl('', [Validators.required, Validators.minLength(5)]),
    zipCode: new FormControl('', [Validators.required, Validators.pattern("^[0-9]{6}$")]),
    stateId: new FormControl('', Validators.required),
    countryId: new FormControl('', Validators.required),
  });
  constructor(private fb: FormBuilder, private router: Router,
    private loginregisterService: LoginregisterService,
    private toastr: ToastrService) {
    const Id = sessionStorage.getItem('userid');
    console.log("Id====", Id);
    this.getusertypedata();
    this.countrydatas = this.getcountrydata();
    this.getUserdatabyid(Id);
  }

  ngOnInit(): void {
    this.registerForm.patchValue(this.Userdata) // View-only form
    
  }

  base64Image: string = '';
  profileImage: any;
  apiUrl = "https://localhost:7162"
  getUserdatabyid(id: any) {
    this.loginregisterService.GetuserbyId(id).subscribe({
      next: (value: any) => {
        this.Userdata = value;
        console.log("userdata : ", this.Userdata);
        this.profileImage = this.apiUrl + value.profileimage;
      }
    });
  }
  onEdit() {
    this.getusertypedata();
    this.getcountrydata();
    this.isEditing = true;
    console.log("Userdata before Edit Patch ===", this.Userdata);
    this.registerForm.patchValue(this.Userdata); // Patch values to edit form
  }
  omit_special_char(event : any)
  {   
     var k;  
     k = event.keyCode;  //         k = event.keyCode;  (Both can be used)
     if(k >= 48 && k <= 57)
      {
        return true
      }
      else
      {
        
        //this.paymentForm.get('cvv')?.markAsTouched()
        return false
      } 
  }


  selectedFile: File | null = null; // To store the Base64 image 

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      // const reader = new FileReader();
      // reader.onload = () => {
      //   this.base64Image = reader.result as string; // Store Base64 string in a variable
      // };
      // reader.readAsDataURL(file);
    }
  }
  onSave() {

    console.log("After Edit Data ==", this.registerForm);
    //this.registerForm.patchValue(this.registerForm.value); // Save changes
    // Switch back to view 
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.toastr.error('Please fill in all required fields correctly.');
    }
    const UpdateData = new FormData();
    Object.keys(this.registerForm.value).forEach((key) => {
      UpdateData.append(key, this.registerForm.get(key)?.value || '');
    });
    UpdateData.append('profileimage', this.selectedFile!);

    this.loginregisterService.Update(UpdateData).subscribe({
      next: (res: any) => {
        console.log("Update Response : ", res);
        if (res.status === 200) {
          console.log('Data Updated Successfully', res);
          this.registerForm.reset();
          this.toastr.success('Data Updated!');
          const Id = sessionStorage.getItem('usertypeid');

          if (Id == '2') {
            this.router.navigateByUrl('/dashboard') // Discard changes
          }
          else {
            this.router.navigateByUrl('/products') // Discard changes

          }
          this.isEditing = false;
          // this.registerForm.patchValue(this.Userdata)
        }
      },
      error: (err: any) => {
        console.error(err);
        this.registerForm.markAllAsTouched();
        this.toastr.error('Edit Failed');
      },
    });
  }

  onCancel() {
    this.isEditing = false; // Discard changes
  }

  onCancelv() {
    const Id = sessionStorage.getItem('usertypeid');

    if (Id == '2') {
      this.router.navigateByUrl('/dashboard') // Discard changes
    }
    else {
      this.router.navigateByUrl('/products') // Discard changes

    }
  }

  getusertypedata() {
    this.loginregisterService.getUserType().subscribe({
      next: (value: any) => {
        this.usertypedata = value;
        console.log("usertypedata : ", this.usertypedata);
      }
    });
  }

  getcountrydata() {
    this.loginregisterService.getCountry().subscribe({
      next: (value: any) => {
        this.countrydatas = value;
        console.log("countrydata : ", this.countrydatas);
      }
    });
  }

  getstatedatabycountryid(id: any) {
    const selectedCountryId = parseInt((id.target as HTMLSelectElement).value);
    console.log(selectedCountryId, 'selectedCountryId');

    this.loginregisterService.getStateByCountryId(selectedCountryId).subscribe(res => {
      console.log(res, 'status response by country id');
      if (res) {
        this.statedatas = res;
      } else {
        alert('State for selected Country not found');
      }
    });
  }

}
