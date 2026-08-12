/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        'c-bg':            'rgb(var(--c-bg-rgb) / <alpha-value>)',
        'c-surface':       'rgb(var(--c-surface-rgb) / <alpha-value>)',
        'c-surface-2':     'rgb(var(--c-surface-2-rgb) / <alpha-value>)',
        'c-mint':          'rgb(var(--c-mint-rgb) / <alpha-value>)',
        'c-mint-deep':     'rgb(var(--c-mint-deep-rgb) / <alpha-value>)',
        'c-mint-soft':     'rgb(var(--c-mint-soft-rgb) / <alpha-value>)',
        'c-mint-text':     'rgb(var(--c-mint-text-rgb) / <alpha-value>)',
        'c-slate':         'rgb(var(--c-slate-rgb) / <alpha-value>)',
        'c-muted':         'rgb(var(--c-muted-rgb) / <alpha-value>)',
        'c-muted-light':   'rgb(var(--c-muted-light-rgb) / <alpha-value>)',
        'c-border':        'rgb(var(--c-border-rgb) / <alpha-value>)',
        'c-border-strong': 'rgb(var(--c-border-strong-rgb) / <alpha-value>)',
        'c-amber':         'rgb(var(--c-amber-rgb) / <alpha-value>)',
      },
    },
  },
  plugins: [],
}
