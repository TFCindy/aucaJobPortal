import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';

@Component({
  selector: 'app-employer-jobs',
  standalone: true,
  imports: [CommonModule, Navbar, RouterLink],
  templateUrl: './employer-jobs.html'
})
export class EmployerJobs implements OnInit {
  jobs: any[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private jobService: JobService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadJobs();
  }

  loadJobs() {
    this.isLoading = true;
    this.errorMessage = '';

    this.jobService.getMyJobs().subscribe({
      next: (response) => {
        this.jobs = response.data || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load jobs.';
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