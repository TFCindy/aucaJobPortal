import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-users.html'
})
export class AdminUsers implements OnInit {
  users: any[] = [];
  filteredUsers: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  roleFilter = '';
  statusFilter = '';

  constructor(
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.userService.getUsers().subscribe({
      next: (response) => {
        this.users = response.data || [];
        this.filteredUsers = [...this.users];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load users.';
        this.cdr.detectChanges();
      }
    });
  }

  applyFilters() {
    this.filteredUsers = this.users.filter(user => {
      const matchesSearch = !this.searchTerm || 
        user.fullName?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        user.email?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesRole = !this.roleFilter || 
        user.role?.toLowerCase() === this.roleFilter.toLowerCase();
      
      const matchesStatus = !this.statusFilter || 
        (this.statusFilter === 'active' ? user.isActive : !user.isActive);
      
      return matchesSearch && matchesRole && matchesStatus;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.roleFilter = '';
    this.statusFilter = '';
    this.filteredUsers = [...this.users];
    this.cdr.detectChanges();
  }

  toggleUserStatus(userId: number, currentStatus: boolean) {
    if (currentStatus) {
      this.userService.deactivateUser(userId).subscribe({
        next: () => {
          const user = this.users.find(u => u.id === userId);
          if (user) {
            user.isActive = false;
            this.successMessage = 'User deactivated successfully.';
          }
          this.applyFilters();
          this.cdr.detectChanges();
        },
        error: (err: any) => {
          this.errorMessage = err.error?.message || 'Failed to deactivate user.';
          this.cdr.detectChanges();
        }
      });
    } else {
      this.userService.activateUser(userId).subscribe({
        next: () => {
          const user = this.users.find(u => u.id === userId);
          if (user) {
            user.isActive = true;
            this.successMessage = 'User activated successfully.';
          }
          this.applyFilters();
          this.cdr.detectChanges();
        },
        error: (err: any) => {
          this.errorMessage = err.error?.message || 'Failed to activate user.';
          this.cdr.detectChanges();
        }
      });
    }
  }

  getRoleBadge(role: string): string {
    const badgeMap: Record<string, string> = {
      'admin': 'bg-red-100 text-red-700',
      'employer': 'bg-blue-100 text-blue-700',
      'student': 'bg-green-100 text-green-700',
      'career_counselor': 'bg-purple-100 text-purple-700'
    };
    return badgeMap[role?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }
}