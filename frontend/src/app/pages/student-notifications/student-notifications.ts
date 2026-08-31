import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { NotificationService } from '../../services/notification';

@Component({
  selector: 'app-student-notifications',
  standalone: true,
  imports: [CommonModule, RouterLink, Navbar],
  templateUrl: './student-notifications.html'
})
export class StudentNotifications implements OnInit {
  notifications: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = ''; 
  unreadCount = 0;

  constructor(
    private notificationService: NotificationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadNotifications();
  }

  loadNotifications() {
    this.isLoading = true;
    this.notificationService.getMyNotifications().subscribe({
      next: (response) => {
        this.notifications = response.data || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load notifications.';
        this.cdr.detectChanges();
      }
    });
  }

  markAsRead(notificationId: number) {
    this.notificationService.markAsRead(notificationId).subscribe({
      next: () => {
        const notification = this.notifications.find(n => n.id === notificationId);
        if (notification) {
          notification.isRead = true;
        }
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  markAllAsRead() {
  if (this.unreadCount === 0) {
    this.successMessage = 'No unread notifications to mark.';
    setTimeout(() => { this.successMessage = ''; }, 3000);
    return;
  }

  this.notificationService.markAllAsRead().subscribe({
    next: () => {
      this.notifications.forEach(n => n.isRead = true);
      this.unreadCount = 0;
      
      // ─── SHOW SUCCESS MESSAGE ───
      this.successMessage = 'All notifications marked as read.';
      
      // ─── HIDE AFTER 3 SECONDS ───
      setTimeout(() => {
        this.successMessage = '';
      }, 3000);
      
      this.cdr.detectChanges();
    },
    error: () => {}
  });
}
}