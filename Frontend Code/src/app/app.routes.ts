import { Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component'; // Import LoginComponent
import { SearchComponent } from './search/search.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { ProviderFormComponent } from './provider-form/provider-form.component';
import { TransportScheduleComponent } from './transport-schedule/transport-schedule.component';
import { CreateScheduleComponent } from './create-schedule/create-schedule.component';
import { BookingComponent } from './booking/booking.component';
import { PaymentComponent } from './payment/payment.component';
import { BookingHistoryComponent } from './booking-history/booking-history.component';
import { PaymentSuccessComponent } from './payment-success/payment-success.component';
import { ScheduleListComponent } from './schedule-list/schedule-list.component';
import { UsersComponent } from './admin/users/users.component';
import { ProvidersComponent } from './admin/providers/providers.component';
import { DashboardMetricsComponent } from './admin/dashboard-metrics/dashboard-metrics.component';
import { RevenueComponent } from './admin/revenue/revenue.component';
import { BookingReportComponent } from './admin/booking-report/booking-report.component';
import { AdminHeaderComponent } from './admin/admin-header/admin-header.component';
import { roleGuard } from './role.guard';
import { ForbiddenComponent } from './error/forbidden/forbidden.component';
export const routes: Routes = [
    {path:'',component:HomeComponent},
    { path: 'register', component: RegisterComponent },
    { path: 'login', component: LoginComponent }, 
    {path:'search',component:SearchComponent},
    {path:'header',component:HeaderComponent},
    {path:'home',component:HomeComponent},
    {path:'provider',component:ProviderFormComponent,canActivate: [roleGuard],data: { requiredRole: 'Admin'}},
    {path:'schedule',component:TransportScheduleComponent,canActivate: [roleGuard],data: { requiredRole: 'Provider'}},
    {path:'booking',component:BookingComponent},
    {path: 'payment',component:PaymentComponent},
    {path:'booking-history',component:BookingHistoryComponent},
    {path:'payed',component:PaymentSuccessComponent},
    {path:'schedule-list',component:ScheduleListComponent,canActivate: [roleGuard],data: { requiredRole: 'Provider'}},
    {path:'admin-user',component:UsersComponent},
    {path:'admin-provider',component:ProvidersComponent,canActivate: [roleGuard],data: { requiredRole: 'Admin'}},
    {path:'admin-dashboard',component:DashboardMetricsComponent,canActivate: [roleGuard],data: { requiredRole: 'Admin'}},
    {path:'admin-revenue',component:RevenueComponent,canActivate: [roleGuard],data: { requiredRole: 'Admin'}},
    {path:'admin-booking',component:BookingReportComponent,canActivate: [roleGuard],data: { requiredRole: 'Admin'}},
    {path:'create-schedule',component:CreateScheduleComponent,canActivate: [roleGuard],data: { requiredRole: 'Provider'}},
    {path:'admin-header',component:AdminHeaderComponent,canActivate: [roleGuard],data: { requiredRole: 'Admin'}},
    {path:'forbidden',component:ForbiddenComponent},
    {path:'**',component:HomeComponent}

   
   
];
