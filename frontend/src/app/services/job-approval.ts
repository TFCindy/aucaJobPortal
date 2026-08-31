import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class JobApprovalService {
  private apiUrl = `${environment.apiUrl}/Jobs`;

  constructor(private http: HttpClient) {}

  getPendingJobs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/pending-approval`);
  }

  approveJob(jobId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${jobId}/approve`, {});
  }

  rejectJob(jobId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${jobId}/reject`, {});
  }
}