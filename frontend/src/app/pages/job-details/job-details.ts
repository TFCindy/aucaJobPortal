import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';

@Component({
  selector: 'app-job-details',
  standalone: true,
  imports: [CommonModule, Navbar, RouterLink],
  templateUrl: './job-details.html'
})
export class JobDetails implements OnInit {
  job: any = null;
  isLoading = true;
  errorMessage = '';
  showDeleteModal = false;
  isDeleting = false;

  constructor(
    private jobService: JobService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const jobId = this.route.snapshot.paramMap.get('id');
    if (jobId) {
      this.loadJob(+jobId);
    }
  }

  loadJob(jobId: number) {
    this.isLoading = true;
    this.errorMessage = '';

    this.jobService.getJobById(jobId).subscribe({
      next: (response) => {
        this.job = response.data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load job details.';
        this.cdr.detectChanges();
      }
    });
  }

  deleteJob() {
    if (!this.job) return;
    this.isDeleting = true;

    this.jobService.deleteJob(this.job.id).subscribe({
      next: () => {
        this.isDeleting = false;
        this.router.navigate(['/employer/jobs']);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isDeleting = false;
        this.errorMessage = err.error?.message || 'Failed to delete job.';
        this.cdr.detectChanges();
      }
    });
  }

  getStatusBadge(status: string): string {
    const statusMap: Record<string, string> = {
      'active': 'bg-green-100 text-green-700',
      'closed': 'bg-gray-100 text-gray-700',
      'draft': 'bg-yellow-100 text-yellow-700'
    };
    return statusMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }
}