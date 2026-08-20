import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';
import { JobTypeService } from '../../services/job-type';  // ← Use this

@Component({
    selector: 'app-update-job',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, Navbar, RouterLink],
    templateUrl: './update-job.html'
})
export class UpdateJob implements OnInit {
    form: FormGroup;
    jobId: number = 0;
    isLoading = true;
    isSubmitting = false;
    errorMessage = '';
    successMessage = '';
    jobTypes: any[] = [];
    selectedJobTypeIds: number[] = [];

    constructor(
        private fb: FormBuilder,
        private jobService: JobService,
        private jobTypeService: JobTypeService,  // ← Use this
        private route: ActivatedRoute,
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
        this.jobId = +this.route.snapshot.paramMap.get('id')!;
        this.loadJobTypes();
        this.loadJob();
    }

    loadJobTypes() {
        this.jobTypeService.getAll().subscribe({
            next: (response) => {
                this.jobTypes = response.data || [];
                this.cdr.detectChanges();
            },
            error: (err) => {
                console.error('Error loading job types:', err);
            }
        });
    }

    loadJob() {
        this.isLoading = true;
        this.jobService.getJobById(this.jobId).subscribe({
            next: (response) => {
                const job = response.data;
                this.selectedJobTypeIds = job.jobTypeIds || [];
                this.form.patchValue({
                    title: job.title,
                    description: job.description,
                    requirements: job.requirements,
                    responsibilities: job.responsibilities,
                    location: job.location,
                    jobTypeIds: this.selectedJobTypeIds,
                    deadline: job.deadline,
                    openings: job.openings
                });
                this.isLoading = false;
                this.cdr.detectChanges();
            },
            error: (err) => {
                this.isLoading = false;
                this.errorMessage = err.error?.message || 'Failed to load job.';
                this.cdr.detectChanges();
            }
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

    onUpdate() {
        if (this.form.invalid) {
            return;
        }

        this.isSubmitting = true;
        this.errorMessage = '';
        this.successMessage = '';

        this.jobService.updateJob(this.jobId, this.form.value).subscribe({
            next: () => {
                this.isSubmitting = false;
                this.successMessage = 'Job updated successfully!';
                this.cdr.detectChanges();
                setTimeout(() => {
                    this.router.navigate(['/employer/jobs', this.jobId]);
                }, 1500);
            },
            error: (err) => {
                this.isSubmitting = false;
                this.errorMessage = err.error?.message || 'Failed to update job.';
                this.cdr.detectChanges();
            }
        });
    }
}