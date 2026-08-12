// src/app/core/services/validation.service.ts
import { inject, Injectable } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { LanguageService } from './language.service';

@Injectable({ providedIn: 'root' })
export class ValidationService {
  private ls = inject(LanguageService);
  // validation.service.ts — update getError
  getError(form: FormGroup, field: string): string | null {
    const control = form.get(field);
    if (!control || !control.touched) return null;

    if (control.invalid && control.errors) {
      return this.resolveMessage(control);
    }

    // ← was hardcoded, now goes through ls
    if (field === 'confirmPassword' && form.errors?.['mismatch']) {
      return this.ls.t().common.validation.passwordMismatch;
    }

    return null;
  }

  isInvalid(form: FormGroup, field: string): boolean {
    const control = form.get(field);
    return !!control && control.invalid && control.touched;
  }

  markAllTouched(form: FormGroup): void {
    Object.values(form.controls).forEach(control => control.markAsTouched());
  }

  private resolveMessage(control: AbstractControl): string | null {
    const errors = control.errors;
    if (!errors) return null;

    if (errors['required'])     return this.ls.t().common.validation.required;
    if (errors['email'])        return this.ls.t().common.validation.invalidEmail;
    if (errors['minlength'])    return this.ls.t().common.validation.minLength(errors['minlength'].requiredLength);
    if (errors['maxlength'])    return this.ls.t().common.validation.maxLength(errors['maxlength'].requiredLength);
    if (errors['min'])          return this.ls.t().common.validation.min(errors['min'].min);
    if (errors['max'])          return this.ls.t().common.validation.max(errors['max'].max);
    if (errors['pattern'])      return this.ls.t().common.validation.pattern;
    if (errors['mismatch'])     return this.ls.t().common.validation.passwordMismatch;
    if (errors['egyptianPhone']) return this.ls.t().common.validation.invalidPhone;

    return this.ls.t().common.validation.invalidValue;
  }
}