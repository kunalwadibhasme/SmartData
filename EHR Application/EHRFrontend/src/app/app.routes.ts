import { Routes } from '@angular/router';
import { LoginregisterComponent } from './Components/loginregister/loginregister.component';
import { VerifyotpComponent } from './Components/verifyotp/verifyotp.component';
import { NavigationBarComponent } from './Components/navigation-bar/navigation-bar.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { ChangepasswordComponent } from './Components/changepassword/changepassword.component';
import { PatientdashboardComponent } from './Components/patientdashboard/patientdashboard.component';
import { ProviderdashboardComponent } from './Components/providerdashboard/providerdashboard.component';
import { CompletedappointmentComponent } from './Components/completedappointment/completedappointment.component';
import { ProviderCompletedDetailsComponent } from './Components/provider-completed-details/provider-completed-details.component';
import { ChatComponent } from './Components/chat/chat.component';
import { authGuard } from './auth.guard';
import { ForgetpasswordComponent } from './Components/forgetpassword/forgetpassword.component';
import { LoginComponent } from './Components/login/login.component';

export const routes: Routes = [
    // {path:'', component:LoginregisterComponent},
    // {path:'verify', component:VerifyotpComponent},
    // {path:'navigate', component: NavigationBarComponent},
    // {path:'profile', component:ProfileComponent},
    // {path:'changepassword', component:ChangepasswordComponent},
    // { path:'patientdashboard', component:PatientdashboardComponent},

    {
        path: '',
        redirectTo: 'login1',
        pathMatch: 'full'
      },
      {
        path:'login1',
        component:LoginComponent
      },
      {
        path: 'login',
        component: LoginregisterComponent
      },
      {
        path: 'forgetpassword',
        component: ForgetpasswordComponent
      },
      {
        path: 'verify',
        component: VerifyotpComponent
      },
      {
        path: '', component: NavigationBarComponent,canActivate: [authGuard],
        children: [
          { path: 'patientdashboard', component: PatientdashboardComponent },
          { path: 'profile', component: ProfileComponent },
          { path: 'changepassword', component: ChangepasswordComponent },
          { path: 'providerdashboard', component: ProviderdashboardComponent},
          { path: 'completeddetails', component:CompletedappointmentComponent},
          { path: 'completedetails', component:ProviderCompletedDetailsComponent},
          // { path: 'chat', component: ChatSelectionComponent },
          { path: 'chat', component: ChatComponent },
        ]
      }

    // {
    //     path:'',
    //     redirectTo:'register',
    //     pathMatch:'full'
    // },
    // {
    //     path:'register',
    //     component:LoginregisterComponent
    // },
    // {
    //     path:'verify',
    //     component:VerifyotpComponent
    // },
    // {
    //     path:'navigate', component:NavigationBarComponent,
    //     children:[
    //         { path:'patientdashboard', component:PatientdashboardComponent},
    //         {path:'profile', component:ProfileComponent},
    //         {path:'changepassword', component:ChangepasswordComponent},


    //     ]
    // }
    // {
    //     path: '',
    //     redirectTo:'login',
    //     pathMatch: 'full'
    // },
    // {
    //     path:'login',
    //     component:LoginregisterComponent,
    // },
    // {
    //     path:'verify',
    //     component:VerifyotpComponent
    // },
    // {path:'', component:NavigationBarComponent,
    //     children:[
    //         { path: 'profile', component: ProfileComponent }
    //     ]
    // }
            
    
];
