import { Component, inject } from '@angular/core';
import { AppointmentService } from '../../Services/appointment.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-provider-completed-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './provider-completed-details.component.html',
  styleUrl: './provider-completed-details.component.css'
})
export class ProviderCompletedDetailsComponent {
toastr: ToastrService;
  ViewpatientDetails: any;
  ngOnInit(): void {
    const UsertypeId = sessionStorage.getItem('usertypeid');
      this.CompletedProviderdetails();
   
  }
constructor(private appointmentservice: AppointmentService) {
    this.toastr = inject(ToastrService);

  }

  patientDetails : any[] = [];
  providerdetails : any;
  isViewing : boolean = false;
  apiUrl = "https://localhost:7162"
  profileImage:any;
  UserId = sessionStorage.getItem('userid');
  isuser: boolean = false;
  islength:boolean = false;


  CompletedProviderdetails(){
    this.appointmentservice.Completedproviderdetails(this.UserId).subscribe({
      next:(value:any) =>{
        console.log("Completed Provider Details", value)
        this.patientDetails = value;
        console.log("Completed Provider Details after shifting", this.providerdetails)
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
