import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AngularFirestore } from '@angular/fire/compat/firestore';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  constructor(private http:HttpClient) {}
  // private firestore: AngularFirestore
  // getMessages(chatId: string): Observable<any[]> {
  //   return this.firestore
  //     .collection(`chats/${chatId}/messages`, ref => ref.orderBy('timestamp'))
  //     .valueChanges();
  // }

  // sendMessage(chatId: string, message: { senderId: string; text: string; timestamp: any }) {
  //   return this.firestore
  //     .collection(`chats/${chatId}/messages`)
  //     .add(message);
  // }
  getpatientbyproviderid(Id : any)
  {
    return this.http.get<any>(`https://localhost:7162/api/Chat/GetPatientbyProviderId?providerId=${Id}`);
  }
  getproviderbypatientid(Id:any)
  {
    return this.http.get<any>(`https://localhost:7162/api/Chat/GetProviderByPatientId?patientId=${Id}`)
  }
}
