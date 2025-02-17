import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AppointmentService } from '../../Services/appointment.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-completedappointment',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './completedappointment.component.html',
  styleUrl: './completedappointment.component.css'
})
export class CompletedappointmentComponent implements OnInit{
  toastr: ToastrService;
  ViewpatientDetails: any;
  ngOnInit(): void {
      this.Completedpatientdetails();

  }
constructor(private appointmentservice: AppointmentService) {
    this.toastr = inject(ToastrService);

  }

  patientDetails : any[] =[];
  providerdetails : any;
  isViewing : boolean = false;
  apiUrl = "https://localhost:7162"
  profileImage:any;
  UserId = sessionStorage.getItem('userid');
  isuser: boolean = false;
  islength: boolean = false;


  

  Completedpatientdetails()
  {
    this.appointmentservice.Completedpatientdetails(this.UserId).subscribe({
      next:(value:any) =>{
        console.log("Completed Patient Details", value)
        this.patientDetails = value;
        if(this.patientDetails.length==0)
          {
            this.islength = true;
          }
          else{
            this.islength = false;
          }

      }
    });
  }
  GoToPatient(appointmentid: any) {
    this.isViewing = true;
    
    // Filter the list of patientsDetails to get the ones with the matching appointmentId
    this.ViewpatientDetails = this.patientDetails.filter((patient: { appointmentId: any; }) => patient.appointmentId === appointmentid);
    console.log("Viewpatientdetails : ", this.ViewpatientDetails)
    this.profileImage = this.apiUrl + this.ViewpatientDetails[0].profileimage;

  }
  GoBack()
  {
    this.isViewing = false;

  }
  

}
