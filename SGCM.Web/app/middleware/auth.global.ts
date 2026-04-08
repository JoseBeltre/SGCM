import { defineNuxtRouteMiddleware, navigateTo } from '#app'

export default defineNuxtRouteMiddleware(async (to, from) => {
  const authStore = useAuthStore()

  // Ensure state is hydrated BEFORE making any routing decisions in SPA
  if (!authStore.isInitialized) {
    await authStore.fetchUser()
  }

  // Lista de rutas que no requieren estar logueado
  const publicRoutes = ['/auth/login', '/auth/register']

  // Si trata de entrar a un área privada (ej: /booking) sin estar logueado, lo mandamos al login
  if (!publicRoutes.includes(to.path) && !authStore.isAuthenticated) {
    return navigateTo('/auth/login')
  }

  // Opcional: Si está logueado y trata de ver login/register, mándalo al core directo
  if ((to.path === '/auth/login' || to.path === '/auth/register') && authStore.isAuthenticated) {
    return navigateTo('/')
  }
})
