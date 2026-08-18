import { Service } from '@angular/core';

@Service()
export class Job {}
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

  getActiveJobs(): Observable<any> {
    return this.http.get(this.apiUrl);
  }
}