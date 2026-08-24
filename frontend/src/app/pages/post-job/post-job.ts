import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';
import { JobTypeService } from '../../services/job-type';

@Component({
  selector: 'app-post-job',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, Navbar, RouterLink],
  templateUrl: './post-job.html'
})
export class PostJob implements OnInit {
  form: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  jobTypes: any[] = [];
  selectedJobTypeIds: number[] = [];

  constructor(
    private fb: FormBuilder,
    private jobService: JobService,
    private jobTypeService: JobTypeService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      requirements: [''],
      responsibilities: [''],
      location: ['', [Validators.required]],
      jobTypeIds: [[], [Validators.required, Validators.minLength(1)]],
      deadlineDate: [null],
      openings: [1, [Validators.required, Validators.min(1), Validators.max(100)]]
    });
  }

  ngOnInit() {
    this.loadJobTypes();
  }

  loadJobTypes() {
    this.jobTypeService.getAll().subscribe({
      next: (response) => {
        this.jobTypes = response.data || [];
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  onJobTypeChange(event: any) {
    const checkbox = event.target;
    if (checkbox.checked) {
      this.selectedJobTypeIds.push(+checkbox.value);
    } else {
      this.selectedJobTypeIds = this.selectedJobTypeIds.filter(id => id !== +checkbox.value);
    }
    this.form.patchValue({ jobTypeIds: this.selectedJobTypeIds });
    this.form.get('jobTypeIds')?.markAsTouched();
  }

  isJobTypeSelected(id: number): boolean {
    return this.selectedJobTypeIds.includes(id);
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.jobService.createJob(this.form.value).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.successMessage = 'Job posted successfully!';
        this.form.reset();
        this.selectedJobTypeIds = [];
        this.form.patchValue({ openings: 1, jobTypeIds: [] });
        this.cdr.detectChanges();
        setTimeout(() => {
          this.router.navigate(['/employer/jobs']);
        }, 2000);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Failed to post job. Please try again.';
        this.cdr.detectChanges();
      }
    });
  }
}