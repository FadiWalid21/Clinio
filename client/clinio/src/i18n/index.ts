// Public API of the i18n folder.
// Everything a consumer (LanguageService, components) needs is re-exported here.
// This means imports look like:
//   import { AppTranslations, AppLang, loadTranslations } from '@i18n';
// instead of:
//   import { AppTranslations } from '@i18n/types';
//   import { loadTranslations } from '@i18n/loader';

export type { AppTranslations } from './types';
export type { AppLang } from './loader';
export { loadTranslations } from './loader';
export { en } from './en'; // exported so LanguageService can use it as sync default
