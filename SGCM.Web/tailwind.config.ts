import type { Config } from 'tailwindcss'

export default {
  content: [
    "./app/components/**/*.{js,vue,ts}",
    "./app/layouts/**/*.vue",
    "./app/pages/**/*.vue",
    "./app/plugins/**/*.{js,ts}",
    "./app.vue",
    "./error.vue"
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Inter', 'sans-serif']
      },
      colors: {
        'sky-reflection': {
          50: "#ecf4f9",
          100: "#d8e9f3",
          200: "#b2d4e6",
          300: "#8bbeda",
          400: "#65a9cd",
          500: "#3e93c1",
          600: "#32769a",
          700: "#255874",
          800: "#193b4d",
          900: "#0c1d27",
          950: "#09151b"
        },
        'charcoal-brown': {
          50: "#f2f3f2",
          100: "#e5e7e4",
          200: "#cbcfc9",
          300: "#b2b6af",
          400: "#989e94",
          500: "#7e8679",
          600: "#656b61",
          700: "#4c5049",
          800: "#323630",
          900: "#191b18",
          950: "#121311"
        },
        'palm-leaf': {
          50: "#f3f6ef",
          100: "#e7edde",
          200: "#cfdbbd",
          300: "#b7c99c",
          400: "#9fb77b",
          500: "#87a45b",
          600: "#6c8448",
          700: "#516336",
          800: "#364224",
          900: "#1b2112",
          950: "#13170d"
        },
        'honey-bronze': {
          50: "#fef6e7",
          100: "#fdedce",
          200: "#fbda9d",
          300: "#f9c86c",
          400: "#f7b53b",
          500: "#f5a30a",
          600: "#c48208",
          700: "#936206",
          800: "#624104",
          900: "#312102",
          950: "#221701"
        }
      }
    }
  },
  plugins: [],
} satisfies Config
