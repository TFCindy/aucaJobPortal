import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { JobService } from '../../services/job';
import { ApplicationService } from '../../services/application';
import { ProfileService } from '../../services/profile';


@Component({
  selector: 'app-job-details-student',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, Navbar],
  templateUrl: './job-details-student.html'
})
export class JobDetailsStudent implements OnInit {
  job: any = null;
  jobId: number = 0; 
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  isSubmitting = false;
  isApplied = false;
  showApplicationForm = false;

  form: FormGroup;
  existingCvPath: string | null = null;
  existingCvFileName: string = '';
  selectedFile: File | null = null;

  studentName = '';
  studentEmail = '';
  studentProgram = '';
  studentYearOfStudy: number | null = null;
  studentIdNumber = '';

  constructor(
    private fb: FormBuilder,
    private jobService: JobService,
    private applicationService: ApplicationService,
    private profileService: ProfileService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      jobId: ['', Validators.required],
      coverLetter: [''],
      professionalExperience: [''],
      skills: ['', Validators.required],
      currentYearOfStudy: [null],
      useExistingCv: [true]
    });
  }

  ngOnInit() {
  this.route.paramMap.subscribe(params => {
    const jobId = params.get('id');
    if (jobId) {
      this.jobId = +jobId;
      this.form.patchValue({ jobId: this.jobId });
      this.loadJob(this.jobId);
      this.loadStudentProfile();
      this.checkApplicationStatus(this.jobId);
    }
  });
}

  loadJob(jobId: number) {
    this.isLoading = true;
    this.errorMessage = '';

    this.jobService.getJobById(jobId).subscribe({
      next: (response) => {
        this.job = response.data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load job details.';
        this.cdr.detectChanges();
      }
    });
  }

  loadStudentProfile() {
    this.profileService.getStudentProfile().subscribe({
      next: (response) => {
        const profile = response.data;
        
        // ─── GET STUDENT INFO FROM PROFILE ───
        this.studentName = profile.fullName || '';
        this.studentEmail = profile.email || '';
        this.studentProgram = profile.program || '';
        this.studentYearOfStudy = profile.yearOfStudy || null;
        this.studentIdNumber = profile.studentId || '';
        
        this.existingCvPath = profile.cvFilePath;
        if (this.existingCvPath) {
          this.existingCvFileName = this.existingCvPath.split('/').pop() || '';
        }
        
        this.form.patchValue({
          currentYearOfStudy: profile.yearOfStudy
        });
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  checkApplicationStatus(jobId: number) {
    this.applicationService.checkApplicationStatus(jobId).subscribe({
      next: (response) => {
        this.isApplied = response.data?.hasApplied || false;
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  openApplicationForm() {
    this.showApplicationForm = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  closeApplicationForm() {
    this.showApplicationForm = false;
    this.cdr.detectChanges();
  }

  onCvOptionChange() {
    this.selectedFile = null;
    this.cdr.detectChanges();
  }

  onFileSelected(event: any) {
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
      this.selectedFile = file;
      this.cdr.detectChanges();
    }
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formData = new FormData();

    const payload: any = {
      jobId: this.form.value.jobId,
      coverLetter: this.form.value.coverLetter,
      professionalExperience: this.form.value.professionalExperience,
      skills: this.form.value.skills,
      currentYearOfStudy: this.form.value.currentYearOfStudy,
      useExistingCv: this.form.value.useExistingCv
    };

    Object.keys(payload).forEach(key => {
      if (payload[key] !== null && payload[key] !== undefined) {
        formData.append(key, payload[key]);
      }
    });

     if (this.form.invalid) {
    this.errorMessage = 'Please fill in all required fields.';
    return;
  }

    if (!this.form.value.useExistingCv && this.selectedFile) {
      formData.append('newCvFile', this.selectedFile);
    }

    this.applicationService.applyToJob(this.job.id, formData).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.isApplied = true;
        this.successMessage = response.message || 'Application submitted successfully!';
        this.showApplicationForm = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Failed to submit application.';
        this.cdr.detectChanges();
      }
    });
  }

  getStatusBadge(status: string): string {
    const badgeMap: Record<string, string> = {
      'pending': 'bg-yellow-100 text-yellow-700',
      'active': 'bg-green-100 text-green-700',
      'rejected': 'bg-red-100 text-red-700'
    };
    return badgeMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  getJobTypeBadge(type: string): string {
    const badgeMap: Record<string, string> = {
      'full-time': 'bg-blue-100 text-blue-700',
      'part-time': 'bg-yellow-100 text-yellow-700',
      'internship': 'bg-green-100 text-green-700',
      'contract': 'bg-purple-100 text-purple-700',
      'remote': 'bg-indigo-100 text-indigo-700',
      'hybrid': 'bg-pink-100 text-pink-700',
      'freelance': 'bg-orange-100 text-orange-700',
      'temporary': 'bg-gray-100 text-gray-700'
    };
    return badgeMap[type?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  goBack() {
    this.router.navigate(['student/jobs']);
  }
}