export type BookAppointmentRequest = {
    timeSlotId: number;
    doctorId: number;
    clinicId?: number;
    note?: string;
}

export type BookAppointmentResponse = {
  id: number;
  timeSlotId: number;
  status: string;
}

export type MyAppointment = {
  id: number;
  date: string;          // ISO date string (e.g., "2026-07-02")
  startTime: string;     // Time string (e.g., "09:30:00")
  endTime: string;
  doctorName: string;
  clinicName: string;
  clinicAddress: string;
  consultationFee: number; // decimal as number
  status: number;        // AppointmentStatus enum (integer)
  notes: string | null;
  cancellationReason: string | null;
  createdAt: string;     // ISO datetime string
};
