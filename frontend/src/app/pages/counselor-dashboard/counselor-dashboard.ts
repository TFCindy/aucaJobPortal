import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { CounselorProgramService } from '../../services/counselor-program';

@Component({
  selector: 'app-counselor-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, Navbar],
  templateUrl: './counselor-dashboard.html'
})
export class CounselorDashboard implements OnInit {
  isLoading = true;
  errorMessage = '';
  counselorName = '';

  // Stats
  totalPrograms = 0;
  activePrograms = 0;
  publishedPrograms = 0;
  draftPrograms = 0;
  totalApplications = 0;
  pendingApplications = 0;
  totalStudents = 0;

  recentPrograms: any[] = [];
  recentApplications: any[] = [];

  constructor(
    private programService: CounselorProgramService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.counselorName = user.fullName || 'Counselor';
    }

    this.loadDashboardData();
  }

  loadDashboardData() {
    this.isLoading = true;
    this.errorMessage = '';

    this.programService.getMyPrograms().subscribe({
      next: (response) => {
        const programs = response.data || [];

        this.totalPrograms = programs.length;
        this.activePrograms = programs.filter((p: any) => p.isActive).length;
        this.publishedPrograms = programs.filter((p: any) => p.status === 'published' || p.status === 'ongoing' || p.status === 'upcoming').length;
        this.draftPrograms = programs.filter((p: any) => p.status === 'draft').length;

        this.totalApplications = programs.reduce((sum: number, p: any) => sum + (p.applicationCount || 0), 0);

        this.totalStudents = programs.reduce((sum: number, p: any) => sum + p.currentParticipants, 0);

        this.recentPrograms = programs
          .sort((a: any, b: any) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
          .slice(0, 3);

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load dashboard data.';
        this.cdr.detectChanges();
      }
    });

    this.programService.getMyPrograms().subscribe({
      next: (response) => {
        const programs = response.data || [];
        if (programs.length > 0) {
          const firstProgramId = programs[0]?.id;
          if (firstProgramId) {
            this.programService.getProgramApplications(firstProgramId).subscribe({
              next: (appResponse) => {
                const applications = appResponse.data || [];
                this.pendingApplications = applications.filter((a: any) => a.status === 'pending').length;
                this.recentApplications = applications
                  .sort((a: any, b: any) => new Date(b.applicationDate).getTime() - new Date(a.applicationDate).getTime())
                  .slice(0, 5);
                this.cdr.detectChanges();
              },
              error: () => {}
            });
          }
        }
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  getStatusBadge(status: string): string {
    const badgeMap: Record<string, string> = {
      'draft': 'bg-gray-100 text-gray-700',
      'published': 'bg-green-100 text-green-700',
      'upcoming': 'bg-blue-100 text-blue-700',
      'ongoing': 'bg-purple-100 text-purple-700',
      'full': 'bg-yellow-100 text-yellow-700',
      'completed': 'bg-gray-100 text-gray-700',
      'cancelled': 'bg-red-100 text-red-700'
    };
    return badgeMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  getApplicationStatusBadge(status: string): string {
    const badgeMap: Record<string, string> = {
      'pending': 'bg-yellow-100 text-yellow-700',
      'approved': 'bg-green-100 text-green-700',
      'rejected': 'bg-red-100 text-red-700',
      'waitlisted': 'bg-blue-100 text-blue-700'
    };
    return badgeMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
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
}