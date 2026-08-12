import { inject, Injectable, signal } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { tap } from 'rxjs';
import { PatientProfile, UpdateMyProfileCommand } from '@core/models/patient-profile.model';
import { environment } from '@env/environment';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private api = inject(ApiService);
  private http = inject(HttpClient);

  // ── shared signal — navbar reads this ──────────────
  private _profile = signal<PatientProfile | null>(null);
  readonly profile = this._profile.asReadonly();

  getMyProfile(): Observable<PatientProfile> {
    return this.api.get<PatientProfile>('patients/me').pipe(
      tap(p => this._profile.set(p))
    );
  }

  updateMyProfile(command: UpdateMyProfileCommand): Observable<boolean> {
    return this.api.put<boolean>('patients/me', command).pipe(
      tap(() => {
        // optimistically update name fields in the shared signal
        this._profile.update(p => p ? {
          ...p,
          firstName: command.firstName,
          lastName:  command.lastName,
          phoneNumber: command.phoneNumber,
        } : null);
      })
    );
  }

  updateImage(file: File): Observable<unknown> {
    const form = new FormData();
    form.append('file', file);
    return this.http.put(`${environment.apiUrl}/auth/me/image`, form).pipe(
      tap((res: any) => {
        if (res?.imageUrl) {
          this._profile.update(p => p ? { ...p, image: res.imageUrl } : null);
        }
      })
    );
  }

  deleteImage(): Observable<unknown> {
    return this.http.delete(`${environment.apiUrl}/auth/me/image`).pipe(
      tap(() => this._profile.update(p => p ? { ...p, image: null } : null))
    );
  }

  clearProfile(): void {
    this._profile.set(null);
  }
}
