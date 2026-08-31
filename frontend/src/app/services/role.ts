import{Injectable} from '@angular/core';
import{HttpClient} from '@angular/common/http';
import{Observable} from 'rxjs';
import{environment} from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class RoleService {
    private apiUrl = `${environment.apiUrl}/Roles`;

    constructor(private http: HttpClient) {}

    getRoles(): Observable<any> {
        return this.http.get(this.apiUrl);
    }

    getRoleById(roleId: number) : Observable<any> {
        return this.http.get(`${this.apiUrl}/${roleId}`);
    }

    getPermissions(): Observable<any> {
        return this.http.get(`${this.apiUrl}/permissions`);
    }

    updateRolePermissions(roleId: number, permissions: string[]): Observable<any> {
        return this.http.put(`${this.apiUrl}/${roleId}/permissions`,{permissions});
    }
}