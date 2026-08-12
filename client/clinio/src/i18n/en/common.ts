export const common = {
  // --- Buttons ---
  // Every reusable action label lives here.
  // Never write "Save" or "Cancel" inside a feature file — reference this.

  nav: {
  findDoctor: 'Find a doctor',
  specialties: 'Specialties',
  howItWorks: 'How it works',
  login: 'Log in',
  getStarted: 'Get started',
},

  buttons: {
    save: 'Save',
    cancel: 'Cancel',
    delete: 'Delete',
    confirm: 'Confirm',
    back: 'Back',
    edit: 'Edit',
    add: 'Add',
    close: 'Close',
    search: 'Search',
    filter: 'Filter',
    clear: 'Clear',
    submit: 'Submit',
    retry: 'Try Again',
    loadMore: 'Load More',
  },

  // --- Validation ---
  // Frontend form validation messages.
  // NOTE: Backend validation errors come localized via the API — do NOT duplicate them here.
  // These are only for reactive form validators that run before the request is sent.
  validation: {
    required: 'This field is required',
    invalidEmail: 'Please enter a valid email address',
    invalidPhone: 'Please enter a valid phone number',
    passwordMismatch: 'Passwords do not match',
    minLength: (n: number) => `Minimum ${n} characters`,
    maxLength: (n: number) => `Maximum ${n} characters`,
    min: (n: number) => `Minimum value is ${n}`,
    max: (n: number) => `Maximum value is ${n}`,
    pattern: 'Invalid format',
    invalidValue: 'Invalid value',  // ← fallback for unknown errors
  },

  // --- Status / Feedback ---
  // Generic UI states shown in tables, lists, loading spinners, etc.
  status: {
    loading: 'Loading...',
    saving: 'Saving...',
    noData: 'No data available',
    noResults: 'No results found',
    error: 'Something went wrong. Please try again.',
  },

  // --- Pagination ---
  pagination: {
    previous: 'Previous',
    next: 'Next',
    page: (n: number) => `Page ${n}`,
    of: 'of',
  },

  // --- Confirmation Dialog ---
  // Used in a shared ConfirmDialogComponent.
  confirm: {
    title: 'Are you sure?',
    deleteMessage: 'This action cannot be undone.',
    yes: 'Yes, proceed',
    no: 'No, cancel',
  },
  aboutUs: {
    title: 'About Us',
  },
};
