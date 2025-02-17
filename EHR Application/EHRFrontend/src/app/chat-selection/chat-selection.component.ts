import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ChatService } from '../Services/chat.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chat-selection',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat-selection.component.html',
  styleUrl: './chat-selection.component.css'
})
export class ChatSelectionComponent {
  patients: any[] =[];
  Person: any[] = [];
  userid: any;
  userTypeid: any;
  constructor(private router: Router, private chatservice: ChatService) {

    this.userid = sessionStorage.getItem('userid');
    this.userTypeid = sessionStorage.getItem('usertypeid');

    if (this.userTypeid == '1') {
      this.GetPatientByProviderId();
    }
    else {
      this.GetProviderByPatientId();
    }
  }

  GetPatientByProviderId() {
    this.chatservice.getpatientbyproviderid(this.userid).subscribe({
      next: (value: any) => {
        this.patients = value;
        console.log("Chat with patients : ", this.patients);
      }
    });
  }

  GetProviderByPatientId() {
    this.chatservice.getproviderbypatientid(this.userid).subscribe({
      next: (value: any) => {
        this.patients = value;
        console.log("Chat with patients : ", this.patients);
      }
    });
  }


  selectPatient(patientId:any) {
    const patientid = parseInt((patientId.target as HTMLSelectElement).value);
    console.log('patientId',patientid);
    this.router.navigate(['/chat', patientid]);
  }
}
