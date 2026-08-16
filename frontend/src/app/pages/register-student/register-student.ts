import { Component, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'ap-register-student',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './register-student.html'
})
export class RegisterStudent {
  form: FormGroup;
  errorMessage = '';
  successMessage = '';
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private authService: Auth,
    private router: Router,
    private cdr: ChangeDetectorRef   // ← add this
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      fullName: ['', Validators.required],
      studentId: ['', Validators.required],
      phone: [null, Validators.required],
      isAlumni: [false, Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      program: ['', Validators.required]
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

    this.authService.registerStudent(this.form.value).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.successMessage = 'Registration successful! Please check your email.';
        this.cdr.detectChanges();   // ← force re-render
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.message || 'Registration failed. Please try again.';
        this.cdr.detectChanges();   // ← force re-render
      }
    });
  }
}