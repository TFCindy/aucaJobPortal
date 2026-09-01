import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CounselorProgramService {
  private apiUrl = `${environment.apiUrl}/CareerPrograms`;

  constructor(private http: HttpClient) {}

  // ─── GET MY PROGRAMS ───
  getMyPrograms(): Observable<any> {
    return this.http.get(`${this.apiUrl}/my-programs`);
  }

  getProgramApplications(programId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${programId}/applications`);
  }

  updateApplicationStatus(applicationId: number, status: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/applications/${applicationId}/status`, { status });
  }

  createProgram(formData: FormData): Observable<any> {
    return this.http.post(this.apiUrl, formData);
  }

  updateProgram(programId: number, formData: FormData): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${programId}`, formData);
  }

  publishProgram(programId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${programId}/publish`, {});
  }

  deleteProgram(programId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${programId}/delete`, {});
  }

  getProgramById(programId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${programId}`);
  }

  getAvailablePrograms(): Observable<any> { 
    return this.http.get(`${this.apiUrl}/available`);
  }

  applyToProgram(programId: number, applicationData: any): Observable<any> {
  return this.http.post(`${this.apiUrl}/${programId}/apply`, applicationData);
}

getMyProgramApplications(): Observable<any> {
  return this.http.get(`${this.apiUrl}/my-applications`);
}

getProgramApplicationById(applicationId: number): Observable<any> {
  return this.http.get(`${this.apiUrl}/applications/${applicationId}`);
}
}