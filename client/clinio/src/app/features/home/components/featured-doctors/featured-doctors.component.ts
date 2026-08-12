import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DoctorsService } from '@core/services/doctors.service';
import { Doctor } from '@core/models/doctor.model';
import { catchError, of } from 'rxjs';
import { LanguageService } from '@core/services/language.service';

@Component({
  selector: 'app-featured-doctors',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './featured-doctors.component.html',
  styleUrl: './featured-doctors.component.scss',
})
export class FeaturedDoctorsComponent implements OnInit {
  private doctorsService = inject(DoctorsService);
  protected ls = inject(LanguageService);

  doctors = signal<Doctor[]>([]);
  isLoading = signal(true);

  ngOnInit(): void {
    this.doctorsService
      .getFeaturedDoctors()
      .pipe(catchError(() => of([])))
      .subscribe(doctors => {
        this.doctors.set(doctors);
        this.isLoading.set(false);
      });
  }

  initials(name: string): string {
    return name
      .split(' ')
      .slice(0, 2)
      .map(n => n[0])
      .join('')
      .toUpperCase();
  }
}
