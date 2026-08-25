import { Routes } from '@angular/router';
import { CompanyComponent } from './components/company/company.component';
import { CompanyDetailComponent } from './components/company-detail/company-detail.component';
import { UserComponent } from './components/user/user.component';
import { UserDetailComponent } from './components/user-detail/user-detail.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './services/auth.guard';

export const routes: Routes = [
    //{ path: '', redirectTo: '/companies', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'companies', component: CompanyComponent, canActivate: [authGuard] },
    { path: 'company-detail', component: CompanyDetailComponent, canActivate: [authGuard] },
    { path: 'users', component: UserComponent, canActivate: [authGuard] },
    { path: 'user-detail', component: UserDetailComponent, canActivate: [authGuard] },
];
