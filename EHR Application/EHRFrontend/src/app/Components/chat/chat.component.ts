import { Component, inject } from '@angular/core';
import { initializeApp } from 'firebase/app';
import { getDatabase, ref, set, onValue, child, push } from 'firebase/database';
import { environment } from '../../../environment/environment';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatService } from '../../Services/chat.service';


// Initialize Firebase
const app = initializeApp(environment.firebase);
const db = getDatabase(app);
@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent {
  receiverId: any;
  message: string = '';
  messages: any[] = [];
  ReceiverData: any[] = [];
  userid: any;
  userTypeid: any;
  isuser: boolean = false;

  constructor(private router: Router, private chatservice: ChatService, private activatedRoute: ActivatedRoute) {

    this.userid = sessionStorage.getItem('userid');
    this.userTypeid = sessionStorage.getItem('usertypeid');


    if (this.userTypeid == '1') {
      this.isuser = true;
      this.GetPatientByProviderId();
    }
    else {
      this.isuser = false;
      this.GetProviderByPatientId();
    }
  }

  ngOnInit() {
    // Initialize listening to messages for a specific receiver
    this.listenForMessages(this.receiverId);

    // Initialize listening to messages for a specific receiver
    this.receiverId = this.activatedRoute.snapshot.paramMap.get('userid');
    this.listenForMessages(this.receiverId);

  }

  GetPatientByProviderId() {
    this.chatservice.getpatientbyproviderid(this.userid).subscribe({
      next: (value: any) => {
        this.ReceiverData = value;
        console.log("Chat with patients : ", this.ReceiverData);
      }
    });
  }

  GetProviderByPatientId() {
    this.chatservice.getproviderbypatientid(this.userid).subscribe({
      next: (value: any) => {
        this.ReceiverData = value;
        console.log("Chat with provider : ", this.ReceiverData);
      }
    });
  }


  selectPatient(patientId: any) {
    this.receiverId = (parseInt((patientId.target as HTMLSelectElement).value)).toString();
    console.log('receiverId ', this.receiverId);
    this.listenForMessages(this.receiverId);

  }

  sendMessage(receiverId: string, message: string): void {
    if (message == '') {
      return
    }
    console.log(receiverId + "" + message);

    const messageRef = ref(db, 'messages/' + receiverId + sessionStorage.getItem('userid'));
    const newMessageRef = push(messageRef);
    set(newMessageRef, {
      senderId: sessionStorage.getItem('userid'),
      message: message,
      timestamp: Date.now(),
    });

    const senderMessageRef = ref(db, 'messages/' + sessionStorage.getItem('userid') + receiverId);
    const senderNewMessageRef = push(senderMessageRef);
    set(senderNewMessageRef, {
      receiverId: receiverId,
      message: message,
      timestamp: Date.now(),
    });
  }

  listenForMessages(receiverId: string): void {

    const messageRef = ref(db, 'messages/' + sessionStorage.getItem('userid') + receiverId);
    onValue(messageRef, (snapshot) => {
      const data = snapshot.val();
      if (data) {
        this.messages = Object.values(data).sort((a: any, b: any) => a.timestamp - b.timestamp);
        console.log(this.messages);

      }
    });

  }
}

