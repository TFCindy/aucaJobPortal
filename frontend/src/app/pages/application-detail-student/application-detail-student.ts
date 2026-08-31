import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ApplicationService } from '../../services/application';

@Component({
  selector: 'app-application-detail-student',
  standalone: true,
  imports: [CommonModule, RouterLink, Navbar],
  templateUrl: './application-detail-student.html'
})
export class ApplicationDetailStudent implements OnInit {
  application: any = null;
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  showWithdrawModal = false;
  isWithdrawing = false;

  constructor(
    private applicationService: ApplicationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadApplication(+id);
    }
  }

  loadApplication(id: number) {
    this.isLoading = true;
    this.errorMessage = '';

    this.applicationService.getApplicationById(id).subscribe({
      next: (response) => {
        this.application = response.data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load application.';
        this.cdr.detectChanges();
      }
    });
  }

  canWithdraw(): boolean {
    return this.application?.status === 'pending';
  }

  openWithdrawModal() {
    this.showWithdrawModal = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  closeWithdrawModal() {
    this.showWithdrawModal = false;
    this.cdr.detectChanges();
  }

  withdrawApplication() {
  if (!this.application) return;

  this.isWithdrawing = true;
  this.errorMessage = '';
  this.successMessage = '';

  this.applicationService.withdrawApplication(this.application.id).subscribe({
    next: () => {
      this.isWithdrawing = false;
      this.successMessage = 'Application withdrawn successfully.';
      this.application.status = 'withdrawn';
      this.showWithdrawModal = false;
      
      // ─── STAY ON PAGE, REFRESH DATA ───
      this.loadApplication(this.application.id);
      
      this.cdr.detectChanges();
    },
    error: (err: any) => {
      this.isWithdrawing = false;
      this.errorMessage = err.error?.message || 'Failed to withdraw application.';
      this.cdr.detectChanges();
    }
  });
}

  getStatusBadge(status: string): string {
    const badgeMap: Record<string, string> = {
      'pending': 'bg-yellow-100 text-yellow-700',
      'reviewed': 'bg-blue-100 text-blue-700',
      'shortlisted': 'bg-purple-100 text-purple-700',
      'hired': 'bg-green-100 text-green-700',
      'rejected': 'bg-red-100 text-red-700',
      'withdrawn': 'bg-gray-100 text-gray-700'
    };
    return badgeMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  goBack() {
    this.router.navigate(['/student/applications']);
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }
}