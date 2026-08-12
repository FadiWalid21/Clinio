// login.component.ts
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ValidationService } from '@core/services/validation.service';
import { AuthService } from '@core/services/auth.service';
import { FieldErrorComponent } from '@shared/components/field-error/field-error.component';
import { LoginRequest } from '@core/models/api-request.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, FieldErrorComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  validation = inject(ValidationService);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  isLoading = false;
  serverError = '';

  submit(): void {
    if (this.form.invalid) {
      this.validation.markAllTouched(this.form);
      return;
    }

    this.isLoading = true;
    this.serverError = '';

    const loginCommand: LoginRequest = {
      email: this.form.get('email')?.value ?? '',
      password: this.form.get('password')?.value ?? ''
    };

    this.auth.login(loginCommand).subscribe({
      next: (response) => {
        this.isLoading = false;
        console.log('Login successful:', response);
        this.router.navigate(['/home']);
      },
      error: (err ) => {
        console.log('Login error:', err);
        this.isLoading = false;
        this.serverError = err?.detail ?? 'Something went wrong.';
        this.cdr.detectChanges();
      }
    });
  }
}