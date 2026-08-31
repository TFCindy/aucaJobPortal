// student-dashboard.ts
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';
import { ApplicationService } from '../../services/application';
import { ProfileService } from '../../services/profile';
import { NotificationService } from '../../services/notification';
import { CounselorProgramService } from '../../services/counselor-program';
import { environment } from '../../../environments/environment';

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
  recentPrograms: any[] = [];
  recentNotifications: any[] = [];
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
    private notificationService: NotificationService,
    private programService: CounselorProgramService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.userFullName = user.fullName;
    }

    this.loadRecentJobs();
    this.loadRecentPrograms();
    this.loadApplicationStats();
    this.loadProfileCompletion();
    this.loadRecentNotifications();
  }

  loadRecentNotifications() {
    this.notificationService.getMyNotifications().subscribe({
      next: (response) => {
        const unreadNotifications = response.data.filter((n: any) => !n.isRead);
        this.recentNotifications = unreadNotifications.slice(0, 4);
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  markNotificationRead(notification: any) {
    if (notification.isRead) {
      return;
    }
    this.notificationService.markAsRead(notification.id).subscribe({
      next: () => {
        notification.isRead = true;
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  loadRecentJobs() {
    this.jobService.getActiveJobs().subscribe({
      next: (response) => {
        this.recentJobs = response.data.slice(0, 3);
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  loadRecentPrograms() {
    this.programService.getAvailablePrograms().subscribe({
      next: (response) => {
        this.recentPrograms = (response.data || []).slice(0, 3);
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

  getImageUrl(imagePath: string): string {
    if (!imagePath) return '';
    const baseUrl = environment.apiUrl.replace('/api', '');
    return `${baseUrl}${imagePath}`;
  }

  getTypeLabel(type: string): string {
    const typeMap: Record<string, string> = {
      'training': 'Training',
      'workshop': 'Workshop',
      'seminar': 'Seminar',
      'webinar': 'Webinar',
      'bootcamp': 'Bootcamp'
    };
    return typeMap[type?.toLowerCase()] || type || 'Program';
  }

  formatDate(date: any): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric', 
      year: 'numeric' 
    });
  }
}