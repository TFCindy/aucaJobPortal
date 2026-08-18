import { Component, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Auth } from '../../services/auth';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register-employer',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './register-employer.html'
})
export class RegisterEmployer {
  form: FormGroup;
  errorMessage = '';
  successMessage = '';
  isSubmitting = false;

  constructor(
  private fb: FormBuilder,
  private authService: Auth,
  private router: Router,
  private cdr: ChangeDetectorRef
) {
  this.form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    phone: ['', Validators.required],
    companyName: ['', Validators.required],
    companyDescription: ['', Validators.required],
    companyWebsite: [''],
    industry: ['', Validators.required],
    companySize: [''],
    location: ['', Validators.required],
    tin: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]]
  });
}

  onSubmit() {
    Object.keys(this.form.controls).forEach(key => {
      this.form.get(key)?.markAsTouched();
    });

    if (this.form.invalid) {
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.registerEmployer(this.form.value).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.successMessage = response.message || 'Registration successful! Please check your email.';
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Registration failed. Please try again.';
        this.cdr.detectChanges();
      }
    });
  }
}