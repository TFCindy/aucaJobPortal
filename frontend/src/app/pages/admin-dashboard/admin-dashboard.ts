import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AdminService } from '../../services/admin';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],  // ← ADD CommonModule
  templateUrl: './admin-dashboard.html'
})
export class AdminDashboard implements OnInit {
  stats = {
    totalUsers: 0,
    totalJobs: 0,
    pendingJobs: 0,
    totalCounselors: 0
  };
  recentUsers: any[] = [];
  recentJobs: any[] = [];
  isLoading = true;
  errorMessage = '';
  lastUpdated: Date = new Date();  // ← ADD THIS

  constructor(
    private adminService: AdminService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadDashboard();
  }

  loadDashboard() {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminService.getDashboardStats().subscribe({
      next: (response) => {
        this.stats = response.data;
        this.recentUsers = response.data.recentUsers || [];
        this.recentJobs = response.data.recentJobs || [];
        this.lastUpdated = new Date();
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load dashboard stats.';
        this.cdr.detectChanges();
      }
    });
  }
}