import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ApplicationService } from '../../services/application';

@Component({
  selector: 'app-my-applications',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, Navbar],
  templateUrl: './my-applications.html'
})
export class MyApplications implements OnInit {
  applications: any[] = [];
  filteredApplications: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  statusFilter = '';
  statuses = ['pending', 'reviewed', 'shortlisted', 'hired', 'rejected'];

  constructor(
    private applicationService: ApplicationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadApplications();
  }

  loadApplications() {
    this.isLoading = true;
    this.errorMessage = '';

    this.applicationService.getMyApplications().subscribe({
      next: (response) => {
        this.applications = response.data || [];
        this.filteredApplications = [...this.applications];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load applications.';
        this.cdr.detectChanges();
      }
    });
  }

  applyFilters() {
    this.filteredApplications = this.applications.filter(app => {
      const matchesSearch = !this.searchTerm ||
        app.jobTitle?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        app.companyName?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesStatus = !this.statusFilter || app.status === this.statusFilter;
      
      return matchesSearch && matchesStatus;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.statusFilter = '';
    this.filteredApplications = [...this.applications];
    this.cdr.detectChanges();
  }

  getStatusBadge(status: string): string {
    const badgeMap: Record<string, string> = {
      'pending': 'bg-yellow-100 text-yellow-700',
      'reviewed': 'bg-blue-100 text-blue-700',
      'shortlisted': 'bg-purple-100 text-purple-700',
      'hired': 'bg-green-100 text-green-700',
      'rejected': 'bg-red-100 text-red-700'
    };
    return badgeMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }
}