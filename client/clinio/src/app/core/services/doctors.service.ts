import { inject, Injectable, signal } from '@angular/core';
import { ApiService } from './api.service';
import { Doctor, DoctorDetail, DoctorFilter } from '@core/models/doctor.model';
import { AvailableSlot, AvailableSlotsParams, DoctorSchedule, SlotsByDate } from '@core/models/schedule.model';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';
import { tap } from 'rxjs/internal/operators/tap';

@Injectable({
  providedIn: 'root',
})
export class DoctorsService {
  private api = inject(ApiService);

  private _doctors = signal<Doctor[] | null>(null);
  readonly doctors = this._doctors.asReadonly();

  // ── Doctors ──────────────────────────────────────────────────────────────

  getDoctors(filters: DoctorFilter = {}): Observable<Doctor[]> {
    const params: Record<string, string | number | boolean> = {};
    if (filters.searchTerm) params['searchTerm'] = filters.searchTerm;
    if (filters.specialty)  params['specialty']  = filters.specialty;
    if (filters.clinicId)   params['clinicId']   = filters.clinicId;
    // if (filters.page)       params['page']        = filters.page;
    // if (filters.pageSize)   params['pageSize']    = filters.pageSize;

    return this.api.get<Doctor[]>('doctors', params).pipe(
      tap(doctors => this._doctors.set(doctors))
    );
  }

  getDoctorById(id: number): Observable<DoctorDetail> {
    return this.api.get<DoctorDetail>(`doctors/${id}`);
  }

  getFeaturedDoctors(): Observable<Doctor[]> {
    return this.getDoctors({ pageSize: 8 });
  }

  // ── Schedules (doctor-facing, requires Doctor role) ──────────────────────

  getMySchedules(): Observable<DoctorSchedule[]> {
    return this.api.get<DoctorSchedule[]>('doctors/schedules');
  }

  // ── Available slots (patient-facing) ─────────────────────────────────────

  getAvailableSlots(
    doctorId: number,
    clinicId: number,
    params: AvailableSlotsParams = {}
  ): Observable<AvailableSlot[]> {
    const query: Record<string, string | number | boolean> = {};
    if (params.fromDate) query['fromDate'] = params.fromDate;
    if (params.toDate)   query['toDate']   = params.toDate;

    return this.api.get<AvailableSlot[]>(
      `doctors/${doctorId}/clinics/${clinicId}/available-slots`,
      query
    );
  }

  /**
   * Same as getAvailableSlots but groups results by date.
   * Used directly in the booking calendar UI.
   */
  getSlotsByDate(
    doctorId: number,
    clinicId: number,
    params: AvailableSlotsParams = {}
  ): Observable<SlotsByDate[]> {
    return this.getAvailableSlots(doctorId, clinicId, params).pipe(
      map(slots => this.groupSlotsByDate(slots))
    );
  }

  // ── Helpers ───────────────────────────────────────────────────────────────

  private groupSlotsByDate(slots: AvailableSlot[]): SlotsByDate[] {
    const today    = this.toDateString(new Date());
    const tomorrow = this.toDateString(new Date(Date.now() + 86_400_000));

    const dateMap = new Map<string, AvailableSlot[]>();
    for (const slot of slots) {
      const existing = dateMap.get(slot.date) ?? [];
      existing.push(slot);
      dateMap.set(slot.date, existing);
    }

    return Array.from(dateMap.entries()).map(([date, daySlots]) => ({
      date,
      label: date === today
        ? 'Today'
        : date === tomorrow
          ? 'Tomorrow'
          : this.formatDateLabel(date),
      slots: daySlots.sort((a, b) => a.startTime.localeCompare(b.startTime)),
    }));
  }

  private toDateString(d: Date): string {
    return d.toISOString().slice(0, 10); // "YYYY-MM-DD"
  }

  private formatDateLabel(dateStr: string): string {
    const date = new Date(dateStr + 'T00:00:00');
    return date.toLocaleDateString('en-GB', {
      weekday: 'short',
      day: 'numeric',
      month: 'short',
    }); // e.g. "Mon 12 Jan"
  }

  formatTime(timeStr: string): string {
    // "09:00:00" → "9:00 AM"
    const [h, m] = timeStr.split(':').map(Number);
    const period = h >= 12 ? 'PM' : 'AM';
    const hour   = h % 12 || 12;
    return `${hour}:${m.toString().padStart(2, '0')} ${period}`;
  }
}
