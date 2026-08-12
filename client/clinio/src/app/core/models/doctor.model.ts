// What GET /doctors returns (list item)
export interface Doctor {
  id: number;           // int on backend
  fullName: string;
  specialty: string;
  clinicName: string;
  clinicId: number;
  profileImageUrl: string | null;

  // Add these to your GetAllDoctorsDto as backend grows:
  city?: string;
  gender?: 'male' | 'female';
  rating?: number;
  reviewCount?: number;
  yearsOfExperience?: number;
  availableToday?: boolean;
  upcomingSlots?: string[];      // 3 next slot strings
  consultationFee?: number;
}

// What GET /doctors/:id returns (full profile)
export interface DoctorDetail extends Doctor {
  area?: string;
  bio?: string;
  nextAvailableSlot?: string;
}

// Query params for GET /doctors
export interface DoctorFilter {
  searchTerm?: string;
  specialty?: string;
  clinicId?: number;
  page?: number;
  pageSize?: number;
  // future: gender, city, availableToday — add when backend supports
  city?: string;
  gender?: 'male' | 'female' | '';
  availableToday?: boolean;
}