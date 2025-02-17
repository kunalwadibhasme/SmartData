import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  

  constructor(private http : HttpClient) { }
  
    private baseUrl = 'https://localhost:7162/api/UserRegister';

  getSpecialization(): Observable<any> {
      return this.http.get<any>(`${this.baseUrl}/Specialization`)
    }

  getProvider(id:any):Observable<any>{
    return this.http.get<any>(`https://localhost:7162/api/Appointment/ProviderbasedonSpeciality?Id=${id}`)
  }

  AddAppointment(formData: FormData) {
    return this.http.post<any>(`https://localhost:7162/api/Appointment/SaveAppointment`, formData)
  }

  getAppointmentbyPatientId(Id : any)
  {
    return this.http.get<any>(`https://localhost:7162/api/Appointment/AppointmentsByPatientId?Id=${Id}`)
  }

  getPatientData()
  {
    return this.http.get<any>(`https://localhost:7162/api/Appointment/ListOfPatient`)
  }

  getProviderFees(Id:any)
  {
    return this.http.get<any>(`https://localhost:7162/api/UserRegister/GetUserById?Id=${Id}`)
  }
   
  getpatientbyproviderId(Id:any)
  {
    return this.http.get<any>(`https://localhost:7162/api/Appointment/GetpatientbyProviderId?Id=${Id}`);
  }

  CancelAppointment(Id:any)
  {
    return this.http.delete<any>(`https://localhost:7162/api/Appointment/CancelAppointment?Id=${Id}`);
  }
  UpdateAppointment(data:any)
  {
    return this.http.post<any>(`https://localhost:7162/api/Appointment/UpdateAppointment`, data);
  }

  GoToAppointment(appointmentId: any)
  {
    return this.http.get<any>(`https://localhost:7162/api/Appointment/GoToAppointmentDetails?AppointmentId=${appointmentId}`);
  }

  SaveSoap(data: any)
  {
    return this.http.post<any>(`https://localhost:7162/api/Appointment/SaveSoap`, data);
  }

  Completedpatientdetails(Id : any)
  {
    return this.http.get<any>(`https://localhost:7162/api/Appointment/CompletedAppointmentDetailsforProvider?Id=${Id}`);
  }

  Completedproviderdetails(Id:any)
  {
    return this.http.get<any>(`https://localhost:7162/api/Appointment/CompletedAppointmentDetailsforPatient?Id=${Id}`);
  }
}
