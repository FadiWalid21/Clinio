import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { catchError, forkJoin, of } from 'rxjs';
import { DoctorsService } from '@core/services/doctors.service';
import { AppointmentsService } from '@core/services/appointments.service';
import { DoctorDetail } from '@core/models/doctor.model';
import { AvailableSlot, SlotsByDate } from '@core/models/schedule.model';
import { AuthService } from '@core/services/auth.service';

@Component({
  selector: 'app-doctor-details',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './doctor-details.component.html',
  styleUrl: './doctor-details.component.scss',
})
export class DoctorDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private auth = inject(AuthService);
  private router = inject(Router);
  private doctorsService = inject(DoctorsService);
  private appointmentsService = inject(AppointmentsService);

  // ── state ────────────────────────────────────────────
  doctor = signal<DoctorDetail | null>(null);
  slotsByDate = signal<SlotsByDate[]>([]);
  selectedSlot = signal<AvailableSlot | null>(null);
  selectedDate = signal<string | null>(null);

  isLoadingDoctor = signal(true);
  isLoadingSlots = signal(true);
  isBooking = signal(false);
  bookingSuccess = signal(false);
  bookingError = signal<string | null>(null);

  // ── derived ──────────────────────────────────────────
  slotsForSelectedDate = computed(() => {
    const date = this.selectedDate();
    if (!date) return [];
    return this.slotsByDate().find(d => d.date === date)?.slots ?? [];
  });

  availableDays = computed(() =>
    this.slotsByDate().filter(d => d.slots.length > 0)
  );

  selectedDateLabel = computed(() => {
    const date = this.selectedDate();
    if (!date) return '';
    return this.slotsByDate().find(d => d.date === date)?.label ?? '';
  });

  // ── lifecycle ────────────────────────────────────────
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) { this.router.navigate(['/doctors']); return; }

    this.doctorsService.getDoctorById(id)
      .pipe(catchError(() => of(null)))
      .subscribe(doctor => {
        this.isLoadingDoctor.set(false);
        if (!doctor) { this.router.navigate(['/doctors']); return; }
        this.doctor.set(doctor);
        this.loadSlots(doctor.id, doctor.clinicId);
      });
  }

  loadSlots(doctorId: number, clinicId: number): void {
    this.isLoadingSlots.set(true);
    this.doctorsService
      .getSlotsByDate(doctorId, clinicId)
      .pipe(catchError(() => of([])))
      .subscribe(days => {
        this.slotsByDate.set(days);
        // auto-select first available date
        if (days.length > 0) {
          this.selectedDate.set(days[0].date);
        }
        this.isLoadingSlots.set(false);
      });
  }

  // ── interactions ─────────────────────────────────────
  selectDate(date: string): void {
    this.selectedDate.set(date);
    this.selectedSlot.set(null); // reset slot when date changes
    this.bookingError.set(null);
  }

  selectSlot(slot: AvailableSlot): void {
    this.selectedSlot.set(
      this.selectedSlot()?.id === slot.id ? null : slot
    );
    this.bookingError.set(null);
  }

  confirmBooking(): void {
    const slot = this.selectedSlot();
    if (!slot || this.isBooking()) return;

    this.isBooking.set(true);
    this.bookingError.set(null);

    if(this.auth.isAuthenticated()) {
    this.appointmentsService
      .bookAppointment({ timeSlotId: slot.id , doctorId: this.doctor()?.id ?? 0 , clinicId: this.doctor()?.clinicId })
      .pipe(catchError(err => {
        this.bookingError.set(err?.detail ?? 'Booking failed. Please try again.');
        this.isBooking.set(false);
        return of(null);
      }))
      .subscribe(result => {
        if (!result) return;
        this.isBooking.set(false);
        this.bookingSuccess.set(true);
      });
    }else this.router.navigate(['/auth/login'], { queryParams: { returnUrl: this.router.url } });
  }

  // ── helpers ──────────────────────────────────────────
  formatTime(time: string): string {
    return this.doctorsService.formatTime(time);
  }

  initials(name: string): string {
    return name.split(' ').slice(0, 2).map(n => n[0]).join('').toUpperCase();
  }

  dayLabel(date: string): string {
    return new Date(date + 'T00:00:00')
      .toLocaleDateString('en-GB', { weekday: 'short' });
  }

  dayNumber(date: string): string {
    return new Date(date + 'T00:00:00')
      .toLocaleDateString('en-GB', { day: 'numeric', month: 'short' });
  }

  
}