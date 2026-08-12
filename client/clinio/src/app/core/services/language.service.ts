import { Injectable, signal, computed } from '@angular/core';
import { AppTranslations, AppLang, loadTranslations, en } from '@i18n';

const STORAGE_KEY = 'clinio_lang';

@Injectable({ providedIn: 'root' })
export class LanguageService {
  private _lang = signal<AppLang>(this.resolveInitialLang());
  private _translations = signal<AppTranslations>(en);

  readonly lang = this._lang.asReadonly();
  readonly isRtl = computed(() => this._lang() === 'ar');
  readonly t = this._translations.asReadonly();

  constructor() {
    const initial = this._lang();
    this.applyToDocument(initial);

    if (initial !== 'en') {
      this.loadAndApply(initial);
    }
  }

  async setLang(lang: AppLang): Promise<void> {
    localStorage.setItem(STORAGE_KEY, lang);
    this._lang.set(lang);
    this.applyToDocument(lang);
    await this.loadAndApply(lang);
  }

  toggleLanguage(): void {
  const next: AppLang = this._lang() === 'en' ? 'ar' : 'en';
  this.setLang(next); // setLang is async but fire-and-forget here is fine
}

  private async loadAndApply(lang: AppLang): Promise<void> {
    const translations = await loadTranslations(lang);
    this._translations.set(translations);
  }

  private resolveInitialLang(): AppLang {
    const stored = localStorage.getItem(STORAGE_KEY) as AppLang | null;
    if (stored === 'ar' || stored === 'en') return stored;
    const browserLang = navigator.language?.toLowerCase() ?? '';
    return browserLang.startsWith('ar') ? 'ar' : 'en';
  }

  private applyToDocument(lang: AppLang): void {
    document.documentElement.lang = lang;
    document.documentElement.dir = lang === 'ar' ? 'rtl' : 'ltr';
  }
}