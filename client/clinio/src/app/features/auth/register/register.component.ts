import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { RegisterPatientRequest } from '@core/models/api-request.model';
import { AuthService } from '@core/services/auth.service';
import { ValidationService } from '@core/services/validation.service';
import { AppValidators } from '@core/validators/app.validators';
import { FieldErrorComponent } from "@shared/components/field-error/field-error.component";

@Component({
  selector: 'app-register', 
  standalone: true,
  imports: [RouterLink, FieldErrorComponent,ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  validation = inject(ValidationService);

    isLoading = false;
    serverError = '';


  form = this.fb.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required , Validators.minLength(8)]],
    dateOfBirth: ['', Validators.required], 
    gender: ['', Validators.required],
    terms: [true, [Validators.requiredTrue]],
  },{
     validators: AppValidators.passwordMatch('password', 'confirmPassword')
  });


  onSubmit() {
    if (this.form.invalid) {
      this.validation.markAllTouched(this.form);
      return;
    }

    this.isLoading = true;
    this.serverError = '';

    const registerPatientRequest: RegisterPatientRequest = {
    firstName: this.form.value.firstName!,
    lastName: this.form.value.lastName!,
    email: this.form.value.email!,
    password: this.form.value.password!,
    dateOfBirth: this.form.value.dateOfBirth!,
    gender: this.form.value.gender!,
  };

    this.auth.registerPatient(registerPatientRequest).subscribe({
      next: (response) => {
        this.isLoading = false;
        console.log('Registration successful:', response);
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.log('Registration error:', err);
        this.isLoading = false;
        this.serverError = err?.detail ?? 'Something went wrong.';
        this.cdr.detectChanges();
      }
    });
  }
}
