import { Component, OnInit, ChangeDetectorRef, HostListener } from '@angular/core';
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
  showNotifications = false;
  notifications: any[] = [];
  navLinks: any[] = [];
  showSuccessMessage = false;
  successMessage = '';

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
        this.unreadCount = response.data?.unreadCount || 0;
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
    }else if (this.userRole === 'career_counselor') {
    this.navLinks = [
      { label: 'Dashboard', path: '/counselor/dashboard' },
      { label: 'My Programs', path: '/counselor/programs' },
      { label: 'Applications', path: '/counselor/applications' }
    ];
  }else {
      this.navLinks = [
        { label: 'Home', path: '/student/dashboard' },
        { label: 'Browse Jobs', path: '/student/jobs' },
        { label: 'My Applications', path: '/student/applications' },
        { label: 'Career Programs', path: '/student/programs' },
        { label: 'Saved Jobs', path: '/student/saved-jobs' }
        
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

  // ─── NOTIFICATION DROPDOWN METHODS ───

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
    if (this.showNotifications) {
      this.loadNotifications();
    }
  }

  loadNotifications() {
    this.notificationService.getMyNotifications().subscribe({
      next: (response) => {
        this.notifications = response.data || [];
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  markNotificationAsRead(notificationId: number) {
    const notification = this.notifications.find(n => n.id === notificationId);
    if (!notification || notification.isRead) return;

    this.notificationService.markAsRead(notificationId).subscribe({
      next: () => {
        notification.isRead = true;
        this.unreadCount = Math.max(0, this.unreadCount - 1);
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  markAllAsRead() {
    this.notificationService.markAllAsRead().subscribe({
      next: () => {
        this.notifications.forEach(n => n.isRead = true);
        this.unreadCount = 0;
        this.showSuccessMessage = true;
      this.successMessage = 'All notifications marked as read';
      
      // ─── HIDE MESSAGE AFTER 3 SECONDS ───
      setTimeout(() => {
        this.showSuccessMessage = false;
        this.successMessage = '';
      }, 3000);
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  // ─── CLOSE DROPDOWN WHEN CLICKING OUTSIDE ───
 @HostListener('document:click', ['$event'])
onClickOutside(event: any) {
  const target = event.target as HTMLElement;
  if (!target.closest('.notification-container')) {
    this.showNotifications = false;
  }
}

  // ─── CLOSE DROPDOWN ON ESCAPE KEY ───
  @HostListener('document:keydown.escape')
  onEscapeKey() {
    this.showNotifications = false;
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }
}