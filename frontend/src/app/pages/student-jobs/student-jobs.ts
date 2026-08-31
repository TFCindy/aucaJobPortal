import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';
import { ApplicationService } from '../../services/application';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-student-jobs',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, Navbar],
  templateUrl: './student-jobs.html'
})
export class StudentJobs implements OnInit {
  jobs: any[] = [];
  filteredJobs: any[] = [];
  savedJobs: number[] = [];
  appliedJobs: number[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  selectedJob: any = null;
  showApplyModal = false;
  isSubmitting = false;

  constructor(
    private jobService: JobService,
    private applicationService: ApplicationService,
    private cdr: ChangeDetectorRef,
    private router: Router  
  ) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        const url = event.url;
        if (url === '/jobs' || url === '/student/jobs') {
          this.loadAppliedJobs();
        }
      }
    });
  }

  ngOnInit() {
    this.loadJobs();
    this.loadSavedJobs();
    this.loadAppliedJobs();
  }

  loadJobs() {
    this.isLoading = true;
    this.errorMessage = '';

    this.jobService.getActiveJobs().subscribe({
      next: (response) => {
        this.jobs = response.data || [];
        this.filteredJobs = [...this.jobs];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load jobs.';
        this.cdr.detectChanges();
      }
    });
  }

  loadSavedJobs() {
  this.jobService.getSavedJobs().subscribe({
    next: (response) => {
      // If response.data is an array of job objects
      if (response.data && response.data.length > 0) {
        // Use .id, not .jobId
        this.savedJobs = response.data.map((j: any) => j.id);
      } else {
        this.savedJobs = [];
      }
      console.log('Saved job IDs:', this.savedJobs);
      this.cdr.detectChanges();
    },
    error: (err: any) => {
      console.error('Error loading saved jobs:', err);
    }
  });
}

  loadAppliedJobs() {
  this.applicationService.getMyApplications().subscribe({
    next: (response) => {
      this.appliedJobs = response.data
        ?.filter((a: any) => a.status !== 'withdrawn' && a.status !== 'rejected')
        .map((a: any) => a.jobId) || [];
      this.cdr.detectChanges();
    },
    error: () => {}
  });
}

  applyFilters() {
    this.filteredJobs = this.jobs.filter(job => {
      const matchesSearch = !this.searchTerm ||
        job.title?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        job.companyName?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        job.location?.toLowerCase().includes(this.searchTerm.toLowerCase());
      return matchesSearch;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.filteredJobs = [...this.jobs];
    this.cdr.detectChanges();
  }

  toggleSaveJob(jobId: number) {
    if (this.savedJobs.includes(jobId)) {
      this.jobService.unsaveJob(jobId).subscribe({
        next: () => {
          this.savedJobs = this.savedJobs.filter(id => id !== jobId);
          this.successMessage = 'Job removed from saved.';
          this.cdr.detectChanges();
        },
        error: (err: any) => {
          this.errorMessage = err.error?.message || 'Failed to unsave job.';
          this.cdr.detectChanges();
        }
      });
    } else {
      this.jobService.saveJob(jobId).subscribe({
        next: () => {
          this.savedJobs.push(jobId);
          this.successMessage = 'Job saved successfully.';
          this.cdr.detectChanges();
        },
        error: (err: any) => {
          this.errorMessage = err.error?.message || 'Failed to save job.';
          this.cdr.detectChanges();
        }
      });
    }
  }

  isJobSaved(jobId: number): boolean {
    return this.savedJobs.includes(jobId);
  }

  isJobApplied(jobId: number): boolean {
    return this.appliedJobs.includes(jobId);
  }

  openApplyModal(job: any) {
    this.selectedJob = job;
    this.showApplyModal = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  closeApplyModal() {
    this.showApplyModal = false;
    this.selectedJob = null;
    this.cdr.detectChanges();
  }

  applyToJob() {
    if (!this.selectedJob) return;

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.jobService.applyToJob(this.selectedJob.id, {}).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.appliedJobs.push(this.selectedJob.id);
        this.successMessage = 'Application submitted successfully!';
        this.showApplyModal = false;
        this.selectedJob = null;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Failed to apply.';
        this.cdr.detectChanges();
      }
    });
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }

  getJobTypeBadge(type: string): string {
    const badgeMap: Record<string, string> = {
      'full-time': 'bg-blue-100 text-blue-700',
      'part-time': 'bg-yellow-100 text-yellow-700',
      'internship': 'bg-green-100 text-green-700',
      'contract': 'bg-purple-100 text-purple-700',
      'remote': 'bg-indigo-100 text-indigo-700',
      'hybrid': 'bg-pink-100 text-pink-700',
      'freelance': 'bg-orange-100 text-orange-700',
      'temporary': 'bg-gray-100 text-gray-700'
    };
    return badgeMap[type?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }
}