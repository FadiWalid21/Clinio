import { Injectable, signal } from '@angular/core';

const STORAGE_KEY = 'clinio-theme';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  isDark = signal(this.loadPreference());

  constructor() {
    this.applyTheme(this.isDark());
  }

  toggleTheme(): void {
    const next = !this.isDark();
    this.isDark.set(next);
    this.applyTheme(next);
    localStorage.setItem(STORAGE_KEY, next ? 'dark' : 'light');
  }

  private applyTheme(dark: boolean): void {
    document.documentElement.classList.toggle('dark', dark);
  }

  private loadPreference(): boolean {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored) return stored === 'dark';
    return window.matchMedia('(prefers-color-scheme: dark)').matches;
  }
}
