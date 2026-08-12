import { Component, inject, input } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { FormGroup } from '@angular/forms';
import { Observable, switchMap } from 'rxjs';
import { ValidationService } from '@core/services/validation.service';
import { LanguageService } from '@core/services/language.service';

@Component({
  selector: 'app-field-error',
  standalone: true,
  imports: [],
  templateUrl: './field-error.component.html',
  styleUrl: './field-error.component.scss',
})
export class FieldErrorComponent {
  form = input.required<FormGroup>();
  field = input.required<string>();

  private validation = inject(ValidationService);
  private ls = inject(LanguageService);

  // converted at field level — valid injection context
  private lang$ = toObservable(this.ls.lang);

  message = toSignal(
    toObservable(this.form).pipe(
      switchMap(form => {
        const control = form.get(this.field());
        if (!control) return [];

        return new Observable<string | null>(observer => {
          const check = () => observer.next(this.validation.getError(form, this.field()));

          Promise.resolve().then(check);

          const statusSub = control.statusChanges.subscribe(check);
          const langSub = this.lang$.subscribe(check); // ← use pre-converted observable

          return () => {
            statusSub.unsubscribe();
            langSub.unsubscribe();
          };
        });
      })
    ),
    { initialValue: null }
  );
}