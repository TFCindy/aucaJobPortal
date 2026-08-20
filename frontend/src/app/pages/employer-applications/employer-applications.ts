import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ApplicationService } from '../../services/application';
import { JobService } from '../../services/job';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-employer-applications',
  standalone: true,
  imports: [CommonModule, Navbar, RouterLink, FormsModule],
  templateUrl: './employer-applications.html'
})
export class EmployerApplications implements OnInit {
  applications: any[] = [];
  filteredApplications: any[] = [];
  jobs: any[] = [];
  isLoading = true;
  errorMessage = '';

  searchTerm = '';
  selectedJobId = '';
  selectedStatus = '';

  statuses = ['pending', 'reviewed', 'shortlisted', 'hired', 'rejected'];

  constructor(
    private applicationService: ApplicationService,
    private jobService: JobService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    this.errorMessage = '';

    this.jobService.getMyJobs().subscribe({
      next: (jobResponse) => {
        this.jobs = jobResponse.data || [];
        this.cdr.detectChanges();
      },
      error: () => {}
    });

    this.applicationService.getEmployerApplications().subscribe({
      next: (response) => {
         console.log('Full response:', response);
        this.applications = response.data || [];
        this.filteredApplications = [...this.applications];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error:', err);
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load applications.';
        this.cdr.detectChanges();
      }
    });
  }

  applyFilters() {
    this.filteredApplications = this.applications.filter(app => {
      const matchesSearch = !this.searchTerm || 
        app.student?.user?.fullName?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        app.job?.title?.toLowerCase().includes(this.searchTerm.toLowerCase());

      const matchesJob = !this.selectedJobId || app.jobId === +this.selectedJobId;

      const matchesStatus = !this.selectedStatus || app.status === this.selectedStatus;

      return matchesSearch && matchesJob && matchesStatus;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.selectedJobId = '';
    this.selectedStatus = '';
    this.filteredApplications = [...this.applications];
    this.cdr.detectChanges();
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

  getJobTitle(jobId: number): string {
    const job = this.jobs.find(j => j.id === jobId);
    return job?.title || 'Unknown Job';
  }
}