// src/app/shared/pipes/field-error.pipe.ts
import { Pipe, PipeTransform, inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ValidationService } from '@core/services/validation.service';
import { LanguageService } from '@core/services/language.service';

@Pipe({
  name: 'fieldError',
  standalone: true,
  pure: false  // ← impure so it re-runs when lang signal changes
})
export class FieldErrorPipe implements PipeTransform {
  private vs = inject(ValidationService);
  private ls = inject(LanguageService);

  transform(form: FormGroup, field: string): string | null {
    this.ls.lang(); // read the signal — makes pipe re-run on lang change
    return this.vs.getError(form, field);
  }
}