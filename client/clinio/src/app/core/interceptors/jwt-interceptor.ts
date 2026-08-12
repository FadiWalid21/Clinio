// src/app/core/interceptors/jwt.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

const PUBLIC_ENDPOINTS = ['auth/login', 'auth/register'];

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  const isPublic = PUBLIC_ENDPOINTS.some(endpoint => req.url.includes(endpoint));
  if (isPublic) return next(req);

  const token = authService.token();
  if (!token) return next(req);

  const cloned = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`)
  });

  return next(cloned);
};