import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DoctorsService } from '@core/services/doctors.service';
import { Doctor, DoctorFilter } from '@core/models/doctor.model';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.scss',
})
export class DoctorsComponent implements OnInit {
  private doctorsService = inject(DoctorsService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  doctors = signal<Doctor[]>([]);
  isLoading = signal(true);
  searchFocused = false; // plain boolean — no signal needed, just drives a CSS class

  filters = signal<DoctorFilter>({
    searchTerm: '',
    specialty: '',
    city: '',
    gender: '',
    availableToday: false,
  });

  specialties = [
    'Cardiology', 'Dental', 'Dermatology', 'ENT',
    'General Practice', 'Gynecology', 'Neurology',
    'Ophthalmology', 'Orthopedic', 'Pediatrics',
    'Psychiatry', 'Urology',
  ];

  cities = [
    'Cairo', 'Alexandria', 'Giza', 'Dakahlia',
    'Mansoura', 'Tanta', 'Assiut', 'Luxor',
  ];

  resultCount = computed(() => this.doctors().length);

  activeFilterCount = computed(() => {
    const f = this.filters();
    return [f.specialty, f.city, f.gender, f.availableToday].filter(Boolean).length;
  });

  // Signature element: a horizontal "available now" spotlight rail, image-forward.
  // Only surfaced when there's something worth spotlighting, and only while the
  // person hasn't already narrowed things down with a filter of their own.
  spotlightDoctors = computed(() => {
    const f = this.filters();
    const hasNarrowFilter = !!(f.specialty || f.city || f.gender || f.searchTerm);
    if (hasNarrowFilter) return [];
    return this.doctors().filter(d => d.availableToday && d.profileImageUrl).slice(0, 6);
  });

  gridDoctors = computed(() => {
    const spotlightIds = new Set(this.spotlightDoctors().map(d => d.id));
    return this.doctors().filter(d => !spotlightIds.has(d.id));
  });

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['search'])    this.filters.update(f => ({ ...f, searchTerm: params['search'] }));
      if (params['specialty']) this.filters.update(f => ({ ...f, specialty: params['specialty'] }));
      this.loadDoctors();
    });
  }

  loadDoctors(): void {
    this.isLoading.set(true);
    const f = this.filters();

    this.doctorsService
      .getDoctors({ searchTerm: f.searchTerm || undefined })
      .pipe(catchError(() => of([])))
      .subscribe(doctors => {
        let result = doctors;
        if (f.specialty)      result = result.filter(d => d.specialty === f.specialty);
        if (f.city)           result = result.filter(d => d.city === f.city);
        if (f.gender)         result = result.filter(d => d.gender === f.gender);
        if (f.availableToday) result = result.filter(d => d.availableToday);
        this.doctors.set(result);
        this.isLoading.set(false);
      });
  }

  setSpecialty(value: string): void {
    this.filters.update(f => ({ ...f, specialty: f.specialty === value ? '' : value }));
    this.loadDoctors();
  }

  setCity(value: string): void {
    this.filters.update(f => ({ ...f, city: f.city === value ? '' : value }));
    this.loadDoctors();
  }

  setGender(value: '' | 'male' | 'female'): void {
    this.filters.update(f => ({ ...f, gender: f.gender === value ? '' : value }));
    this.loadDoctors();
  }

  toggleAvailableToday(): void {
    this.filters.update(f => ({ ...f, availableToday: !f.availableToday }));
    this.loadDoctors();
  }

  clearFilters(): void {
    this.filters.set({ searchTerm: '', specialty: '', city: '', gender: '', availableToday: false });
    this.loadDoctors();
  }

  onSearchInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.filters.update(f => ({ ...f, searchTerm: value }));
    this.loadDoctors();
  }

  onSearchEnter(): void {
    this.loadDoctors();
  }

  clearSearch(): void {
    this.filters.update(f => ({ ...f, searchTerm: '' }));
    this.loadDoctors();
  }

  initials(name: string): string {
    return name.split(' ').slice(0, 2).map(n => n[0]).join('').toUpperCase();
  }
}