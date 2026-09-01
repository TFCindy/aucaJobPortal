import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ProfileService } from '../../services/profile';

@Component({
  selector: 'app-counselor-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, Navbar],
  templateUrl: './counselor-profile.html'
})
export class CounselorProfile implements OnInit {
  form: FormGroup;
  isLoading = true;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  counselorId: number = 0;

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      department: ['', Validators.required],
      title: ['', Validators.required],
      bio: [''],
      specialization: [''],
      yearsOfExperience: [null, [Validators.min(0), Validators.max(50)]],
      officeLocation: ['']
    });
  }

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.isLoading = true;
    this.errorMessage = '';

    this.profileService.getCareerCounselorProfile().subscribe({
      next: (response: any) => {
        const profile = response.data;
        this.counselorId = profile.id;

        this.form.patchValue({
          fullName: profile.fullName || '',
          email: profile.email || '',
          phone: profile.phone || '',
          department: profile.department || '',
          title: profile.title || '',
          bio: profile.bio || '',
          specialization: profile.specialization || '',
          yearsOfExperience: profile.yearsOfExperience || null,
          officeLocation: profile.officeLocation || ''
        });

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load profile.';
        this.cdr.detectChanges();
      }
    });
  }

  onSubmit() {
    // Mark all fields as touched
    Object.keys(this.form.controls).forEach(key => {
      this.form.get(key)?.markAsTouched();
    });

    if (this.form.invalid) {
      this.errorMessage = 'Please fill in all required fields.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    const payload = {
      department: this.form.value.department,
      title: this.form.value.title,
      bio: this.form.value.bio,
      specialization: this.form.value.specialization,
      yearsOfExperience: this.form.value.yearsOfExperience,
      officeLocation: this.form.value.officeLocation,
      phone: this.form.value.phone
    };

    this.profileService.updateCareerCounselorProfile(payload).subscribe({
      next: (response: any) => {
        this.isSubmitting = false;
        this.successMessage = 'Profile updated successfully!';
        this.cdr.detectChanges();
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Failed to update profile.';
        this.cdr.detectChanges();
      }
    });
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }

  goToDashboard() {
    this.router.navigate(['/counselor/dashboard']);
  }
}