import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';
import { ApplicationService } from '../../services/application';

@Component({
  selector: 'app-saved-jobs',
  standalone: true,
  imports: [CommonModule, RouterLink, Navbar],
  templateUrl: './saved-jobs.html'
})
export class SavedJobs implements OnInit {
  savedJobs: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';

  constructor(
    private jobService: JobService,
    private applicationService: ApplicationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadSavedJobs();
  }

  loadSavedJobs() {
    this.isLoading = true;
    this.errorMessage = '';

    this.jobService.getSavedJobs().subscribe({
      next: (response) => {
        this.savedJobs = response.data || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load saved jobs.';
        this.cdr.detectChanges();
      }
    });
  }

  removeSavedJob(jobId: number) {
    this.jobService.unsaveJob(jobId).subscribe({
      next: () => {
        this.savedJobs = this.savedJobs.filter(j => j.id !== jobId);
        this.successMessage = 'Job removed from saved.';
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Failed to remove saved job.';
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

  isJobApplied(jobId: number): boolean {
    return false;
  }
}