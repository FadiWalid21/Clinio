
export interface PatientProfile {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  image: string | null;
  age: number;
  dateOfBirth: string;   // ISO string "YYYY-MM-DD"
  gender: string;
  bloodType: string | null;
  chronicDiseases: string | null;
  allergies: string | null;
}

export interface UpdateMyProfileCommand {
  firstName: string;
  lastName: string;
  phoneNumber: string | null;
  dateOfBirth: string;   // ISO string
  gender: string;
  bloodType: string | null;
  chronicDiseases: string | null;
  allergies: string | null;
}