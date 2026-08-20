// src/app/pages/application-details/application-details.ts
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ApplicationService } from '../../services/application';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-application-details',
  standalone: true,
  imports: [CommonModule, Navbar, RouterLink, FormsModule],
  templateUrl: './application-details.html'
})
export class ApplicationDetails implements OnInit {
  application: any = null;
  isLoading = true;
  errorMessage = '';
  isUpdating = false;
  updateSuccess = '';

  statuses = ['pending', 'reviewed', 'shortlisted', 'hired', 'rejected'];
  selectedStatus = '';

  constructor(
    private applicationService: ApplicationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const appId = this.route.snapshot.paramMap.get('id');
    if (appId) {
      this.loadApplication(+appId);
    }
  }

  loadApplication(appId: number) {
    this.isLoading = true;
    this.errorMessage = '';

    this.applicationService.getApplicationById(appId).subscribe({
      next: (response) => {
        this.application = response.data;
        this.selectedStatus = this.application.status || 'pending';
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load application details.';
        this.cdr.detectChanges();
      }
    });
  }

  updateStatus() {
    if (!this.application || this.selectedStatus === this.application.status) {
      return;
    }

    this.isUpdating = true;
    this.errorMessage = '';
    this.updateSuccess = '';

    this.applicationService.updateApplicationStatus(this.application.id, this.selectedStatus).subscribe({
      next: () => {
        this.application.status = this.selectedStatus;
        this.isUpdating = false;
        this.updateSuccess = 'Status updated successfully.';
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isUpdating = false;
        this.errorMessage = err.error?.message || 'Failed to update status.';
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

  goBack() {
    this.router.navigate(['/employer/applications']);
  }
}