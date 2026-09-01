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
  isAlumni = false;
  
  profilePicturePreview: string | null = null;
  existingCvPath: string | null = null;
  existingTranscriptPath: string | null = null;
  existingDiplomaPath: string | null = null;
  
  selectedFile: File | null = null;
  selectedCvFile: File | null = null;
  selectedTranscriptFile: File | null = null;
  selectedDiplomaFile: File | null = null;
  employmentStatuses = ['Employed', 'Self-Employed', 'Unemployed'];
  degreeClassifications = ['Grand distinction', 'Distinction','Satisfaction'];

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
      skills: [''],
      bio: [''],
      emailNotificationsEnabled: [true],
      
      // Alumni fields
      graduationYear: [null],
      degreeClassification: [''],
      currentEmploymentStatus: [''],
      currentEmployer: [''],
      currentJobTitle: [''],
      industry: [''],
      employmentStartDate: ['']
    });
  }

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.isLoading = true;
    this.errorMessage = '';

    this.profileService.getStudentProfile().subscribe({
      next: (response: any) => {
        const profile = response.data;
        this.isAlumni = profile.isAlumni || false;
        
        this.form.patchValue({
          fullName: profile.fullName || '',
          email: profile.email || '',
          phone: profile.phone || '',
          studentId: profile.studentId || '',
          program: profile.program || '',
          faculty: profile.faculty || '',
          department: profile.department || '',
          yearOfStudy: profile.yearOfStudy || null,
          skills: profile.skills || '',
          bio: profile.bio || '',
          emailNotificationsEnabled: profile.emailNotificationsEnabled !== undefined ? profile.emailNotificationsEnabled : true,
          
          // Alumni fields
          graduationYear: profile.graduationYear || null,
          degreeClassification: profile.degreeClassification || '',
          currentEmploymentStatus: profile.currentEmploymentStatus || '',
          currentEmployer: profile.currentEmployer || '',
          currentJobTitle: profile.currentJobTitle || '',
          industry: profile.industry || '',
          employmentStartDate: profile.employmentStartDate || ''
        });

        if (profile.profilePicture) {
          const baseUrl = environment.apiUrl.replace('/api', '');
          this.profilePicturePreview = `${baseUrl}${profile.profilePicture}`;
        }

        if (profile.cvFilePath) {
          const baseUrl = environment.apiUrl.replace('/api', '');
          this.existingCvPath = `${baseUrl}${profile.cvFilePath}`;
        }

        if (profile.transcriptFilePath) {
          const baseUrl = environment.apiUrl.replace('/api', '');
          this.existingTranscriptPath = `${baseUrl}${profile.transcriptFilePath}`;
        }

        if (profile.diplomaFilePath) {
          const baseUrl = environment.apiUrl.replace('/api', '');
          this.existingDiplomaPath = `${baseUrl}${profile.diplomaFilePath}`;
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

  onTranscriptSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      if (file.size > 5 * 1024 * 1024) {
        alert('File size must be less than 5MB');
        return;
      }
      const allowedTypes = ['application/pdf'];
      if (!allowedTypes.includes(file.type)) {
        alert('Only PDF files are allowed');
        return;
      }
      this.selectedTranscriptFile = file;
      this.cdr.detectChanges();
    }
  }

  onDiplomaSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      if (file.size > 5 * 1024 * 1024) {
        alert('File size must be less than 5MB');
        return;
      }
      const allowedTypes = ['application/pdf'];
      if (!allowedTypes.includes(file.type)) {
        alert('Only PDF files are allowed');
        return;
      }
      this.selectedDiplomaFile = file;
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

    if (this.selectedFile) {
      formData.append('profilePicture', this.selectedFile);
    }

    if (this.selectedCvFile) {
      formData.append('cvFile', this.selectedCvFile);
    }

    if (this.selectedTranscriptFile) {
      formData.append('transcriptFile', this.selectedTranscriptFile);
    }

    if (this.selectedDiplomaFile) {
      formData.append('diplomaFile', this.selectedDiplomaFile);
    }

    this.profileService.updateStudentProfile(formData).subscribe({
      next: (response: any) => {
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

        if (updatedProfile.transcriptFilePath) {
          this.existingTranscriptPath = `${baseUrl}${updatedProfile.transcriptFilePath}`;
        }

        if (updatedProfile.diplomaFilePath) {
          this.existingDiplomaPath = `${baseUrl}${updatedProfile.diplomaFilePath}`;
        }

        this.selectedFile = null;
        this.selectedCvFile = null;
        this.selectedTranscriptFile = null;
        this.selectedDiplomaFile = null;

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