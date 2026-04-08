<template>
  <div class="min-h-screen bg-charcoal-brown-50 font-sans text-charcoal-brown-900 flex flex-col">
    <!-- Navbar -->
    <header class="bg-white shadow-sm border-b border-charcoal-brown-100 sticky top-0 z-50">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16">
          <div class="flex items-center">
            <div class="flex-shrink-0 flex items-center gap-2 cursor-pointer" @click="navigateTo('/')">
              <span class="font-bold text-xl text-sky-reflection-900 tracking-tight">SGCM</span>
            </div>
          </div>

          <div class="flex items-center space-x-4">
            <template v-if="authStore.isAuthenticated">
              <div class="text-right flex flex-col justify-center">
                <span class="text-sm font-bold text-sky-reflection-900 leading-tight">{{ authStore.user?.fullName || 'Usuario' }}</span>
                <span v-if="authStore.user?.nationalId" class="text-xs text-charcoal-brown-500 font-medium">Cédula: {{ authStore.user.nationalId }}</span>
              </div>
              <button @click="authStore.logout" class="text-xs font-semibold bg-charcoal-brown-100 text-charcoal-brown-700 px-3 py-1.5 rounded-full hover:bg-charcoal-brown-200 transition-colors ml-2">Salir</button>
            </template>
            <template v-else>
              <button class="text-sm font-medium text-charcoal-brown-600 hover:text-sky-reflection-600 transition-colors hidden sm:block" @click="navigateTo('/auth/login')">Iniciar Sesión</button>
              <button class="text-sm font-semibold bg-sky-reflection-500 text-white px-4 py-2 rounded-full hover:bg-sky-reflection-600 hover:shadow-md transition-all" @click="navigateTo('/auth/register')">Registrarse</button>
            </template>
          </div>
        </div>
      </div>
    </header>

    <!-- Contenido principal -->
    <main class="flex-grow flex flex-col justify-center py-10 px-4 sm:px-6 lg:px-8 items-center w-full">
      <div class="w-full max-w-3xl">
        <slot />
      </div>
    </main>

    <!-- Footer -->
    <footer class="bg-white border-t border-charcoal-brown-100 py-6 text-center">
      <p class="text-sm text-charcoal-brown-600">&copy; {{ new Date().getFullYear() }} Sistema de Gestión de Citas Médicas. Desarrollado por José Beltre.</p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { navigateTo } from '#app'
const authStore = useAuthStore()
</script>

<style>
/* Global Transitions */
.page-enter-active,
.page-leave-active {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}
.page-enter-from,
.page-leave-to {
  opacity: 0;
  transform: translateY(8px);
}

/* Stepper Transitions */
.slide-fade-enter-active {
  transition: all 0.4s ease-out;
}
.slide-fade-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}
.slide-fade-enter-from {
  transform: translateX(15px);
  opacity: 0;
}
.slide-fade-leave-to {
  transform: translateX(-15px);
  opacity: 0;
}
</style>
