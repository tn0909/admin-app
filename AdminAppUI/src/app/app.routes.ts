import { Routes } from '@angular/router';
import { CompanyComponent } from './components/company/company.component';
import { CompanyDetailComponent } from './components/company-detail/company-detail.component';
import { UserComponent } from './components/user/user.component';
import { UserDetailComponent } from './components/user-detail/user-detail.component';

export const routes: Routes = [
    //{ path: '', redirectTo: '/companies', pathMatch: 'full' },
    { path: 'companies', component: CompanyComponent },
    { path: 'company-detail', component: CompanyDetailComponent },
    { path: 'users', component: UserComponent },
    { path: 'user-detail', component: UserDetailComponent },
];
