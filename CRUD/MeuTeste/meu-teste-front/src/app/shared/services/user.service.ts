import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface User {
  id: number;
  username: string;
  email: string;
  role: string;
  isApproved: boolean;
  createdAt: Date;
}

export interface ApproveUserRequest {
  userId: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) { }

  /**
   * Obter lista de usuários pendentes de aprovação
   * Requer autenticação e role Admin
   */
  getInactiveUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/inactive`);
  }

  /**
   * Aprovar um usuário
   * Requer autenticação e role Admin
   */
  approveUser(userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${userId}/approve`, {});
  }

  /**
   * Obter informações do usuário autenticado
   * Requer autenticação
   */
  getCurrentUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/me`);
  }
}
