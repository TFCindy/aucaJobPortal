import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/Users`;

  constructor(private http: HttpClient) {}

  getUsers(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  deactivateUser(userId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${userId}/deactivate`, {});
  }

  activateUser(userId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${userId}/activate`, {});
  }
}