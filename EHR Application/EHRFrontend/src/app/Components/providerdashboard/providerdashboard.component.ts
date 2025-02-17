import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppointmentService } from '../../Services/appointment.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-providerdashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './providerdashboard.component.html',
  styleUrl: './providerdashboard.component.css'
})
export class ProviderdashboardComponent implements OnInit {
  profileImage: any;

  constructor(private appointmentservice: AppointmentService, private router: Router) {
    this.toastr = inject(ToastrService);

  }
  isFormVisible: boolean = false;
  patients: any[]=[];
  toastr: any;
  timeMin: any;
  minDate: any;
  formdata: any;
  fees: any;
  patientslist: any;
  gotoappointmentdetails: any;
  isEditing: boolean = false;
  ispatientlength: Boolean = false;
  isLoading:boolean=false;



  ngOnInit(): void {
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];
    this.getpatientbyproviderId();


  }

  UserId = sessionStorage.getItem('userid');

  appointmentForm = new FormGroup({
    patientId: new FormControl('', [Validators.required]),
    providerId: new FormControl(this.UserId),
    appointmentDate: new FormControl('', [Validators.required]),
    appointmentTime: new FormControl('', [Validators.required]),
    chiefComplaint: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(200)]),
    status: new FormControl('Scheduled'),
    fees: new FormControl('')
  });

  soapForm = new FormGroup({
    appointmentId: new FormControl(''),
    subjective: new FormControl('', [Validators.required]),
    objective: new FormControl('', [Validators.required]),
    assessment: new FormControl('', [Validators.required]),
    planning: new FormControl('', [Validators.required])
  });

  navigatetochat()
  {
    this.router.navigateByUrl('/chat');
  }

  showAddAppointmentForm(): void {
    this.getPatientData();
    this.isFormVisible = true; // Show form and hide the "Add Appointment" button
  }

  getpatientbyproviderId() {
    this.appointmentservice.getpatientbyproviderId(this.UserId).subscribe({
      next: (value: any) => {
        this.patientslist = value;
        console.log("PatientsList : ", this.patientslist);
        if(this.patientslist.length ==0)
          {
            this.ispatientlength = true;
          }
          else{
            this.ispatientlength = false;
          }
      }
    })
    this.getProviderFees();
  }

  Completeddetails() {
    this.router.navigateByUrl('/completeddetails');
  }
  getPatientData() {
    this.appointmentservice.getPatientData().subscribe({
      next: (value: any) => {
        this.patients = value;
        console.log("Patients Data : ", this.patients);
      }
    });
  }

  getProviderFees() {
    this.appointmentservice.getProviderFees(this.UserId).subscribe({
      next: (value: any) => {
        this.fees = value.visitingCharge;
        console.log("Provider fees : ", this.fees);
      }
    })
  }

  onDateChange(event: any) {
    const selectedDate = event.target.value;
    const today = new Date().toISOString().split('T')[0]; // Today's date in YYYY-MM-DD format

    if (selectedDate === today) {
      // If today is selected, set the time min to current time
      const currentTime = new Date();
      const hours = String(currentTime.getHours()).padStart(2, '0');
      const minutes = String(currentTime.getMinutes()).padStart(2, '0');
      this.timeMin = `${hours}:${minutes}`;
    } else {
      this.timeMin = '00:00'; // Reset timeMin to allow any time for future dates
    }
  }

  onCancel(): void {
    this.appointmentForm.reset();
    this.isFormVisible = false; // Hide form on cancel
  }
  onCancelsoap(): void {
    this.isEditing = false;
  }


  onSubmit(){
    this.isLoading=true;
    this.appointmentForm.value.fees = this.fees;
    if (this.appointmentForm.valid && this.isTimeValid() ) {
      console.log('Form Submitted:', this.appointmentForm.value);
      this.formdata = this.appointmentForm.value;
      this.appointmentservice.AddAppointment(this.formdata).subscribe({
        next: (res: any) => {
          if (res == true) {
            this.getpatientbyproviderId();
            this.isLoading=false;
            this.toastr.success('Appointment successfully added!');
            this.appointmentForm.reset();
            this.isFormVisible = false;
          }
        }
      })
      // Hide form after successful submission
    } else {
      this.isLoading=false;
      this.appointmentForm.markAllAsTouched();
    }
    this.isLoading=false;
  }

  OnCancelAppointment(Id: any) {
    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you really want to cancel this appointment?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, cancel it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.isConfirmed) {
        this.appointmentservice.CancelAppointment(Id).subscribe({
          next: (res: any) => {
            if (res == true) {
              Swal.fire({
                title: 'Success!',
                text: 'Appointment successfully canceled!',
                icon: 'success',
                confirmButtonText: 'OK'
              });
              this.getpatientbyproviderId()
            }
          }
        });
      }
    });
  }
  apiUrl = "https://localhost:7162"

  GoToAppointment(appointmentId: any) {
    this.appointmentservice.GoToAppointment(appointmentId).subscribe({
      next: (res: any) => {
        if (res) {
          this.gotoappointmentdetails = res;
          this.profileImage = this.apiUrl + this.gotoappointmentdetails.profileimage;
          this.isEditing = true;
          console.log('GoToAppointment Details : ', this.gotoappointmentdetails);
        }
      }
    })
  }

  // isTimeValid(): boolean {
  //   const selectedDate = new Date(this.appointmentForm.value.appointmentDate);
  //   const today = new Date();
  //   if (selectedDate.toDateString() === today.toDateString()) {
  //     const selectedTime = this.appointmentForm.value.appointmentTime;
  //     const currentTime = today.getHours() + today.getMinutes() / 60;
  //     const selectedTimeValue = parseInt(selectedTime.split(':')[0]) + parseInt(selectedTime.split(':')[1]) / 60;
  //     return selectedTimeValue > currentTime + 1;
  //   }
  //   return true;
  // }selected

  selectedTime : any;
  appointdate : any;
  isTimeValid(): boolean {
    this.appointdate = this.appointmentForm.value.appointmentDate;
    this.selectedTime = this.appointmentForm.value.appointmentTime;
    const selectedDate = new Date(this.appointdate);
    const today = new Date();
    if (selectedDate.toDateString() === today.toDateString()) {
      const currentTime = today.getHours() + today.getMinutes() / 60;
      const selectedTimeValue = parseInt(this.selectedTime.split(':')[0]) + parseInt(this.selectedTime.split(':')[1]) / 60;
      return selectedTimeValue > currentTime + 1;
    }
    return true;
  }


  onSubmitSoap(appointId: any) {
    if (this.soapForm.valid) {
      this.soapForm.value.appointmentId = appointId;
      this.appointmentservice.SaveSoap(this.soapForm.value).subscribe({
        next: (res: any) => {
          if (res == true) {
            Swal.fire({
              title: 'Success!',
              text: 'Appointment Completed!',
              icon: 'success',
              confirmButtonText: 'OK'
            });
            this.getpatientbyproviderId()
            this.isEditing = false;
            console.log('OnSumbitSoap : ', res);
          }
          else {
            Swal.fire({
              title: 'Failed!',
              text: 'Appointment Failed!',
              icon: 'error',
              confirmButtonText: 'OK'
            });
          }
        }
      })
    }
    else {
      this.soapForm.markAllAsTouched();

    }
  }



}
