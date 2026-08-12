import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { catchError, of } from 'rxjs';
import { PatientProfile } from '@core/models/patient-profile.model';
import { ProfileService } from '@core/services/profile.service';

@Component({
  selector: 'app-patient-profile',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './patient-profile.component.html',
  styleUrl: './patient-profile.component.scss',
})
export class PatientProfileComponent implements OnInit {
  private profileService = inject(ProfileService);
  private fb = inject(FormBuilder);

  // ── state ─────────────────────────────────────────────
  profile = signal<PatientProfile | null>(null);
  isLoading = signal(true);
  isSaving = signal(false);
  isUploadingImage = signal(false);
  saveSuccess = signal(false);
  saveError = signal<string | null>(null);
  imagePreview = signal<string | null>(null);

  // ── form ──────────────────────────────────────────────
  form = this.fb.group({
    firstName:       ['', Validators.required],
    lastName:        ['', Validators.required],
    phoneNumber:     [''],
    dateOfBirth:     ['', Validators.required],
    gender:          ['', Validators.required],
    bloodType:       [''],
    chronicDiseases: [''],
    allergies:       [''],
  });

  genders = ['Male', 'Female'];
  bloodTypes = ['A+', 'A−', 'B+', 'B−', 'AB+', 'AB−', 'O+', 'O−'];

  // ── lifecycle ─────────────────────────────────────────
  ngOnInit(): void {
    this.profileService.getMyProfile()
      .pipe(catchError(() => of(null)))
      .subscribe(profile => {
        this.isLoading.set(false);
        if (!profile) return;
        this.profile.set(profile);
        this.imagePreview.set(profile.image);
        this.form.patchValue({
          firstName:       profile.firstName,
          lastName:        profile.lastName,
          phoneNumber:     profile.phoneNumber ?? '',
          dateOfBirth:     profile.dateOfBirth.slice(0, 10), // "YYYY-MM-DD" for date input
          gender:          profile.gender,
          bloodType:       profile.bloodType ?? '',
          chronicDiseases: profile.chronicDiseases ?? '',
          allergies:       profile.allergies ?? '',
        });
      });
  }

  // ── save profile ──────────────────────────────────────
  save(): void {
    if (this.form.invalid || this.isSaving()) return;
    this.isSaving.set(true);
    this.saveSuccess.set(false);
    this.saveError.set(null);

    const v = this.form.getRawValue();

    this.profileService.updateMyProfile({
      firstName:       v.firstName!,
      lastName:        v.lastName!,
      phoneNumber:     v.phoneNumber || null,
      dateOfBirth:     v.dateOfBirth!,
      gender:          v.gender!,
      bloodType:       v.bloodType || null,
      chronicDiseases: v.chronicDiseases || null,
      allergies:       v.allergies || null,
    }).pipe(catchError(err => {
      this.saveError.set(err?.detail ?? 'Failed to save. Please try again.');
      this.isSaving.set(false);
      return of(null);
    })).subscribe(result => {
      if (result === null) return;
      this.isSaving.set(false);
      this.saveSuccess.set(true);
      // update displayed name
      this.profile.update(p => p ? {
        ...p,
        firstName: v.firstName!,
        lastName: v.lastName!,
      } : null);
      setTimeout(() => this.saveSuccess.set(false), 3000);
    });
  }

  // ── image upload ──────────────────────────────────────
  onImagePicked(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    // show preview immediately
    const reader = new FileReader();
    reader.onload = () => this.imagePreview.set(reader.result as string);
    reader.readAsDataURL(file);

    this.isUploadingImage.set(true);
    this.profileService.updateImage(file)
      .pipe(catchError(() => of(null)))
      .subscribe(() => this.isUploadingImage.set(false));
  }

  removeImage(): void {
    this.isUploadingImage.set(true);
    this.profileService.deleteImage()
      .pipe(catchError(() => of(null)))
      .subscribe(() => {
        this.imagePreview.set(null);
        this.isUploadingImage.set(false);
      });
  }

  // ── helpers ───────────────────────────────────────────
  initials(profile: PatientProfile): string {
    return `${profile.firstName[0]}${profile.lastName[0]}`.toUpperCase();
  }

  fullName(profile: PatientProfile): string {
    return `${profile.firstName} ${profile.lastName}`;
  }
}