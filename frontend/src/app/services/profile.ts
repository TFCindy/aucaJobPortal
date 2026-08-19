import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private apiUrl = `${environment.apiUrl}/Profile`;

  constructor(private http: HttpClient) {}

  // STUDENT 
  getStudentProfile(): Observable<any> {
    return this.http.get(`${this.apiUrl}/student`);
  }

  updateStudentProfile(payload: any): Observable<any> {
    return this.http.patch(`${this.apiUrl}/student`, payload);
  }

  // EMPLOYER
  getEmployerProfile(): Observable<any> {
    return this.http.get(`${this.apiUrl}/employer`);
  }

  updateEmployerProfile(payload: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/employer`, payload);
  }
}