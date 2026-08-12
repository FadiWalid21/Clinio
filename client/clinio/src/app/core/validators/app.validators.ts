import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class AppValidators {

  static egyptianPhone(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) return null;
      const valid = /^01[0125][0-9]{8}$/.test(control.value);
      return valid ? null : { egyptianPhone: true };
    };
  }

  static passwordMatch(passwordField: string, confirmField: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const password = group.get(passwordField)?.value;
      const confirm = group.get(confirmField)?.value;
      if (!password || !confirm) return null;
      return password === confirm ? null : { mismatch: true };
    };
  }

  static minAge(minAge: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) return null;
      const birthDate = new Date(control.value);
      const age = new Date().getFullYear() - birthDate.getFullYear();
      return age >= minAge ? null : { minAge: { required: minAge, actual: age } };
    };
  }
}