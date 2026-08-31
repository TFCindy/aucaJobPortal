import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CounselorService } from '../../services/counselor';

@Component({
  selector: 'app-admin-counselors',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin-counselors.html'
})
export class AdminCounselors implements OnInit {
  counselors: any[] = [];
  filteredCounselors: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  showCreateModal = false;
  isSubmitting = false;

  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private counselorService: CounselorService,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      fullName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      department: ['', [Validators.required]],
      title: ['', [Validators.required]],
      bio: [''],
      phone: ['', [Validators.required]],
      specialization: [''],
      yearsOfExperience: [null],
      officeLocation: ['']
    });
  }

  ngOnInit() {
    this.loadCounselors();
  }

  loadCounselors() {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.counselorService.getCounselors().subscribe({
      next: (response) => {
        this.counselors = response.data || [];
        this.filteredCounselors = [...this.counselors];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load counselors.';
        this.cdr.detectChanges();
      }
    });
  }

  applyFilters() {
    this.filteredCounselors = this.counselors.filter(counselor => {
      const matchesSearch = !this.searchTerm ||
        counselor.fullName?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        counselor.email?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        counselor.department?.toLowerCase().includes(this.searchTerm.toLowerCase());
      return matchesSearch;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.filteredCounselors = [...this.counselors];
    this.cdr.detectChanges();
  }

  openCreateModal() {
    this.showCreateModal = true;
    this.form.reset();
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  closeCreateModal() {
    this.showCreateModal = false;
    this.form.reset();
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  createCounselor() {
    if (this.form.invalid) {
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.counselorService.createCounselor(this.form.value).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.successMessage = 'Counselor created successfully.';
        this.showCreateModal = false;
        this.form.reset();
        this.loadCounselors();
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Failed to create counselor.';
        this.cdr.detectChanges();
      }
    });
  }

  toggleCounselorStatus(counselorId: number, currentStatus: boolean) {
    if (currentStatus) {
      this.counselorService.deactivateCounselor(counselorId).subscribe({
        next: () => {
          const counselor = this.counselors.find(c => c.id === counselorId);
          if (counselor) {
            counselor.isActive = false;
            this.successMessage = 'Counselor deactivated successfully.';
          }
          this.applyFilters();
          this.cdr.detectChanges();
        },
        error: (err: any) => {
          this.errorMessage = err.error?.message || 'Failed to deactivate counselor.';
          this.cdr.detectChanges();
        }
      });
    } else {
      this.counselorService.activateCounselor(counselorId).subscribe({
        next: () => {
          const counselor = this.counselors.find(c => c.id === counselorId);
          if (counselor) {
            counselor.isActive = true;
            this.successMessage = 'Counselor activated successfully.';
          }
          this.applyFilters();
          this.cdr.detectChanges();
        },
        error: (err: any) => {
          this.errorMessage = err.error?.message || 'Failed to activate counselor.';
          this.cdr.detectChanges();
        }
      });
    }
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }

  getStatusBadge(isActive: boolean): string {
    return isActive
      ? 'bg-green-100 text-green-700'
      : 'bg-red-100 text-red-700';
  }
}