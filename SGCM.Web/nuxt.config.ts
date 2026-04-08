// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  ssr: false,
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  css: ['@/assets/css/main.css'],
  modules: [
    '@nuxtjs/tailwindcss',
    '@nuxtjs/google-fonts',
    '@pinia/nuxt'
  ],
  googleFonts: {
    families: {
      Inter: [300, 400, 500, 600, 700]
    },
    display: 'swap'
  },
  build: {
    transpile: ['@vuepic/vue-datepicker']
  },
  routeRules: {
    '/api/**': { proxy: 'http://localhost:5055/api/**' }
  }
})
