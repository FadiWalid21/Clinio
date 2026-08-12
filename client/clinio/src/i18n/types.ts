import { en } from './en';

// AppTranslations is derived from the EN object.
// EN is the source of truth — AR must always match this shape.
//
// Why EN and not a manual interface?
// Because if you add a key to EN, TypeScript automatically
// makes AR fail compilation until you add the same key there.
// You never need to maintain a separate type definition.

export type AppTranslations = typeof en;
