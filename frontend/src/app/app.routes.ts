import { Routes } from '@angular/router';
import { RegisterStudent } from './pages/register-student/register-student';
import { RegisterEmployer } from './pages/register-employer/register-employer';
import { Login } from './pages/login/login';
import { VerifyEmail } from './pages/verify-email/verify-email';

export const routes: Routes = [
  { path: 'register/student', component: RegisterStudent },
  { path: 'register/employer', component: RegisterEmployer },
  { path: 'login', component: Login },
  { path: 'verify-email', component: VerifyEmail },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];