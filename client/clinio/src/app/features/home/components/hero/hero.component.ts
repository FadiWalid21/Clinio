import { Component, inject, signal, DestroyRef, ElementRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged, switchMap, catchError, of } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DoctorsService } from '@core/services/doctors.service';
import { Doctor } from '@core/models/doctor.model';
import { LanguageService } from '@core/services/language.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-hero',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './hero.component.html',
  styleUrl: './hero.component.scss'
})
export class HeroComponent {
  private doctorService = inject(DoctorsService);
  private destroyRef = inject(DestroyRef);
  private router = inject(Router);
  protected ls = inject(LanguageService);

  searchTerm = signal('');
  results = signal<Doctor[]>([]);
  isLoading = signal(false);
  isOpen = signal(false);

  private search$ = new Subject<string>();

  constructor(private elRef: ElementRef) {
    this.search$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(term => {
        if (term.trim().length < 2) {
          this.results.set([]);
          this.isOpen.set(false);
          this.isLoading.set(false);
          return of([]);
        }
        this.isLoading.set(true);
        return this.doctorService.getDoctors({ searchTerm: term }).pipe(
          catchError(() => of([]))
        );
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(doctors => {
      this.isLoading.set(false);
      this.results.set(doctors);
      this.isOpen.set(doctors.length > 0);
    });
  }

  onInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchTerm.set(value);
    this.search$.next(value);
  }

  onFocus(): void {
    if (this.results().length > 0) this.isOpen.set(true);
  }

  onBlur(): void {
    setTimeout(() => this.isOpen.set(false), 150);
  }

  onSearchSubmit(): void {
    const term = this.searchTerm().trim();
    if (!term) return;
    this.isOpen.set(false);
    this.router.navigate(['/doctors'], { queryParams: { search: term } });
  }

  selectDoctor(doctor: Doctor): void {
    this.isOpen.set(false);
    this.router.navigate(['/doctors', doctor.id]);
  }

  highlight(text: string): string {
    const q = this.searchTerm().trim();
    if (!q) return text;
    const escaped = q.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    const regex = new RegExp(`(${escaped})`, 'gi');
    return text.replace(regex, '<mark class="highlight">$1</mark>');
  }

  initials(name: string): string {
    return name.split(' ').slice(0, 2).map(n => n[0]).join('').toUpperCase();
  }

  scrollToNext() {
  this.elRef.nativeElement.nextElementSibling?.scrollIntoView({ behavior: 'smooth' });
}
}