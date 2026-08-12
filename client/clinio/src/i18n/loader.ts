import { AppTranslations } from './types';

export type AppLang = 'ar' | 'en';

// loadTranslations uses dynamic import() so Angular's build system
// splits each language into its own JS chunk.
//
// What this means in practice:
//   - On first load, only the user's language is downloaded
//   - Switching to AR downloads ~X KB — not the whole app
//   - The return type is enforced as AppTranslations,
//     so if AR is missing a key the build fails here too

export async function loadTranslations(lang: AppLang): Promise<AppTranslations> {
  if (lang === 'ar') {
    const module = await import('./ar');
    return module.ar;
  }

  const module = await import('./en');
  return module.en;
}
