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
  unreadCount = 0;
  isDropdownOpen = false;

  constructor(
    private router: Router,
    private notificationService: NotificationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.userFullName = user.fullName;
      this.userInitials = this.getInitials(user.fullName);
    }

    this.notificationService.getUnreadCount().subscribe({
      next: (response) => {
        this.unreadCount = response.data.unreadCount;
        this.cdr.detectChanges();
      },
      error: () => {
        // Silently fail — a notification count isn't critical enough to show an error for
      }
    });
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