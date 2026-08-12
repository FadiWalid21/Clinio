// This file is the single merge point for the EN language.
// When you add a new feature (e.g. payments), you:
//   1. Create en/payments.ts
//   2. Import it here and add it to the object
//   3. Mirror the same key in ar/index.ts
// TypeScript will break the build if AR doesn't match the shape.

import { common } from './common';
import { auth } from './auth';
import { hero } from './hero';
import { steps } from './steps';
import { specialties } from './specialties';
import { testimonials } from './testimonials';
import { featuredDoctors } from './featuredDoctors';
import { footer } from './footer';
import { about } from './about';

export const en = {
  common,
  auth,
  hero,
  steps,
  specialties,
  testimonials,
  featuredDoctors,
  footer,
  about
  // payments,   ← add here when ready
  // medical,
  // appointments,
  // dashboard,
  // notifications,
};
