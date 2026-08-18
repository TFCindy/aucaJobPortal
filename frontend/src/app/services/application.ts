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

  getMyApplications(): Observable<any> {
    return this.http.get(`${this.apiUrl}/my-applications`);
  }
}