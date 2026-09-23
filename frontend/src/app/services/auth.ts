import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from "../../environments/environment";

export interface RegisterStudentPayload {
  email: string;
  fullName: string;
  password: string;
  phone: string;
  studentId: string;
  program: string;
  isAlumni: boolean;
}

export interface RegisterEmployerPayload {
  email: string;
  password: string;
  phone: string;
  companyName: string;
  companyDescription: string;
  companyWebsite?: string;
  industry: string;
  companySize?: string;
  location: string;
  tin: string;
}

export interface LoginPayload {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class Auth {
  private apiUrl = `${environment.apiUrl}/Auth`;

  constructor(private http: HttpClient) {}

  registerStudent(payload: RegisterStudentPayload): Observable<any> {
    return this.http.post(`${this.apiUrl}/register/student`, payload);
  }

  registerEmployer(payload: RegisterEmployerPayload): Observable<any> {
    return this.http.post(`${this.apiUrl}/register/employer`, payload);
  }

  verifyEmail(token: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/verify-email`, { params: { token } });
  }

  login(payload: LoginPayload): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, payload);
  }

forgotPassword(payload: { email: string }): Observable<any> {
  return this.http.post(`${this.apiUrl}/forgot-password`, payload);
}

resetPassword(payload: { token: string; newPassword: string; confirmPassword: string }): Observable<any> {
  return this.http.post(`${this.apiUrl}/reset-password`, payload);
}
}