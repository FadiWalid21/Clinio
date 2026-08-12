// Must mirror en/index.ts exactly — same keys, same structure.
// TypeScript enforces this via AppTranslations type in types.ts.
// If you add a key to EN and forget it here, the build will fail. That's intentional.

import { common } from './common';
import { auth } from './auth';
import { hero } from './hero';
import { steps } from './steps';
import { specialties } from './specialties';
import { testimonials } from './testimonials';
import { featuredDoctors } from './featuredDoctors';
import { footer } from './footer';
import { about } from './about';

export const ar = {
  common,
  auth,
  hero,
  steps,
  specialties,
  testimonials,
  featuredDoctors,
  footer,
  about
  // payments,
  // medical,
  // appointments,
  // dashboard,
  // notifications,
};
