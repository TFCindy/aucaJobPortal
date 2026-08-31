// edit-program.ts
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { CounselorProgramService } from '../../services/counselor-program';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-edit-program',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, Navbar],
  templateUrl: './edit-program.html'
})
export class EditProgram implements OnInit {
  form: FormGroup;
  isSubmitting = false;
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  selectedImage: File | null = null;
  imagePreview: string | null = null;
  existingImageUrl: string | null = null;
  programId: number = 0;
  program: any = null; // ✅ Changed from CareerProgram to any
  publishNow = false;

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
      cost: [null],
      status: [''],
      isFeatured: [false]
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.programId = +params['id'];
      this.loadProgram();
    });
  }

  loadProgram() {
    this.isLoading = true;
    this.errorMessage = '';

    this.programService.getProgramById(this.programId).subscribe({
      next: (response: any) => {
        this.program = response.data;
        this.populateForm(this.program);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load program.';
        this.cdr.detectChanges();
      }
    });
  }

  populateForm(program: any) { // ✅ Changed from CareerProgram to any
    if (!program) return;

    // Set publishNow based on current status
    this.publishNow = program.status === 'published';

    // Format dates for input fields (YYYY-MM-DD)
    const formatDate = (date: any) => {
      if (!date) return '';
      const d = new Date(date);
      return d.toISOString().split('T')[0];
    };

    this.form.patchValue({
      title: program.title || '',
      description: program.description || '',
      shortDescription: program.shortDescription || '',
      isExternal: program.isExternal || false,
      externalOrganization: program.externalOrganization || '',
      type: program.type || '',
      level: program.level || '',
      startDate: formatDate(program.startDate),
      endDate: formatDate(program.endDate),
      registrationDeadline: formatDate(program.registrationDeadline),
      isOnline: program.isOnline || false,
      location: program.location || '',
      venue: program.venue || '',
      meetingLink: program.meetingLink || '',
      maxParticipants: program.maxParticipants || 10,
      isFree: program.isFree !== undefined ? program.isFree : true,
      cost: program.cost || null,
      status: program.status || 'draft',
      isFeatured: program.isFeatured || false
    });

    // Set existing image preview
    if (program.imageUrl) {
      const baseUrl = environment.apiUrl.replace('/api', '');
      this.existingImageUrl = `${baseUrl}${program.imageUrl}`;
      this.imagePreview = this.existingImageUrl;
    }

    this.cdr.detectChanges();
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
    this.imagePreview = this.existingImageUrl;
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

    // If publishNow is checked, set status to 'published'
    if (this.publishNow) {
      formData.append('status', 'published');
    }

    if (this.selectedImage) {
      formData.append('image', this.selectedImage);
    }

    this.programService.updateProgram(this.programId, formData).subscribe({
      next: (response: any) => {
        this.isSubmitting = false;
        this.successMessage = 'Program updated successfully!';
        this.cdr.detectChanges();
        setTimeout(() => {
          this.router.navigate(['/counselor/programs'], { 
            queryParams: { refresh: Date.now() } 
          });
        }, 1500);
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Failed to update program.';
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

  getImageUrl(imagePath: string): string {
    if (!imagePath) return '';
    const baseUrl = environment.apiUrl.replace('/api', '');
    return `${baseUrl}${imagePath}`;
  }
}