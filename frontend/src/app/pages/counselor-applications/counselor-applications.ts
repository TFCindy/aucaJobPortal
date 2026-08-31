// src/app/pages/counselor-applications/counselor-applications.ts
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Navbar } from '../../components/navbar/navbar';
import { CounselorProgramService } from '../../services/counselor-program';

@Component({
  selector: 'app-counselor-applications',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, Navbar],
  templateUrl: './counselor-applications.html'
})
export class CounselorApplications implements OnInit {
  applications: any[] = [];
  filteredApplications: any[] = [];
  programs: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  statusFilter = '';
  selectedProgramFilter = '';
  showStatusModal = false;
  selectedApplication: any = null;
  newStatus = '';
  isUpdating = false;

  statuses = ['pending', 'approved', 'rejected', 'waitlisted'];
  statusColors: any = {
    'pending': 'bg-yellow-100 text-yellow-700',
    'approved': 'bg-green-100 text-green-700',
    'rejected': 'bg-red-100 text-red-700',
    'waitlisted': 'bg-blue-100 text-blue-700'
  };
  statusLabels: any = {
    'pending': 'Pending',
    'approved': 'Approved',
    'rejected': 'Rejected',
    'waitlisted': 'Waitlisted'
  };

  constructor(
    private programService: CounselorProgramService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    this.errorMessage = '';

    // First get all programs for this counselor
    this.programService.getMyPrograms().subscribe({
      next: (response: any) => {
        this.programs = response.data || [];
        this.loadAllApplications();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load programs.';
        this.cdr.detectChanges();
      }
    });
  }

  loadAllApplications() {
    if (this.programs.length === 0) {
      this.isLoading = false;
      this.applications = [];
      this.filteredApplications = [];
      this.cdr.detectChanges();
      return;
    }

    let completed = 0;
    const allApplications: any[] = [];

    // Get applications for each program
    this.programs.forEach((program: any) => {
      this.programService.getProgramApplications(program.id).subscribe({
        next: (response: any) => {
          const apps = (response.data || []).map((app: any) => ({
            ...app,
            programTitle: program.title
          }));
          allApplications.push(...apps);
          completed++;

          if (completed === this.programs.length) {
            this.applications = allApplications;
            this.filteredApplications = [...this.applications];
            this.isLoading = false;
            this.cdr.detectChanges();
          }
        },
        error: (err: any) => {
          completed++;
          if (completed === this.programs.length) {
            this.applications = allApplications;
            this.filteredApplications = [...this.applications];
            this.isLoading = false;
            this.cdr.detectChanges();
          }
        }
      });
    });
  }

  applyFilters() {
    this.filteredApplications = this.applications.filter((app: any) => {
      const matchesSearch = !this.searchTerm ||
        app.studentName?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        app.studentEmail?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        app.programTitle?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesStatus = !this.statusFilter || app.status === this.statusFilter;
      const matchesProgram = !this.selectedProgramFilter || app.programId === +this.selectedProgramFilter;
      
      return matchesSearch && matchesStatus && matchesProgram;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.statusFilter = '';
    this.selectedProgramFilter = '';
    this.filteredApplications = [...this.applications];
    this.cdr.detectChanges();
  }

  openStatusModal(application: any) {
    this.selectedApplication = application;
    this.newStatus = application.status;
    this.showStatusModal = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  closeStatusModal() {
    this.showStatusModal = false;
    this.selectedApplication = null;
    this.newStatus = '';
    this.cdr.detectChanges();
  }

  updateStatus() {
    if (!this.selectedApplication || !this.newStatus) return;

    this.isUpdating = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.programService.updateApplicationStatus(this.selectedApplication.id, this.newStatus).subscribe({
      next: (response: any) => {
        this.isUpdating = false;
        this.successMessage = `Application status updated to ${this.getStatusLabel(this.newStatus)}!`;
        this.showStatusModal = false;
        
        // Update the application in the list
        const index = this.applications.findIndex((a: any) => a.id === this.selectedApplication.id);
        if (index !== -1) {
          this.applications[index] = response.data;
          this.filteredApplications = [...this.applications];
        }
        
        this.cdr.detectChanges();
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error: (err: any) => {
        this.isUpdating = false;
        this.errorMessage = err.error?.message || 'Failed to update application status.';
        this.cdr.detectChanges();
      }
    });
  }

  showMotivationLetter(application: any) {
    this.selectedApplication = application;
    this.cdr.detectChanges();
  }

  closeMotivationModal() {
    this.selectedApplication = null;
    this.cdr.detectChanges();
  }

  getStatusColor(status: string): string {
    return this.statusColors[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  getStatusLabel(status: string): string {
    return this.statusLabels[status?.toLowerCase()] || status || 'Unknown';
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }

  formatDate(date: any): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric', 
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getStatusCount(status: string): number {
    return this.applications.filter((a: any) => a.status === status).length;
  }

  getProgramTitle(programId: number): string {
    const program = this.programs.find((p: any) => p.id === programId);
    return program?.title || 'Unknown Program';
  }
}