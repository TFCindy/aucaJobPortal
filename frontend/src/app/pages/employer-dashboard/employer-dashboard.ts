import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';
import { ApplicationService } from '../../services/application';
import { ProfileService } from '../../services/profile';

@Component({
  selector: 'app-employer-dashboard',
  standalone: true,
  imports: [CommonModule, Navbar, RouterLink],
  templateUrl: './employer-dashboard.html'
})
export class EmployerDashboard implements OnInit {
  companyName = '';
  profileCompletionPercent = 0;

  activeJobsCount = 0;
  totalApplications = 0;
  newApplications = 0;
  hiredCount = 0;

  pendingCount = 0;
  shortlistedCount = 0;
  interviewCount = 0;
  rejectedCount = 0;

  recentApplications: any[] = [];
  recentJobs: any[] = [];

  isLoading = true;
  errorMessage = '';

  constructor(
    private jobService: JobService,
    private applicationService: ApplicationService,
    private profileService: ProfileService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadDashboard();
  }

  loadDashboard() {
    this.isLoading = true;
    this.errorMessage = '';

    // 1. Load Employer Profile
    this.profileService.getEmployerProfile().subscribe({
      next: (response) => {
        const profile = response.data;
        this.companyName = profile.companyName || '';
        
        const fields = [
          profile.companyName,
          profile.companyDescription,
          profile.companyWebsite,
          profile.industry,
          profile.location,
          profile.companyLogo
        ];
        const filledCount = fields.filter(f => f && f.trim() !== '').length;
        this.profileCompletionPercent = Math.round((filledCount / fields.length) * 100);
        this.cdr.detectChanges();
      },
      error: () => {}
    });

    // 2. Load Jobs
    this.jobService.getMyJobs().subscribe({
      next: (response) => {
        const jobs = response.data || [];
        this.activeJobsCount = jobs.filter((j: any) => j.isActive).length;
        this.recentJobs = jobs.slice(0, 3);
        this.cdr.detectChanges();
      },
      error: () => {}
    });

    // 3. Load Applications
    this.applicationService.getEmployerApplications().subscribe({
      next: (response) => {
        const applications = response.data || [];
        this.totalApplications = applications.length;

        const oneWeekAgo = new Date();
        oneWeekAgo.setDate(oneWeekAgo.getDate() - 7);
        this.newApplications = applications.filter(
          (a: any) => new Date(a.appliedAt) >= oneWeekAgo
        ).length;

        this.pendingCount = applications.filter((a: any) => a.status === 'pending').length;
        this.shortlistedCount = applications.filter((a: any) => a.status === 'shortlisted').length;
        this.interviewCount = applications.filter((a: any) => a.status === 'interview').length;
        this.rejectedCount = applications.filter((a: any) => a.status === 'rejected').length;
        this.hiredCount = applications.filter((a: any) => a.status === 'hired').length;

        this.recentApplications = applications
          .sort((a: any, b: any) => new Date(b.appliedAt).getTime() - new Date(a.appliedAt).getTime())
          .slice(0, 5);

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  getStatusBadgeClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      'pending': 'bg-yellow-100 text-yellow-700',
      'reviewed': 'bg-blue-100 text-blue-700',
      'shortlisted': 'bg-purple-100 text-purple-700',
      'interview': 'bg-indigo-100 text-indigo-700',
      'hired': 'bg-green-100 text-green-700',
      'rejected': 'bg-red-100 text-red-700'
    };
    return statusMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }
}