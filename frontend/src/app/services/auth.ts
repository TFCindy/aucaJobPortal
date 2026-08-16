import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from "../../environments/environment";

export interface RegisterStudentPayload{
  email:string;
  fullName:string;
  password:string;
  phone:string;
  studentId:string;
  program:string;
  isAlumni:boolean;
}

@Injectable({
  providedIn:'root'
})
export class Auth{
  private apiUrl = `${environment.apiUrl}/Auth`;

  constructor(private http: HttpClient){}

  registerStudent(payload: RegisterStudentPayload): Observable<any>{
    return this.http.post(`${this.apiUrl}/register/student`,payload);
  }
}