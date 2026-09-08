import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoleService } from '../../services/role';

@Component({
  selector: 'app-admin-roles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-roles.html'
})
export class AdminRoles implements OnInit {
  roles: any[] = [];
  allPermissions: string[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  selectedRole: any = null;
  isEditing = false;

  constructor(
    private roleService: RoleService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
  this.isLoading = true;
  this.errorMessage = '';
  this.successMessage = '';

  this.roleService.getRoles().subscribe({
    next: (response) => {
      this.roles = response.data || [];
      this.isLoading = false;
      this.cdr.detectChanges();
    },
    error: (err: any) => {
      this.isLoading = false;
      this.errorMessage = err.error?.message || 'Failed to load roles.';
      this.cdr.detectChanges();
    }
  });

  this.roleService.getPermissions().subscribe({
    next: (response) => {
      // ─── FIX: Map objects to strings if needed ───
      if (response.data && response.data.length > 0 && typeof response.data[0] === 'object') {
        this.allPermissions = response.data.map((p: any) => p.name);
      } else {
        this.allPermissions = response.data || [];
      }
      this.cdr.detectChanges();
    },
    error: () => {}
  });
}

  editRole(role: any) {
    this.selectedRole = JSON.parse(JSON.stringify(role));
    this.isEditing = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  cancelEdit() {
    this.isEditing = false;
    this.selectedRole = null;
    this.errorMessage = '';
    this.successMessage = '';
    this.cdr.detectChanges();
  }

  isPermissionSelected(permission: any): boolean {
  const permissionName = typeof permission === 'string' ? permission : permission.name;
  return this.selectedRole?.permissions?.includes(permissionName) || false;
}

togglePermission(permission: any) {
  if (!this.selectedRole) return;

  const permissionName = typeof permission === 'string' ? permission : permission.name;
  const index = this.selectedRole.permissions.indexOf(permissionName);
  if (index > -1) {
    this.selectedRole.permissions.splice(index, 1);
  } else {
    this.selectedRole.permissions.push(permissionName);
  }
  this.cdr.detectChanges();
}

  savePermissions() {
    if (!this.selectedRole) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.roleService.updateRolePermissions(this.selectedRole.id, this.selectedRole.permissions).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = `Permissions updated for ${this.selectedRole.name}.`;
        
        // Update the role in the list
        const index = this.roles.findIndex(r => r.id === this.selectedRole.id);
        if (index > -1) {
          this.roles[index].permissions = [...this.selectedRole.permissions];
        }
        
        this.isEditing = false;
        this.selectedRole = null;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Failed to update permissions.';
        this.cdr.detectChanges();
      }
    });
  }

   getPermissionBadge(permission: string): string {
    const badgeMap: Record<string, string> = {
      'manage_users': 'bg-gray-100 text-gray-700',
      'manage_roles': 'bg-gray-100 text-gray-700',
      'manage_counselors': 'bg-gray-100 text-gray-700',
      'view_counselors': 'bg-gray-100 text-gray-700',
      'view_audit_logs': 'bg-gray-100 text-gray-700',
      'approve_jobs': 'bg-gray-100 text-gray-700',
      'apply_jobs': 'bg-gray-100 text-gray-700',
      'save_jobs': 'bg-gray-100 text-gray-700',
      'post_jobs': 'bg-gray-100 text-gray-700',
      'view_applications': 'bg-gray-100 text-gray-700',
      'send_messages': 'bg-gray-100 text-gray-700',
      'manage-programs':'bg-gray-100 text-gray-700',
      'view_reports': 'bg-gray-100 text-gray-700'
    };
    return badgeMap[permission] || 'bg-gray-100 text-gray-700';
  }

  clearMessages() {
    this.errorMessage = '';
    this.successMessage = '';
  }
}