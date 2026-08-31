import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JobApprovalService } from '../../services/job-approval';

@Component({
  selector: 'app-admin-job-approval',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-job-approval.html'
})
export class AdminJobApproval implements OnInit {
  jobs: any[] = [];
  filteredJobs: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  selectedJob: any = null;
  showRejectModal = false;
  rejectReason = '';

  constructor(
    private jobApprovalService: JobApprovalService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadPendingJobs();
  }

  loadPendingJobs() {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.jobApprovalService.getPendingJobs().subscribe({
      next: (response) => {
        this.jobs = response.data || [];
        this.filteredJobs = [...this.jobs];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load pending jobs.';
        this.cdr.detectChanges();
      }
    });
  }

  applyFilters() {
    this.filteredJobs = this.jobs.filter(job => {
      const matchesSearch = !this.searchTerm ||
        job.title?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        job.companyName?.toLowerCase().includes(this.searchTerm.toLowerCase());
      return matchesSearch;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.filteredJobs = [...this.jobs];
    this.cdr.detectChanges();
  }

  viewJob(job: any) {
    this.selectedJob = job;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  closeJobDetails() {
    this.selectedJob = null;
    this.cdr.detectChanges();
  }

  approveJob(jobId: number) {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.jobApprovalService.approveJob(jobId).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = 'Job approved successfully.';
        this.selectedJob = null;
        this.loadPendingJobs();
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to approve job.';
        this.cdr.detectChanges();
      }
    });
  }

  openRejectModal(jobId: number) {
    this.selectedJob = this.jobs.find(j => j.id === jobId) || null;
    this.showRejectModal = true;
    this.rejectReason = '';
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  closeRejectModal() {
    this.showRejectModal = false;
    this.selectedJob = null;
    this.rejectReason = '';
    this.cdr.detectChanges();
  }

  confirmReject() {
    if (!this.selectedJob) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.jobApprovalService.rejectJob(this.selectedJob.id).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = `Job "${this.selectedJob.title}" rejected.`;
        this.showRejectModal = false;
        this.selectedJob = null;
        this.loadPendingJobs();
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to reject job.';
        this.cdr.detectChanges();
      }
    });
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }

  getStatusBadge(status: string): string {
    const badgeMap: Record<string, string> = {
      'pending': 'bg-yellow-100 text-yellow-700',
      'active': 'bg-green-100 text-green-700',
      'rejected': 'bg-red-100 text-red-700'
    };
    return badgeMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }
}