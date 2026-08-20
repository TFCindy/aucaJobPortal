import { Routes } from '@angular/router';
import { RegisterStudent } from './pages/register-student/register-student';
import { RegisterEmployer } from './pages/register-employer/register-employer';
import { Login } from './pages/login/login';
import { VerifyEmail } from './pages/verify-email/verify-email';
import { StudentDashboard } from './pages/student-dashboard/student-dashboard';
import { authGuard } from './guards/auth-guard';
import { EmployerDashboard } from './pages/employer-dashboard/employer-dashboard';
import { JobApplications } from './pages/job-applications/job-applications';
import { EmployerApplications } from './pages/employer-applications/employer-applications';
import { EmployerJobs } from './pages/employer-jobs/employer-jobs';
import { JobDetails } from './pages/job-details/job-details';
import { ApplicationDetails } from './pages/application-details/application-details';
import { UpdateJob } from './pages/update-job/update-job';


export const routes: Routes = [
  { path: 'register/student', component: RegisterStudent },
  { path: 'register/employer', component: RegisterEmployer },
  { path: 'login', component: Login },
  { path: 'verify-email', component: VerifyEmail },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'student/dashboard', component: StudentDashboard, canActivate: [authGuard] },
  { path: 'employer/dashboard', component: EmployerDashboard, canActivate: [authGuard] }, 
  { path: 'employer/jobs', component: EmployerJobs, canActivate: [authGuard] },
  { path: 'employer/jobs/:id', component: JobDetails, canActivate: [authGuard] },
  { path: 'employer/jobs/:id/applications', component: JobApplications, canActivate: [authGuard] },
  { path: 'employer/applications', component: EmployerApplications, canActivate: [authGuard] },
  { path: 'employer/applications/:id', component: ApplicationDetails, canActivate: [authGuard] },
  { path: 'employer/jobs/:id/edit', component: UpdateJob, canActivate: [authGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];