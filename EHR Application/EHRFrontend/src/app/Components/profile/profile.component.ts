// profile.component.ts
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginregisterService } from '../../Services/loginregister.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports:[CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent{
  isEditing: boolean = false; // Toggle state for forms
  viewForm!: FormGroup; // Form to display user details
  statedatas: any;
  countrydatas: any;
  usertypedata: any;
  Userdata: any;
  profileImage: any ; // Replace with actual image path
  country: any;
  bloodGroup: any;
  qualification: any;
  specialization: any;
  gender: any;
  isProvider:boolean=false;
  usertypeId:any;
  isLoading:boolean=false;

  
  

  // Calculate age from date of birth
 
  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]),
    email: new FormControl('', [Validators.required, Validators.email, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$"), Validators.maxLength(30)]),
    usertypeId: new FormControl(1),
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

  formType: 'view' | 'edit' = 'view';
  maxDate = new Date().toISOString().split('T')[0];
  constructor(private fb: FormBuilder, private router: Router,
    private loginregisterService: LoginregisterService,
    private toastr: ToastrService) {
    const Id = sessionStorage.getItem('userid');
    console.log("Id====", Id);
    this.getusertypedata();
    this.countrydatas = this.getcountrydata();
    this.getBloodGroup();
    this.getGenderData();
    this.getusertypedata();
    this.getcountrydata();
    this.getSpecializationdata();    
    this.getUserdatabyid(Id);
    
  }

  ngOnInit(): void {
    this.isLoading=true;
    //this.registerForm.patchValue(this.Userdata)
    this.isLoading=false;
    const usertype = sessionStorage.getItem('usertypeid')
    if(usertype ==="1")
    {
      this.isProvider=true;
    }
    else{
      this.isProvider = false;
    }
  }
  
  apiUrl = "https://localhost:7162"


  getUserdatabyid(id: any) {
    this.isLoading=true;
    this.loginregisterService.GetuserbyId(id).subscribe({
      next: (value: any) => {
        this.Userdata = value;
        console.log("userdata : ", this.Userdata);
        this.profileImage = this.apiUrl + value.profileimage;
        this.bloodGroup = this.bloodGroupsMap[this.Userdata.bloodGroupId];
        this.gender = this.GenderMap[this.Userdata.genderId];
        this.Specialization = this.SpecializationMap[this.Userdata.specializationId];
        this.country = this.CountryMap[this.Userdata.countryId];
        this.usertypeId = this.Userdata.usertypeId
      }
    });
    this.isLoading=false;
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

  getSpecializationdata()
  {
    this.loginregisterService.getSpecialization().subscribe({
      next: (value:any) =>{
        this.specialization = value;
        console.log("Specialization : ",this.specialization);
        this.SpecializationMap = this.specialization.reduce((map: any, group: any) => {
          map[group.id] = group.speciality;
          return map;
        }, {});
      }   
    });
  }
  SpecializationMap: { [key: number]: string } = {}; // To store BloodGroupData as a map
  Specialization: any;

  getGenderData()
  {
    this.loginregisterService.GenderData().subscribe({
      next:(value:any) =>{
        this.Gender = value;
        console.log("GenderData : ",this.Gender);

        this.GenderMap = this.Gender.reduce((map: any, group: any) => {
          map[group.id] = group.gender;
          return map;
        }, {});
      }
    })
  }
  GenderMap: { [key: number]: string } = {}; // To store BloodGroupData as a map
  Gender: any[] = [];

 
bloodGroupsMap: { [key: number]: string } = {}; // To store BloodGroupData as a map
BloodGroup: any[] = []; // To store BloodGroup array data

getBloodGroup() {
  this.loginregisterService.BloodGroupData().subscribe({
    next: (value: any) => {
      this.BloodGroup = value;
      console.log("BloodGroupData: ", this.BloodGroup);

      // Create a map for quick lookup
      this.bloodGroupsMap = this.BloodGroup.reduce((map: any, group: any) => {
        map[group.id] = group.bloodgroup;
        return map;
      }, {});
    },
  });
}

  getusertypedata() {
    this.loginregisterService.getUserType().subscribe({
      next: (value: any) => {
        this.usertypedata = value;
        console.log("usertypedata : ", this.usertypedata);
      }
    });
  }

  CountryMap: { [key: number]: string } = {}; // To store BloodGroupData as a map
  Country: any[] = [];

  getcountrydata() {
    this.loginregisterService.getCountry().subscribe({
      next: (value: any) => {
        this.countrydatas = value;
        console.log("countrydata : ", this.countrydatas);

        this.CountryMap = this.countrydatas.reduce((map: any, group: any) => {
          map[group.countryId] = group.name;
          return map;
        }, {});
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

  onCancel() {
    this.isEditing = false; // Discard changes
  }

  onCancelv() {
    const usertypeId = sessionStorage.getItem('usertypeid');
    if(usertypeId==='2')
    {
      this.router.navigateByUrl('/patientdashboard');
    }
    else{
      this.router.navigateByUrl('/providerdashboard');
    }
  }

  selectedFile: File | null = null; // To store the Base64 image 

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
     
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
    



    this.loginregisterService.Update(this.registerForm.value).subscribe({
      next: (res: any) => {
        console.log("Update Response : ", res);
        if (res.status === 200) {
          console.log('Data Updated Successfully', res);
          this.registerForm.reset();
          Swal.fire({
            title: 'Success!',
            text: 'Profile Updated!',
            icon: 'success',
            showConfirmButton: false,
            timer: 2000
          });
          const Id = sessionStorage.getItem('usertypeid');

          if (Id == '2') {
            this.router.navigateByUrl('/patientdashboard') // Discard changes
          }
          else {
            this.router.navigateByUrl('/providerdashboard') // Discard changes

          }
          this.isEditing = false;
          // this.registerForm.patchValue(this.Userdata)
        }
      },
      error: (err: any) => {
        console.error(err);
        this.registerForm.markAllAsTouched();
        Swal.fire({
            icon: 'error',
            title: 'Updates Failed',
            text: 'Fill all the Required Filled',
            showConfirmButton: false,
            timer: 2000
          })      },
    });
  }
  
  onEdit(){
    this.getBloodGroup();
    this.getGenderData();
    this.getusertypedata();
    this.getcountrydata();
    this.getSpecializationdata();
    const dob = this.Userdata.dateOfBirth;
    const dateObj = new Date(dob);
    this.Userdata.dateOfBirth = dateObj.toLocaleDateString('en-CA');
    console.log("Formatted date is : ",this.Userdata.dateOfBirth);
    this.registerForm.patchValue(this.Userdata);
    this.isEditing = true;
  }


}
