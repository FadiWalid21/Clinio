// import { Injectable, signal, computed } from '@angular/core';
// import { AppTranslations, AppLang, loadTranslations, en } from '@i18n';

// const STORAGE_KEY = 'clinio_lang';

// @Injectable({ providedIn: 'root' })
// export class LanguageService {

//   // --- Internal state ---

//   private _lang = signal<AppLang>(this.resolveInitialLang());

//   // _translations starts with EN synchronously — no flash of untranslated content.
//   // If the user's lang is AR, it gets replaced async in the constructor below.
//   private _translations = signal<AppTranslations>(en);

//   // --- Public readonly API ---

//   readonly lang = this._lang.asReadonly();
//   readonly isRtl = computed(() => this._lang() === 'ar');

//   // t() is what every component uses.
//   // It's a signal, so Angular tracks it reactively —
//   // when lang changes, every template reading t() updates automatically.
//   readonly t = this._translations.asReadonly();

//   constructor() {
//     const initial = this._lang();
//     this.applyToDocument(initial);

//     // If user prefers AR, load it immediately on startup.
//     // EN is already loaded synchronously above, so no layout shift.
//     if (initial !== 'en') {
//       this.loadAndApply(initial);
//     }
//   }

//   // --- Public methods ---

//   // Call this from a language toggle button.
//   // It's async because it waits for the translation chunk to load before switching.
//   async setLang(lang: AppLang): Promise<void> {
//     localStorage.setItem(STORAGE_KEY, lang);
//     this._lang.set(lang);
//     this.applyToDocument(lang);
//     await this.loadAndApply(lang);
//   }

//   // --- Private helpers ---

//   private async loadAndApply(lang: AppLang): Promise<void> {
//     const translations = await loadTranslations(lang);
//     this._translations.set(translations);
//   }

//   private resolveInitialLang(): AppLang {
//     const stored = localStorage.getItem(STORAGE_KEY) as AppLang | null;
//     if (stored === 'ar' || stored === 'en') return stored;

//     const browserLang = navigator.language?.toLowerCase() ?? '';
//     return browserLang.startsWith('ar') ? 'ar' : 'en';
//   }

//   private applyToDocument(lang: AppLang): void {
//     document.documentElement.lang = lang;
//     document.documentElement.dir = lang === 'ar' ? 'rtl' : 'ltr';
//   }
// }
