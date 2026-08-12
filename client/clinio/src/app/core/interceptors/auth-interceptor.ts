import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '@core/services/auth.service';
import { catchError, switchMap, throwError } from 'rxjs';

const PUBLIC_ENDPOINTS = ['auth/login', 'auth/register', 'auth/refresh'];

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  const isPublic = PUBLIC_ENDPOINTS.some(endpoint => req.url.includes(endpoint));
  if (isPublic) return next(req);

  const token = authService.token();
  const cloned = token
    ? req.clone({ headers: req.headers.set('Authorization', `Bearer ${token}`) })
    : req;

  return next(cloned).pipe(
    catchError((error: HttpErrorResponse) => {
      const status = error.status as number;

      if (status === 403) return throwError(() => error);
      if (status !== 401) return throwError(() => error);

      const currentToken = authService.token();
      const refreshToken = localStorage.getItem('refreshToken');

      if (!currentToken || !refreshToken) {
        authService.clearSession();
        return throwError(() => error);
      }

      return authService.refresh(currentToken, refreshToken).pipe(
        switchMap(response => {
          const retried = req.clone({
            headers: req.headers.set('Authorization', `Bearer ${response.token}`)
          });
          return next(retried);
        }),
        catchError(refreshError => {
          authService.clearSession();
          return throwError(() => refreshError);
        })
      );
    })
  );
};