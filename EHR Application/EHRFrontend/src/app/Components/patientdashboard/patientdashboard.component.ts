import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppointmentService } from '../../Services/appointment.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { environment } from '../../../environment/environment';
import { StripeService } from '../../Services/stripe.service';
import { Router } from '@angular/router';
declare var Stripe: any;


@Component({
  selector: 'app-patientdashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './patientdashboard.component.html',
  styleUrls: ['./patientdashboard.component.css']
})
export class PatientdashboardComponent {

  appointmentForm!: FormGroup;
  updateForm!: FormGroup;
  isFormVisible: boolean = false; // Toggle visibility of the form
  specialities: any;
  providers: any;
  filteredProviders: any;
  minDate!: string; // For binding the minimum date
  timeMin: string = '';
  UserId: any;
  speciality: any;
  fees: any;
  Patients: any[]=[];
  ispatientlength: Boolean = false;
  patientupdate: any;
  update: boolean = false;
  isLoading:boolean=false;
  stripe: any;
  elements: any;
  card: any;

  
  constructor(private fb: FormBuilder, private appointmentservice: AppointmentService, 
    private toastr: ToastrService, private stripeservice: StripeService, private router:Router) {
      const today = new Date();
      this.minDate = today.toISOString().split('T')[0];
  }

