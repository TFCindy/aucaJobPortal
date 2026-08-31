// create-program.ts (updated)
import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { CounselorProgramService } from '../../services/counselor-program';

function dateValidator(control: AbstractControl): ValidationErrors | null {
  const startDate = control.get('startDate')?.value;
  const endDate = control.get('endDate')?.value;
  
  if (startDate && endDate && endDate <= startDate) {
    return { endDateInvalid: true };
  }
  return null;
}
@Component({
  selector: 'app-create-program',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, Navbar],
  templateUrl: './create-program.html'
})
export class CreateProgram {
  form: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  selectedImage: File | null = null;
  imagePreview: string | null = null;
  publishNow = false; // New property

  constructor(
    private fb: FormBuilder,
    private programService: CounselorProgramService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      shortDescription: [''],
      isExternal: [false],
      externalOrganization: [''],
      type: ['', [Validators.required]],
      level: [''],
      startDate: ['', [Validators.required]],
      endDate: ['', [Validators.required]],
      registrationDeadline: ['', [Validators.required]],
      isOnline: [false],
      location: [''],
      venue: [''],
      meetingLink: [''],
      maxParticipants: [10, [Validators.required, Validators.min(1), Validators.max(1000)]],
      isFree: [true],
      cost: [null]}, { validators: dateValidator 
    });
  }

  onImageSelected(event: any) {
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
      this.selectedImage = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage() {
    this.selectedImage = null;
    this.imagePreview = null;
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
    this.cdr.detectChanges();
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

    if (this.selectedImage) {
      formData.append('image', this.selectedImage);
    }

    // If publishNow is checked, set status to 'published'
    if (this.publishNow) {
      formData.append('status', 'published');
    }

    this.programService.createProgram(formData).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        
        if (this.publishNow) {
          this.successMessage = 'Program created and published successfully!';
        } else {
          this.successMessage = 'Program created successfully as draft!';
        }
        
        this.cdr.detectChanges();
        setTimeout(() => {
          this.router.navigate(['/counselor/programs'], { 
            queryParams: { refresh: Date.now() } 
          });
        }, 1500);
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Failed to create program.';
        this.cdr.detectChanges();
      }
    });
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }

  goToPrograms() {
    this.router.navigate(['/counselor/programs']);
  }
}