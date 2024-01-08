/** @type {import('tailwindcss').Config} */

const colors = require("tailwindcss/colors");
const withMT = require("@material-tailwind/html/utils/withMT");

module.exports = withMT({
  mode: 'jit',
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {},
    colors: {
      transparent: 'transparent',
      current: 'currentColor',
      background: colors.neutral,
      'navigator': {
        'background': colors.slate[900],
        'background-highlight': colors.slate[800],
        'low-contrast': colors.neutral[400],
        'low-contrast-highlight': colors.neutral[500],
        'high-contrast': colors.neutral[300],
        'high-contrast-highlight': colors.neutral[50],
        'navigator-scroll-thumb': colors.slate[200],
        'navigator-scroll-thumb-hover':colors.slate[100]
      },
      'page-header': {
        'background': colors.slate[50]
      },
      'content-container': {
        'background': colors.slate[200]
      },
      'content-background': colors.white,      
      'high-contrast': colors.black,
      'low-contrast': colors.gray,
      primary: colors.sky
    }
  },
  plugins: [
  ],
});

