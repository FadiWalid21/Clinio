// A single bookable time slot, as returned by GET /doctors/:id/clinics/:id/available-slots
export interface AvailableSlot {
  id: number;
  date: string;       // "YYYY-MM-DD"
  startTime: string;  // "09:00:00"
  endTime: string;    // "09:30:00"
  isBooked?: boolean;
}

// Query params for GET .../available-slots
export interface AvailableSlotsParams {
  fromDate?: string;  // "YYYY-MM-DD"
  toDate?: string;    // "YYYY-MM-DD"
}

// Slots grouped by day, used by the booking calendar UI
export interface SlotsByDate {
  date: string;   // "YYYY-MM-DD"
  label: string;  // "Today" | "Tomorrow" | "Mon 12 Jan"
  slots: AvailableSlot[];
}

// A doctor's recurring weekly schedule, as returned by GET /doctors/schedules (doctor-facing)
export interface DoctorSchedule {
  id: number;
  clinicId: number;
  dayOfWeek: number;      // 0 (Sunday) – 6 (Saturday)
  startTime: string;      // "09:00:00"
  endTime: string;        // "17:00:00"
  slotDurationMinutes: number;
  isActive: boolean;
}
