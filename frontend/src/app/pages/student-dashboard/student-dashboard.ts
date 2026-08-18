
import { RouterLink } from '@angular/router';import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [CommonModule, Navbar, RouterLink],
  templateUrl: './student-dashboard.html'
})
export class StudentDashboard implements OnInit {
  userFullName = '';
  profileCompletionPercent = 0;

  ngOnInit() {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.userFullName = user.fullName;
    }
    // profileCompletionPercent will be calculated once we build the profile fetch — placeholder for now
    this.profileCompletionPercent = 40;
  }
}