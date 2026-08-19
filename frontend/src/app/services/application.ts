import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {
  private apiUrl = `${environment.apiUrl}/Applications`;

  constructor(private http: HttpClient) {}

  //STUDENT
  getMyApplications(): Observable<any> {
    return this.http.get(`${this.apiUrl}/my-applications`);
  }

  applyToJob(jobId: number, payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/apply/${jobId}`, payload);
  }

  //EMPLOYER
  getEmployerApplications(): Observable<any> {
    return this.http.get(`${this.apiUrl}/employer-applications`);
  }

  updateApplicationStatus(applicationId: number, status: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${applicationId}/status`, { status });
  }

  getApplicationById(applicationId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${applicationId}`);
  }
}