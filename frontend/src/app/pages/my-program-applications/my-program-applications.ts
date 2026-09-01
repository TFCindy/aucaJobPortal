// src/app/pages/my-program-applications/my-program-applications.ts
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { CounselorProgramService } from '../../services/counselor-program';

@Component({
  selector: 'app-my-program-applications',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, Navbar],
  templateUrl: './my-program-applications.html'
})
export class MyProgramApplications implements OnInit {
  applications: any[] = [];
  filteredApplications: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  statusFilter = '';
  statuses = ['pending', 'approved', 'rejected'];

  constructor(
    private programService: CounselorProgramService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadApplications();
  }

  loadApplications() {
    this.isLoading = true;
    this.errorMessage = '';

    this.programService.getMyProgramApplications().subscribe({
      next: (response: any) => {
        this.applications = response.data || [];
        this.filteredApplications = [...this.applications];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load program applications.';
        this.cdr.detectChanges();
      }
    });
  }

  applyFilters() {
    this.filteredApplications = this.applications.filter(app => {
      const matchesSearch = !this.searchTerm ||
        app.programTitle?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        app.studentName?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
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
      month: 'short', 
      day: 'numeric', 
      year: 'numeric' 
    });
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }
}