import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LanguageService } from '@core/services/language.service';

export const languageInterceptor: HttpInterceptorFn = (req, next) => {
  const languageService = inject(LanguageService);

  const cloned = req.clone({
    headers: req.headers.set('Accept-Language', languageService.lang())
  });

  return next(cloned);
};