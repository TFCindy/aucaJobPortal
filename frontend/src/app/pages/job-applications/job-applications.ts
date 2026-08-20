import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ApplicationService } from '../../services/application';

@Component({
  selector: 'app-job-applications',
  standalone: true,
  imports: [CommonModule, Navbar, RouterLink],
  templateUrl: './job-applications.html'
})
export class JobApplications implements OnInit {
  applications: any[] = [];
  jobTitle = '';
  isLoading = true;
  errorMessage = '';

  constructor(
    private applicationService: ApplicationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const jobId = this.route.snapshot.paramMap.get('id');
    if (jobId) {
      this.loadApplications(+jobId);
    }
  }

  loadApplications(jobId: number) {
    this.isLoading = true;
    this.errorMessage = '';

    this.applicationService.getJobApplications(jobId).subscribe({
      next: (response) => {
        this.applications = response.data || [];
        this.jobTitle = response.jobTitle || 'Job';
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load applications.';
        this.cdr.detectChanges();
      }
    });
  }

  getStatusBadge(status: string): string {
    const statusMap: Record<string, string> = {
      'pending': 'bg-yellow-100 text-yellow-700',
      'reviewed': 'bg-blue-100 text-blue-700',
      'shortlisted': 'bg-purple-100 text-purple-700',
      'hired': 'bg-green-100 text-green-700',
      'rejected': 'bg-red-100 text-red-700'
    };
    return statusMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }
}