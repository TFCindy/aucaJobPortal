import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class JobService {
  private apiUrl = `${environment.apiUrl}/Jobs`;

  constructor(private http: HttpClient) {}

  // PUBLIC (Students)
  getActiveJobs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/public`);
  }

  getJobById(jobId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${jobId}`);
  }


  saveJob(jobId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${jobId}/save`, {});
  }

  unsaveJob(jobId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${jobId}/save`);
  }

  getSavedJobs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/saved`);
}

  applyToJob(jobId: number, payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/${jobId}/apply`, payload);
  } 

  checkApplicationStatus(jobId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${jobId}/application-status`);
  }

  // EMPLOYER 
  getMyJobs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/my-jobs`);
  }

  createJob(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, payload);
  }

  updateJob(jobId: number, payload: any): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${jobId}`, payload);
  }

  deleteJob(jobId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${jobId}`);
  }

  // GET APPLICATIONS FOR A SPECIFIC JOB
  getJobApplications(jobId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${jobId}/applications`);
  }
}