import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.html'
})
export class Navbar implements OnInit {
  userInitials = '';
  userFullName = '';
  userRole: string | null = null;
  unreadCount = 0;
  isDropdownOpen = false;
  navLinks: any[] = [];

  constructor(
    private router: Router,
    private notificationService: NotificationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.userFullName = user.fullName || user.companyName || 'User';
      if (user.role) {
        this.userRole = user.role.toLowerCase(); 
      } else if (user.userType) {
        this.userRole = user.userType.toLowerCase();
      } else {
        this.userRole = 'student';  
      }
      
      this.userInitials = this.getInitials(this.userFullName);
    }

    this.setNavLinks();

    this.notificationService.getUnreadCount().subscribe({
      next: (response) => {
        this.unreadCount = response.data.unreadCount;
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  setNavLinks() {
    this.navLinks = [];

    if (this.userRole === 'employer') {
      this.navLinks = [
        { label: 'Dashboard', path: '/employer/dashboard' },
        { label: 'My Jobs', path: '/employer/jobs' },
        { label: 'Applications', path: '/employer/applications' },
        { label: 'Post a Job', path: '/employer/jobs/post' }
      ];
    } else {
      this.navLinks = [
        { label: 'Home', path: '/student/dashboard' },
        { label: 'Browse Jobs', path: '/jobs' },
        { label: 'My Applications', path: '/applications' },
        { label: 'Saved Jobs', path: '/saved-jobs' }
      ];
    }
  }

  getInitials(fullName: string): string {
    const parts = fullName.trim().split(' ');
    if (parts.length === 1) {
      return parts[0].charAt(0).toUpperCase();
    }
    return (parts[0].charAt(0) + parts[parts.length - 1].charAt(0)).toUpperCase();
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }
}