  ngOnInit(): void {
    this.isFormVisible = false;
    this.UserId = sessionStorage.getItem('userid');
    this.initializeForm();
    this.setMinDate();
    this.getAppointmentDetailsbyUserId();    

    this.updateForm = this.fb.group({
      appointmentId:[],
      appointmentDate: ['', Validators.required],
      appointmentTime: ['', Validators.required],
      chiefComplaint: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(200)]],
    })
  }

  initializeForm() {
    this.UserId = sessionStorage.getItem('userid');

    this.appointmentForm = this.fb.group({
      patientId: [this.UserId],
      providerId: ['', Validators.required],
      appointmentDate: ['', Validators.required],
      appointmentTime: ['', Validators.required],
      chiefComplaint: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(200)]],
      status: ['Scheduled'],
      fees: [this.fees]

    });

  }

  setMinDate() {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const yyyy = today.getFullYear();
    this.minDate = `${yyyy}-${mm}-${dd}`; // Format as YYYY-MM-DD
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

  Completeddetails()
  {
    this.router.navigateByUrl('/completedetails');
  }


  getProviderfees(event: any): void {
    const target = event.target as HTMLSelectElement;
    if (!target) {
      console.error("Invalid event target.");
      return;
    }
    const selectedId = target.value; 
    if (!selectedId) {
      console.error("No provider selected.");
      return;
    }
    const selectedProvider = this.filteredProviders.find((provider: { id: any; }) => provider.id === parseInt(selectedId));
    if (selectedProvider) {
      console.log("Selected provider:", selectedProvider);
      const providerFees = selectedProvider.visitingCharge; 
      console.log("Provider Fees:", providerFees);
      this.fees = providerFees;
    } else {
      console.error("Provider not found with ID:", selectedId);
    }
  }

  showAddAppointmentForm(): void {
    this.getSpecializationdata();
    this.isFormVisible = true; 
    // loadStripe(environment.stripePublishableKey).then(stripe => {
    //   this.stripe = stripe;
    //   this.elements = this.stripe.elements();
    //   // Create an instance of the Card element
    //   this.card = this.elements.create('card');
    //   this.card.mount('#card-element');
    // });
    

  }
  getAppointmentDetailsbyUserId() {
    this.appointmentservice.getAppointmentbyPatientId(this.UserId).subscribe({
      next: (value: any) => {
        this.Patients = value;
        console.log("Patient : ", this.Patients);
        if(this.Patients.length ==0)
        {
          this.ispatientlength = true;
        }
        else{
          this.ispatientlength = false;
        }

      }
    })
  }

  getSpecializationdata() {
    this.appointmentservice.getSpecialization().subscribe({
      next: (value: any) => {
        this.specialities = value;
        console.log("Specialization : ", this.specialities);
      }
    });
  }

  getProviderbasedonSpeciality(event: Event) {
    
    this.stripe = Stripe(environment.stripePublishableKey);
    this.elements = this.stripe.elements();
    this.card = this.elements.create('card');
    this.card.mount('#card-element');
    const target = event.target as HTMLSelectElement;
    if (!target) {
      console.error("Invalid event target.");
      return;
    }
    const selectedId = target.selectedIndex;
    this.appointmentservice.getProvider(selectedId).subscribe({
      next: (value: any) => {
        this.filteredProviders = value;
        console.log("Providers: ", this.filteredProviders);
      },
      error: (err) => {
        console.error("Error fetching providers:", err);
      },
    });
  }


  isTimeValid(): boolean {
    const selectedDate = new Date(this.appointmentForm.value.appointmentDate);
    const today = new Date();
    if (selectedDate.toDateString() === today.toDateString()) {
      const selectedTime = this.appointmentForm.value.appointmentTime;
      const currentTime = today.getHours() + today.getMinutes() / 60;
      const selectedTimeValue = parseInt(selectedTime.split(':')[0]) + parseInt(selectedTime.split(':')[1]) / 60;
      return selectedTimeValue > currentTime + 1;
    }
    return true;
  }





  async onSubmit(){
    this.isLoading=true;
    this.appointmentForm.value.fees = this.fees;
    if (this.appointmentForm.valid && this.isTimeValid()) {
      console.log('Form Submitted:', this.appointmentForm.value);
      try {
        const paymentIntent = await this.stripeservice.createPaymentIntent(this.fees);
        const { error, paymentIntent: confirmedPaymentIntent } = await this.stripe.confirmCardPayment(
          paymentIntent?.client_secret,
          {
            payment_method: {
              card: this.card,
              billing_details: {
                name: 'Test User',
                email: 'mailto:test@example.com',
                address: { line1: '123 Main St', city: 'Test City', postal_code: '12345' }
              }
            }
          }
        );
        if (error) {
          console.error(error);
          this.isLoading = false;
          alert('Payment failed: ' + error.message);
        } else if (confirmedPaymentIntent.status === 'succeeded') {
          this.appointmentservice.AddAppointment(this.appointmentForm.value).subscribe({
            next: (res: any) => {
              if (res == true) {
                Swal.fire({
                  title: 'Success!',
                  text: 'Appointment Booked successfully!',
                  icon: 'success',
                  confirmButtonText: 'OK'
                });
                this.initializeForm();
                this.getAppointmentDetailsbyUserId();
                this.isFormVisible = false;
                this.isLoading=false;
              }
            }
          })        
        } else {
          this.isLoading = false;
          alert('Payment failed');
        }
      } catch (error) {
        console.error(error);
        alert('Error processing payment');
        this.isLoading = false;
      }
      
      // Hide form after successful submission
    } else {
      this.isLoading=false;
      this.appointmentForm.markAllAsTouched();
    }
  }

  onUpdate()
  {
    this.isLoading=true;
    this.updateForm.value.appointmentId = this.patientupdate.appointmentId;
    if (this.updateForm.valid && this.isTimeValid()) {
      console.log('UpdateForm Submitted:', this.updateForm.value);

      this.appointmentservice.UpdateAppointment(this.updateForm.value).subscribe({
        next: (res: any) => {
          if (res == true) {
            Swal.fire({
              title: 'Success!',
              text: 'Appointment Updated successfully!',
              icon: 'success',
              confirmButtonText: 'OK'
            });
            this.isLoading=false;
            this.initializeForm();
            this.getAppointmentDetailsbyUserId();
            this.update = false;
          }
          else{
            Swal.fire({
              icon: "error",
              title: "Failed",
              text: "Appointment Update Failed!",
            });
            this.isLoading=false;    
          }
        }
      })
      // Hide form after successful submission
    } else {
      this.updateForm.markAllAsTouched();
    }
  }

  OnEditClick(patientupdate : any)
  {
    this.update=true;
    this.patientupdate = patientupdate;
    this.updateForm.patchValue(patientupdate);
  }

  onCancel(): void {
    this.initializeForm();
    this.isFormVisible = false; // Hide form on cancel
  }

  onCancelupdate():void{
    this.initializeForm();
    this.update = false;
  }

  navigatetochat()
  {
    this.router.navigateByUrl('/chat');
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
              this.initializeForm();
              this.getAppointmentDetailsbyUserId();
            }
          }
        });
      }
    });
  }
}
