// src/app/pages/employer-profile/employer-profile.ts
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ProfileService } from '../../services/profile';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-employer-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, Navbar],
  templateUrl: './employer-profile.html'
})
export class EmployerProfile implements OnInit {
  form: FormGroup;
  isLoading = true;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  employerId: number = 0;
  logoPreview: string | null = null;
  existingLogoUrl: string | null = null;
  selectedLogo: File | null = null;

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      companyName: ['', Validators.required],
      companyDescription: [''],
      companyWebsite: [''],
      industry: [''],
      companySize: [''],
      location: [''],
      phone: [''],
      email: ['', [Validators.required, Validators.email]],
      emailNotificationsEnabled: [true]
    });
  }

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.isLoading = true;
    this.errorMessage = '';

    this.profileService.getEmployerProfile().subscribe({
      next: (response: any) => {
        const profile = response.data;
        this.employerId = profile.id;

        this.form.patchValue({
          companyName: profile.companyName || '',
          companyDescription: profile.companyDescription || '',
          companyWebsite: profile.companyWebsite || '',
          industry: profile.industry || '',
          companySize: profile.companySize || '',
          location: profile.location || '',
          phone: profile.phone || '',
          email: profile.email || '',
          emailNotificationsEnabled: profile.emailNotificationsEnabled !== undefined ? profile.emailNotificationsEnabled : true
        });

        // Set existing logo preview
        if (profile.companyLogo) {
          const baseUrl = environment.apiUrl.replace('/api', '');
          this.existingLogoUrl = `${baseUrl}${profile.companyLogo}`;
          this.logoPreview = this.existingLogoUrl;
        }

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

  onLogoSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      if (file.size > 5 * 1024 * 1024) {
        alert('File size must be less than 5MB');
        return;
      }
      const allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
      if (!allowedTypes.includes(file.type)) {
        alert('Only JPEG, PNG, GIF, and WEBP images are allowed');
        return;
      }
      this.selectedLogo = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.logoPreview = reader.result as string;
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(file);
    }
  }

  removeLogo() {
    this.selectedLogo = null;
    this.logoPreview = this.existingLogoUrl;
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
    this.cdr.detectChanges();
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

    const formData = new FormData();
    
    // Append all form values
    Object.keys(this.form.value).forEach(key => {
      const value = this.form.value[key];
      if (value !== null && value !== undefined && value !== '') {
        if (typeof value === 'boolean') {
          formData.append(key, value ? 'true' : 'false');
        } else {
          formData.append(key, value);
        }
      }
    });

    if (this.selectedLogo) {
      formData.append('companyLogo', this.selectedLogo);
    }

    this.profileService.updateEmployerProfile(formData).subscribe({
      next: (response: any) => {
        this.isSubmitting = false;
        this.successMessage = 'Profile updated successfully!';
        
        // Update logo preview with new logo if uploaded
        if (response.data.companyLogo) {
          const baseUrl = environment.apiUrl.replace('/api', '');
          this.existingLogoUrl = `${baseUrl}${response.data.companyLogo}`;
          this.logoPreview = this.existingLogoUrl;
          this.selectedLogo = null;
        }
        
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
    this.router.navigate(['/employer/dashboard']);
  }
}