import { Routes } from '@angular/router';
import { authGuard } from '@core/guards/auth-guard';

export const routes: Routes = [

  // ── Public ──────────────────────────────────────────────
  {
    path: '',
    loadComponent: () =>
      import('./features/home/home.component').then(m => m.HomeComponent),
  },
  {
    path: 'doctors',
    loadComponent: () =>
      import('./features/doctors-page/doctors.component').then(m => m.DoctorsComponent),
  },
  {
    path: 'doctors/:id',
    loadComponent: () =>
      import('./features/doctors-page/components/doctor-details/doctor-details.component').then(m => m.DoctorDetailsComponent),
  },
  {
    path: 'about-us',
    loadComponent: () =>
      import('./features/about-us/about-us.component').then(m => m.AboutUsComponent),
  },


  // ── Auth ────────────────────────────────────────────────
  {
    path: 'auth',
    loadComponent: () =>
      import('./features/auth/auth/auth.component').then(m => m.AuthComponent),
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      {
        path: 'login',
        loadComponent: () =>
          import('./features/auth/login/login.component').then(m => m.LoginComponent),
      },
      {
        path: 'register',
        loadComponent: () =>
          import('./features/auth/register/register.component').then(m => m.RegisterComponent),
      },
    ],
  },

  // ── Patient (protected) ─────────────────────────────────
  {
    path: 'patient',
    canActivate: [authGuard],
    children: [
      {
        path: 'appointments',
        loadComponent: () =>
          import('./features/profile/my-appointments/my-appointments.component').then(m => m.MyAppointmentsComponent),
      },
      // {
      //   path: 'appointments/:id',
      //   loadComponent: () =>
      //     import('./features/patient/appointment-detail/appointment-detail.component').then(m => m.AppointmentDetailComponent),
      // },
      {
        path: 'profile',
        loadComponent: () =>
          import('./features/profile/patient-profile/patient-profile.component').then(m => m.PatientProfileComponent),
      },
      { path: '', redirectTo: 'appointments', pathMatch: 'full' },
    ],
  },

  // ── Fallback ─────────────────────────────────────────────
  { path: '**', redirectTo: '' },
];