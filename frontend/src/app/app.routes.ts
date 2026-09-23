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
import { PostJob } from './pages/post-job/post-job';
import { AdminLayout } from './layouts/admin-layout/admin-layout';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';
import { AdminUsers } from './pages/admin-users/admin-users';
import { AdminRoles } from './pages/admin-roles/admin-roles';
import { AdminJobApproval } from './pages/admin-job-approval/admin-job-approval';
import { AdminCounselors } from './pages/admin-counselors/admin-counselors';
import { StudentJobs } from './pages/student-jobs/student-jobs';
import { SavedJobs } from './pages/saved-jobs/saved-jobs';
import { JobDetailsStudent } from './pages/job-details-student/job-details-student';
import { MyApplications } from './pages/my-applications/my-applications';
import { ApplicationDetailStudent } from './pages/application-detail-student/application-detail-student';
import { StudentNotifications } from './pages/student-notifications/student-notifications';
import { StudentProfile } from './pages/student-profile/student-profile';
import { CounselorDashboard } from './pages/counselor-dashboard/counselor-dashboard';
import { CounselorPrograms } from './pages/counselor-programs/counselor-programs';
import { CreateProgram } from './pages/create-program/create-program';
import { EditProgram } from './pages/edit-program/edit-program';
import { CounselorApplications } from './pages/counselor-applications/counselor-applications';
import { StudentPrograms } from './pages/student-programs/student-programs';
import { CounselorProfile } from './pages/counselor-profile/counselor-profile';
import { EmployerProfile } from './pages/employer-profile/employer-profile';
import { MyProgramApplications } from './pages/my-program-applications/my-program-applications';
import { ProgramApplicationDetail } from './pages/program-application-detail/program-application-detail';
import { Landing } from './pages/landing/landing';
import { ResetPassword } from './pages/reset-password/reset-password';
import { ForgotPassword } from './pages/forgot-password/forgot-password';


export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'register/student', component: RegisterStudent },
  { path: 'register/employer', component: RegisterEmployer },
  { path: 'login', component: Login },
  { path: 'verify-email', component: VerifyEmail },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'forgot-password', component: ForgotPassword },
  { path: 'reset-password', component: ResetPassword },
  {path: 'admin',component: AdminLayout,canActivate: [authGuard],children: [{ path: 'dashboard', component: AdminDashboard },{ path: 'users', component: AdminUsers },  {path: 'roles',component: AdminRoles},{ path: 'job-approval',component: AdminJobApproval},{ path: 'counselors',component: AdminCounselors}]},
  { path: 'student/dashboard', component: StudentDashboard, canActivate: [authGuard] },
  { path: 'employer/dashboard', component: EmployerDashboard, canActivate: [authGuard] }, 
  { path: 'employer/jobs', component: EmployerJobs, canActivate: [authGuard] },
  { path: 'employer/jobs/post', component: PostJob, canActivate: [authGuard] },
  { path: 'employer/jobs/:id', component: JobDetails, canActivate: [authGuard] },
  { path: 'employer/jobs/:id/applications', component: JobApplications, canActivate: [authGuard] },
  { path: 'employer/applications', component: EmployerApplications, canActivate: [authGuard] },
  { path: 'employer/applications/:id', component: ApplicationDetails, canActivate: [authGuard] },
  { path: 'employer/jobs/:id/edit', component: UpdateJob, canActivate: [authGuard] },
  { path: 'employer/profile', component: EmployerProfile, canActivate: [authGuard] },
  {path: 'student/jobs', component: StudentJobs,canActivate: [authGuard]},
  { path: 'student/applications', component: MyApplications,canActivate: [authGuard]},
    { path: 'student/applications/:id', component: ApplicationDetailStudent,canActivate: [authGuard]},
  {  path: 'student/program-applications',  component: MyProgramApplications,canActivate: [authGuard]},
  {  path: 'student/program-applications/:id',  component: ProgramApplicationDetail, canActivate: [authGuard]},
  { path: 'student/programs', component: StudentPrograms },
  {path: 'student/saved-jobs',component: SavedJobs,canActivate: [authGuard]},
  {path: 'jobs/:id',component: JobDetailsStudent,canActivate: [authGuard]},
  { path: 'student/notifications', component: StudentNotifications, canActivate: [authGuard]},
  { path: 'student/profile',component: StudentProfile, canActivate: [authGuard]},
  { path: 'counselor/dashboard',component: CounselorDashboard,canActivate: [authGuard]},
  { path: 'counselor/programs',  component: CounselorPrograms, canActivate: [authGuard]},
  {path: 'counselor/programs/create', component: CreateProgram, canActivate: [authGuard]},
  { path: 'counselor/programs/:id/edit',component: EditProgram },
  { path: 'counselor/applications',component: CounselorApplications},
  { path: 'counselor/profile', component: CounselorProfile},
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];