import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';
import { ApplicationService } from '../../services/application';
import { ProfileService } from '../../services/profile';

@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [CommonModule, Navbar, RouterLink],
  templateUrl: './student-dashboard.html'
})
export class StudentDashboard implements OnInit {
  userFullName = '';
  profileCompletionPercent = 0;

  recentJobs: any[] = [];
  applicationStats = {
    pending: 0,
    reviewed: 0,
    shortlisted: 0,
    hired: 0
  };

  constructor(
    private jobService: JobService,
    private applicationService: ApplicationService,
    private profileService: ProfileService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.userFullName = user.fullName;
    }

    this.loadRecentJobs();
    this.loadApplicationStats();
    this.loadProfileCompletion();
  }

  loadRecentJobs() {
    this.jobService.getActiveJobs().subscribe({
      next: (response) => {
        // Just show the first 3 for a dashboard preview
        this.recentJobs = response.data.slice(0, 3);
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  loadApplicationStats() {
    this.applicationService.getMyApplications().subscribe({
      next: (response) => {
        const applications = response.data;
        this.applicationStats.pending = applications.filter((a: any) => a.status === 'pending').length;
        this.applicationStats.reviewed = applications.filter((a: any) => a.status === 'reviewed').length;
        this.applicationStats.shortlisted = applications.filter((a: any) => a.status === 'shortlisted').length;
        this.applicationStats.hired = applications.filter((a: any) => a.status === 'hired').length;
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  loadProfileCompletion() {
    this.profileService.getStudentProfile().subscribe({
      next: (response) => {
        const profile = response.data;
        const fields = [
          profile.bio,
          profile.skills,
          profile.cvFilePath,
          profile.profilePicture,
          profile.faculty,
          profile.department
        ];
        const filledCount = fields.filter(f => f && f.trim() !== '').length;
        this.profileCompletionPercent = Math.round((filledCount / fields.length) * 100);
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }
}