import { Component, inject, signal, computed, effect } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { LanguageService } from '@core/services/language.service';
import { ThemeService } from '@core/services/theme.service';
import { AuthService } from '@core/services/auth.service';
import { ProfileService } from '@core/services/profile.service';
import { ClickOutsideDirective } from '@shared/directives/click-outside';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, ClickOutsideDirective],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  protected ls = inject(LanguageService);
  protected authService = inject(AuthService);
  private themeService = inject(ThemeService);
  private profileService = inject(ProfileService);
  private router = inject(Router);

  isMenuOpen = signal(false);
  profileOpen = false;

  isDark = this.themeService.isDark;

  // ── derived from ProfileService's shared signal ──────────────
  profileImage = computed(() => this.profileService.profile()?.image ?? null);
  firstName = computed(() => this.profileService.profile()?.firstName ?? null);
  initials = computed(() => {
    const p = this.profileService.profile();
    if (!p) return null;
    const f = p.firstName?.charAt(0) ?? '';
    const l = p.lastName?.charAt(0) ?? '';
    return (f + l).toUpperCase() || null;
  });

  constructor() {
    effect(() => {
      if (this.authService.isAuthenticated()) {
        if (!this.profileService.profile()) {
          this.profileService.getMyProfile().subscribe();
        }
      } else {
        this.profileService.clearProfile();
      }
    });
  }

  toggleMenu(): void {
    this.isMenuOpen.update(v => !v);
  }

  toggleLanguage(): void {
    this.ls.toggleLanguage();
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }

  logout(): void {
    const refreshToken = localStorage.getItem('refreshToken') ?? '';

    this.authService.logout({ refreshToken }).subscribe({
      next: () => {
        this.profileService.clearProfile();
        this.router.navigate(['/']);
      },
      error: () => {
        // Even if the server call fails, clear the local session so the
        // UI doesn't stay stuck "logged in" with a dead token.
        this.authService.clearSession();
        this.profileService.clearProfile();
        this.router.navigate(['/']);
      },
    });
  }
}