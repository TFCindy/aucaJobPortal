// src/app/pages/student-programs/student-programs.ts
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Navbar } from '../../components/navbar/navbar';
import { CounselorProgramService } from '../../services/counselor-program';
import { ProfileService } from '../../services/profile';
import { ApplicationService } from '../../services/application';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-student-programs',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, ReactiveFormsModule, Navbar],
  templateUrl: './student-programs.html'
})
export class StudentPrograms implements OnInit {
  programs: any[] = [];
  filteredPrograms: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  typeFilter = '';
  appliedProgramIds: Set<number> = new Set(); 
  
  // Application Modal
  showApplyModal = false;
  selectedProgram: any = null;
  applicationForm: FormGroup;
  isSubmitting = false;
  studentProfile: any = null;
  
  types = ['training', 'workshop', 'seminar', 'webinar', 'bootcamp'];

  constructor(
    private programService: CounselorProgramService,
    private profileService: ProfileService,
    private applicationService: ApplicationService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.applicationForm = this.fb.group({
      programId: ['', Validators.required],
      motivationLetter: [''],
      currentProgram: ['', Validators.required],
      currentYearOfStudy: ['', [Validators.required, Validators.min(1), Validators.max(4)]]
    });
  }

  ngOnInit() {
    this.loadPrograms();
    this.loadStudentProfile();
    this.loadAppliedPrograms();
  }

  loadStudentProfile() {
    this.profileService.getStudentProfile().subscribe({
      next: (response: any) => {
        this.studentProfile = response.data;
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  loadAppliedPrograms() {
  this.programService.getMyProgramApplications().subscribe({
    next: (response: any) => {
      const applications = response.data || [];
      this.appliedProgramIds = new Set(
        applications
          .filter((app: any) => app.status === 'pending' || app.status === 'approved' || app.status === 'waitlisted')
          .map((app: any) => app.programId)
      );
      this.cdr.detectChanges();
    },
    error: (err: any) => {
      console.error('Failed to load applications:', err);
    }
  });
}

  loadPrograms() {
    this.isLoading = true;
    this.errorMessage = '';

    this.programService.getAvailablePrograms().subscribe({
      next: (response: any) => {
        this.programs = response.data || [];
        this.filteredPrograms = [...this.programs];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load programs.';
        this.cdr.detectChanges();
      }
    });
  }

  hasApplied(programId: number): boolean {
    return this.appliedProgramIds.has(programId);
  }

  applyFilters() {
    this.filteredPrograms = this.programs.filter((program: any) => {
      const matchesSearch = !this.searchTerm ||
        program.title?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        program.type?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        program.description?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesType = !this.typeFilter || program.type === this.typeFilter;
      
      return matchesSearch && matchesType;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.typeFilter = '';
    this.filteredPrograms = [...this.programs];
    this.cdr.detectChanges();
  }

  openApplyModal(program: any) {
    this.selectedProgram = program;
    this.errorMessage = '';
    this.successMessage = '';
    
    // Prefill the form with student profile data
    this.applicationForm.patchValue({
      programId: program.id,
      currentProgram: this.studentProfile?.program || '',
      currentYearOfStudy: this.studentProfile?.yearOfStudy || 1
    });
    
    this.showApplyModal = true;
    this.cdr.detectChanges();
  }

  closeApplyModal() {
    this.showApplyModal = false;
    this.selectedProgram = null;
    this.applicationForm.reset();
    this.isSubmitting = false;
    this.cdr.detectChanges();
  }

  onSubmitApplication() {
    if (this.applicationForm.invalid) {
      Object.keys(this.applicationForm.controls).forEach(key => {
        this.applicationForm.get(key)?.markAsTouched();
      });
      this.errorMessage = 'Please fill in all required fields.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formData = this.applicationForm.value;

    this.programService.applyToProgram(formData.programId, {
      programId: formData.programId,
      motivationLetter: formData.motivationLetter,
      currentProgram: formData.currentProgram,
      currentYearOfStudy: formData.currentYearOfStudy
    }).subscribe({
      next: (response: any) => {
        this.isSubmitting = false;
        this.successMessage = 'Application submitted successfully!';
        this.appliedProgramIds.add(formData.programId);
        this.cdr.detectChanges();
        
        setTimeout(() => {
          this.closeApplyModal();
          this.loadPrograms(); // Refresh to update participant counts
        }, 2000);
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Failed to submit application.';
        this.cdr.detectChanges();
      }
    });
  }

  getImageUrl(imagePath: string): string {
    if (!imagePath) return '';
    const baseUrl = environment.apiUrl.replace('/api', '');
    return `${baseUrl}${imagePath}`;
  }

  getTypeLabel(type: string): string {
    const typeMap: Record<string, string> = {
      'training': 'Training',
      'workshop': 'Workshop',
      'seminar': 'Seminar',
      'webinar': 'Webinar',
      'bootcamp': 'Bootcamp'
    };
    return typeMap[type?.toLowerCase()] || type || 'Program';
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