import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CounselorService {
  private apiUrl = `${environment.apiUrl}/Counselors`;

  constructor(private http: HttpClient) {}

  getCounselors(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  createCounselor(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  deactivateCounselor(counselorId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${counselorId}/deactivate`, {});
  }

  activateCounselor(counselorId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${counselorId}/activate`, {});
  }
}