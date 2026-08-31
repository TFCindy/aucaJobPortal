import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ProfileService } from '../../services/profile';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, Navbar],
  templateUrl: './student-profile.html'
})
export class StudentProfile implements OnInit {
  form: FormGroup;
  isLoading = true;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  
  profilePicturePreview: string | null = null;
  existingCvPath: string | null = null; 
  selectedFile: File | null = null;
  selectedCvFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    public router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      studentId: [''],
      program: [''],
      faculty: [''],
      department: [''],
      yearOfStudy: [null],
      graduationYear: [null],
      skills: [''],
      bio: [''],
      emailNotificationsEnabled: [true]
    });
  }

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
  this.isLoading = true;
  this.errorMessage = '';

  this.profileService.getStudentProfile().subscribe({
    next: (response) => {
      const profile = response.data;
      
      this.form.patchValue({
        fullName: profile.fullName || '',
        email: profile.email || '',
        phone: profile.phone || '',
        studentId: profile.studentId || '',
        program: profile.program || '',
        faculty: profile.faculty || '',
        department: profile.department || '',
        yearOfStudy: profile.yearOfStudy || null,
        graduationYear: profile.graduationYear || null,
        skills: profile.skills || '',
        bio: profile.bio || '',
        emailNotificationsEnabled: profile.emailNotificationsEnabled !== undefined ? profile.emailNotificationsEnabled : true
      });

      if (profile.profilePicture) {
        const baseUrl = environment.apiUrl.replace('/api', '');
        this.profilePicturePreview = `${baseUrl}${profile.profilePicture}`;
      }

      if (profile.cvFilePath) {
  const baseUrl = environment.apiUrl.replace('/api', '');
  this.existingCvPath = `${baseUrl}${profile.cvFilePath}`;
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

  onProfilePictureSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.profilePicturePreview = reader.result as string;
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(file);
    }
  }

  onCvSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      if (file.size > 5 * 1024 * 1024) {
        alert('File size must be less than 5MB');
        return;
      }
      const allowedTypes = ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];
      if (!allowedTypes.includes(file.type)) {
        alert('Only PDF, DOC, and DOCX files are allowed');
        return;
      }
      this.selectedCvFile = file;
      this.cdr.detectChanges();
    }
  }

  onSubmit() {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        this.form.get(key)?.markAsTouched();
      });
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formData = new FormData();
    
    // Add all form fields
    Object.keys(this.form.value).forEach(key => {
      const value = this.form.value[key];
      if (value !== null && value !== undefined) {
        if (typeof value === 'boolean') {
          formData.append(key, value ? 'true' : 'false');
        } else {
          formData.append(key, value);
        }
      }
    });

    if (this.selectedFile) {
      formData.append('profilePicture', this.selectedFile);
    }

    if (this.selectedCvFile) {
      formData.append('cvFile', this.selectedCvFile);
    }

    this.profileService.updateStudentProfile(formData).subscribe({
      next: (response) => {
  this.isSubmitting = false;
  this.successMessage = 'Profile updated successfully!';

  const updatedProfile = response.data;
  const baseUrl = environment.apiUrl.replace('/api', '');

  if (updatedProfile.profilePicture) {
    this.profilePicturePreview = `${baseUrl}${updatedProfile.profilePicture}`;
  }

  if (updatedProfile.cvFilePath) {
    this.existingCvPath = `${baseUrl}${updatedProfile.cvFilePath}`;
  }

  this.selectedFile = null;
  this.selectedCvFile = null;

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
}