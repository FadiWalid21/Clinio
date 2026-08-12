import { inject, Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs/internal/Observable';
import { BookAppointmentRequest, BookAppointmentResponse, MyAppointment } from '@core/models/appoinments.model';

@Injectable({ providedIn: 'root' })
export class AppointmentsService {
  private api = inject(ApiService);

  bookAppointment(request: BookAppointmentRequest): Observable<BookAppointmentResponse> {
    return this.api.post<BookAppointmentResponse>('appointments', request);
  }

  getAppointments(): Observable<MyAppointment[]> {
    return this.api.get<MyAppointment[]>('appointments/my');
  }

  cancelAppointment(appointmentId: number, reason: string = ''): Observable<void> {
  return this.api.put<void>(`appointments/${appointmentId}/cancel`, reason || null);
}
}