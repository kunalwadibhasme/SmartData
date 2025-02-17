import { Routes } from '@angular/router';
import { LoginregisterComponent } from './Components/loginregister/loginregister.component';
import { VerifyotpComponent } from './Components/verifyotp/verifyotp.component';
// import { ProductFormComponent } from './Components/product-form/product-form.component';
import { ProductComponent } from './Components/product/product.component';
import { NavigationbarComponent } from './Components/navigationbar/navigationbar.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { UserDetailsComponent } from './Components/user-details/user-details.component';
import { DisplaycartproductsComponent } from './Components/displaycartproducts/displaycartproducts.component';
import { authGuard } from './auth.guard';
import { ChangePasswordComponent } from './Components/change-password/change-password.component';
import { ReciptComponent } from './Components/recipt/recipt.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo:'login',
        pathMatch: 'full'
    },
    {
        path:'login',
        component:LoginregisterComponent,
    },
    {
        path:'verify',
        component:VerifyotpComponent
    },
    {path:'', component:NavigationbarComponent,canActivate: [authGuard],
        children:[
            { path: 'products', component: ProductComponent },
            { path: 'dashboard' , component: DashboardComponent },
            { path: 'changepassword', component: ChangePasswordComponent},
            { path: 'userprofile', component: UserDetailsComponent },
            { path: 'cartDetail', component: DisplaycartproductsComponent },
            { path: 'receipt', component:ReciptComponent}
        ]
    }
    
];
