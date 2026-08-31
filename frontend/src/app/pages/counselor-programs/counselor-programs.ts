import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Navbar } from '../../components/navbar/navbar';
import { CounselorProgramService } from '../../services/counselor-program';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-counselor-programs',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, Navbar],
  templateUrl: './counselor-programs.html'
})
export class CounselorPrograms implements OnInit {
  programs: any[] = [];
  filteredPrograms: any[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  searchTerm = '';
  statusFilter = '';
  showDeleteModal = false;
  selectedProgramId: number | null = null;
  isDeleting = false;

  statuses = ['draft', 'published', 'upcoming', 'ongoing', 'full', 'completed', 'cancelled'];

  constructor(
    private programService: CounselorProgramService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadPrograms();
  }

  loadPrograms() {
    this.isLoading = true;
    this.errorMessage = '';

    this.programService.getMyPrograms().subscribe({
      next: (response) => {
        this.programs = response.data || [];
        this.filteredPrograms = [...this.programs];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to load programs.';
        this.cdr.detectChanges();
      }
    });
  }

  applyFilters() {
    this.filteredPrograms = this.programs.filter(program => {
      const matchesSearch = !this.searchTerm ||
        program.title?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        program.type?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesStatus = !this.statusFilter || program.status === this.statusFilter;
      
      return matchesSearch && matchesStatus;
    });
    this.cdr.detectChanges();
  }

  resetFilters() {
    this.searchTerm = '';
    this.statusFilter = '';
    this.filteredPrograms = [...this.programs];
    this.cdr.detectChanges();
  }

  openDeleteModal(programId: number) {
    this.selectedProgramId = programId;
    this.showDeleteModal = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
    this.selectedProgramId = null;
    this.cdr.detectChanges();
  }

  confirmDelete() {
    if (!this.selectedProgramId) return;

    this.isDeleting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.programService.deleteProgram(this.selectedProgramId).subscribe({
      next: () => {
        this.isDeleting = false;
        this.successMessage = 'Program deleted successfully.';
        this.showDeleteModal = false;
        this.loadPrograms();
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isDeleting = false;
        this.errorMessage = err.error?.message || 'Failed to delete program.';
        this.cdr.detectChanges();
      }
    });
  }

  publishProgram(programId: number) {
    this.programService.publishProgram(programId).subscribe({
      next: () => {
        this.successMessage = 'Program published successfully.';
        this.loadPrograms();
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Failed to publish program.';
        this.cdr.detectChanges();
      }
    });
  }

  getStatusBadge(status: string): string {
    const badgeMap: Record<string, string> = {
      'draft': 'bg-gray-100 text-gray-700',
      'published': 'bg-green-100 text-green-700',
      'upcoming': 'bg-blue-100 text-blue-700',
      'ongoing': 'bg-purple-100 text-purple-700',
      'full': 'bg-yellow-100 text-yellow-700',
      'completed': 'bg-gray-100 text-gray-700',
      'cancelled': 'bg-red-100 text-red-700'
    };
    return badgeMap[status?.toLowerCase()] || 'bg-gray-100 text-gray-700';
  }

  getImageUrl(imagePath: string): string {
  if (!imagePath) return '';
  const baseUrl = environment.apiUrl.replace('/api', '');
  return `${baseUrl}${imagePath}`;
}
  getTypeLabel(type: string): string {
    const typeMap: Record<string, string> = {
      'training': 'Training',
      'workshop': 'Workshop',
      'seminar': 'Seminar',
      'webinar': 'Webinar',
      'bootcamp': 'Bootcamp'
    };
    return typeMap[type?.toLowerCase()] || type || 'Program';
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }
}