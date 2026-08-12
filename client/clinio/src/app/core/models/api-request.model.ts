export type LoginRequest = {
  email: string;
  password: string;
}

export type RegisterPatientRequest = {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  gender: string;
  bloodType?: string;
  chronicDiseases?: string;
  allergies?: string;
}

export type RegisterClinicDto = {
    name: string;
    address: string;
    phone: string;
}

export type RegisterDoctorRequest = {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  specialty: string;
  licenseNumber: string;
  consultationFee: number;
  registerClinicDto?: RegisterClinicDto;
  clinicId?: number;
}

export type LogoutCommand = {
  refreshToken?: string;
  logoutAllDevices?: boolean;
}

