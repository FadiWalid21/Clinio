import { Component, signal, computed, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppointmentsService } from '@core/services/appointments.service';
import { MyAppointment } from '@core/models/appoinments.model';

const APPOINTMENT_STATUS = {
  SCHEDULED: 0,
  CONFIRMED: 1,
  CANCELLED: 2,
  COMPLETED: 3,
} as const;

@Component({
  selector: 'app-my-appointments',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-appointments.component.html',
  styleUrl: './my-appointments.component.scss',
})
export class MyAppointmentsComponent implements OnInit {
  protected readonly status = APPOINTMENT_STATUS;

  private allAppointments = signal<MyAppointment[] | null>(null);
  protected appointmentService = inject(AppointmentsService);
  
  isLoading = false;
  selectedTab = signal<'upcoming' | 'past'>('upcoming');

  // Helper to normalize dates to midnight for consistent comparison
  private getNormalizedDate(dateString: string): number {
    const d = new Date(dateString);
    d.setHours(0, 0, 0, 0);
    return d.getTime();
  }

  upcomingAppointments = computed(() => {
    const appointments = this.allAppointments();
    if (!appointments) return [];
    
    const today = this.getNormalizedDate(new Date().toISOString());

    return appointments.filter(a => 
      (a.status === APPOINTMENT_STATUS.CONFIRMED || a.status === APPOINTMENT_STATUS.SCHEDULED) &&
      this.getNormalizedDate(a.date) >= today
    );
  });

  pastAppointments = computed(() => {
    const appointments = this.allAppointments();
    if (!appointments) return [];
    
    const today = this.getNormalizedDate(new Date().toISOString());

    return appointments.filter(a => 
      a.status === APPOINTMENT_STATUS.COMPLETED || 
      a.status === APPOINTMENT_STATUS.CANCELLED ||
      this.getNormalizedDate(a.date) < today
    );
  });

  currentAppointments = computed(() =>
    this.selectedTab() === 'upcoming'
      ? this.upcomingAppointments()
      : this.pastAppointments()
  );

  cancellingId = signal<number | null>(null);

canCancel(appt: MyAppointment): boolean {
  if (appt.status !== APPOINTMENT_STATUS.CONFIRMED && appt.status !== APPOINTMENT_STATUS.SCHEDULED) {
    return false;
  }
  const apptDateTime = new Date(`${appt.date}T${appt.startTime}`);
  const diffMinutes = (apptDateTime.getTime() - Date.now()) / 60000;
  return diffMinutes > 15;
}

cancelAppointment(appt: MyAppointment): void {
  if (this.cancellingId()) return; // guard against double-click

  this.cancellingId.set(appt.id);
  this.appointmentService.cancelAppointment(appt.id).subscribe({
    next: () => {
      this.allAppointments.update(list =>
        list?.map(a => a.id === appt.id ? { ...a, status: APPOINTMENT_STATUS.CANCELLED } : a) ?? list
      );
      this.cancellingId.set(null);
    },
    error: (err) => {
      console.error('Error cancelling appointment:', err);
      this.cancellingId.set(null);
    }
  });
}

  getStatusText(status: number): string {
    const map: Record<number, string> = {
      [APPOINTMENT_STATUS.SCHEDULED]: 'Scheduled',
      [APPOINTMENT_STATUS.CONFIRMED]: 'Confirmed',
      [APPOINTMENT_STATUS.CANCELLED]: 'Cancelled',
      [APPOINTMENT_STATUS.COMPLETED]: 'Completed',
    };
    return map[status] ?? 'Unknown';
  }

  getStatusClass(status: number): string {
  const map: Record<number, string> = {
    [APPOINTMENT_STATUS.SCHEDULED]: 'status-scheduled',
    [APPOINTMENT_STATUS.CONFIRMED]: 'status-confirmed',
    [APPOINTMENT_STATUS.CANCELLED]: 'status-cancelled',
    [APPOINTMENT_STATUS.COMPLETED]: 'status-completed',
  };
  return map[status] ?? 'status-scheduled';
}

  initials(name: string): string {
    if (!name) return '';
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  }

  

  ngOnInit(): void {
    this.isLoading = true;
    this.appointmentService.getAppointments().subscribe({
      next: (appointments) => {
        this.allAppointments.set(appointments);
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching appointments:', err);
        this.isLoading = false;
      }
    });
  }
}