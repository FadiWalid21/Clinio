import { HttpInterceptorFn, HttpErrorResponse, HttpContext, HttpContextToken } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { HotToastService } from '@ngxpert/hot-toast';

export const SKIP_ERROR_TOAST = new HttpContextToken<boolean>(() => false);

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(HotToastService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      console.log('🔴 Error Interceptor:', {
    status: error.status,
    url: error.url,
    problem: error.error,
    message: error.error?.detail ?? getFallbackMessage(error.status)
  });
      if (req.context.get(SKIP_ERROR_TOAST)) return throwError(() => error);
      if (error.status === 401) return throwError(() => error);

      const message = error.error?.detail ?? getFallbackMessage(error.status);

      toast.error(message, { duration: 5000 });

      return throwError(() => error);
    })
  );
};

function getFallbackMessage(status: number): string {
  switch (status) {
    case 400: return 'Invalid request';
    case 403: return 'Access denied';
    case 404: return 'Resource not found';
    case 500: return 'An unexpected error occurred. Please try again later.';
    default:  return 'Something went wrong';
  }
}