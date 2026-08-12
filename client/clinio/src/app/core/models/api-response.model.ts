export type ProblemDetails = {
  status: number;
  title: string;
  detail?: string;
  errors?: ValidationError[];
}

export type ValidationError = {
  propertyName: string;
  errorMessage: string;
}

// Success wrapper
export type ApiResult<T> = {
  isAuthenticated?: boolean;
  message?: string;
  value?: T;
}

// Auth specific
export type AuthResponse = {
  isAuthenticated: boolean;
  message: string;
  username: string;
  email: string;
  token: string;
  refreshToken: string;
  refreshTokenExpiration: string;
}