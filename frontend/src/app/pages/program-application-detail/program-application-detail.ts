// src/app/pages/program-application-detail/program-application-detail.ts
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { CounselorProgramService } from '../../services/counselor-program';

@Component({
  selector: 'app-program-application-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, Navbar],
  templateUrl: './program-application-detail.html'
})
export class ProgramApplicationDetail implements OnInit {
  application: any = null;
  isLoading = true;
  errorMessage = '';
  applicationId: number = 0;

  constructor(
    private programService: CounselorProgramService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.applicationId = +params['id'];
      this.loadApplication();
    });
  }

  loadApplication() {
    this.isLoading = true;
    this.errorMessage = '';

    this.programService.getProgramApplicationById(this.applicationId).subscribe({
      next: (response: any) => {
        this.application = response.data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load application details.';
        this.cdr.detectChanges();
      }
    });
  }

  getStatusBadge(status: string): string {
    const badgeMap: Record<string, string> = {
      'pending': 'bg-yellow-100 text-yellow-700',
      'approved': 'bg-green-100 text-green-700',
      'rejected': 'bg-red-100 text-red-700',
      'waitlisted': 'bg-blue-100 text-blue-700'
    };
    return badgeMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  formatDate(date: any): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString('en-US', { 
      month: 'long', 
      day: 'numeric', 
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  goBack() {
    this.router.navigate(['/student/program-applications']);
  }
}