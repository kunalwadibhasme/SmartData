import { Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { NavigationbarComponent } from './navigationbar/navigationbar.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { authGaurdGuard } from './auth-gaurd.guard';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
      },
      {
        path:'login',
        component:LoginComponent
      },
      {
        path:'register',
        component:RegisterComponent
      },{
        path:'',
        component:NavigationbarComponent,canActivate: [authGaurdGuard],
        children: [
          { path: 'dashboard', component: DashboardComponent },
          { path: 'landingpage', component: LandingPageComponent }
        ]
      }
];
