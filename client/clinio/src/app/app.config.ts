import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { routes } from './app.routes';
import { languageInterceptor } from '@core/interceptors/language-interceptor';
import { authInterceptor } from '@core/interceptors/auth-interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHotToastConfig } from '@ngxpert/hot-toast';
import { errorInterceptor } from '@core/interceptors/error-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        languageInterceptor,
        authInterceptor,
        errorInterceptor
      ])
    ),
    provideAnimationsAsync(),
    provideHotToastConfig({
      position: 'bottom-right',
      duration: 5000
    })
  ]
};