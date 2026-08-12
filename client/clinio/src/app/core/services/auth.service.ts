import { Injectable, inject, signal, computed } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ApiService } from './api.service';
import { AuthResponse } from '../models/api-response.model';
import { LoginRequest, LogoutCommand, RegisterPatientRequest } from '@core/models/api-request.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = inject(ApiService);

  private _token = signal<string | null>(localStorage.getItem('token'));
  private _role = signal<string | null>(this.decodeRole(localStorage.getItem('token')));


  token = this._token.asReadonly();
  role = this._role.asReadonly();
  isAuthenticated = computed(() => !!this._token());

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('auth/login', request).pipe(
      tap(response => this.storeToken(response))
    );
  }

  registerPatient(request: RegisterPatientRequest): Observable<AuthResponse> {
  const form = new FormData();
  form.append('email',       request.email);
  form.append('password',    request.password);
  form.append('firstName',   request.firstName);
  form.append('lastName',    request.lastName);
  form.append('dateOfBirth', request.dateOfBirth);
  form.append('gender',      request.gender);
  if (request.bloodType)       form.append('bloodType',       request.bloodType);
  if (request.chronicDiseases) form.append('chronicDiseases', request.chronicDiseases);
  if (request.allergies)       form.append('allergies',       request.allergies);

  return this.api.post<AuthResponse>(
    `auth/register/patient`,
    form
  ).pipe(tap(response => this.storeToken(response)));
}

  registerDoctor(request: RegisterPatientRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('auth/register/doctor', request).pipe(
      tap(response => this.storeToken(response))
    );
  }

  logout(request : LogoutCommand): Observable<boolean> {
    return this.api.post<boolean>('auth/logout', request).pipe(
      tap(() => {
        this._token.set(null);
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
      })
    );
  }

  refresh(token: string, refreshToken: string): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('auth/refresh', { token, refreshToken }).pipe(
      tap(response => this.storeToken(response))
    );
  }

  private decodeRole(token: string | null): string | null {
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['role']?? null;
    } catch {
      return null;
    }
  }

  private storeToken(response: AuthResponse): void {
    this._token.set(response.token);
    this._role.set(this.decodeRole(response.token));
    localStorage.setItem('token', response.token);
    localStorage.setItem('refreshToken', response.refreshToken);
  }

  clearSession(): void {
  this._token.set(null);
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
}
}