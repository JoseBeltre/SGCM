<script setup lang="ts">
import { LucideAlertTriangle, LucideHome } from 'lucide-vue-next'

const props = defineProps({
  error: Object
})

const handleError = () => clearError({ redirect: '/' })
</script>

<template>
  <div class="min-h-screen bg-charcoal-brown-50 flex items-center justify-center px-4 py-12 sm:px-6 lg:px-8">
    <div class="max-w-md w-full text-center space-y-8 bg-white p-10 rounded-3xl shadow-sm border border-charcoal-brown-100">
      <div>
        <div class="mx-auto flex items-center justify-center h-24 w-24 rounded-full bg-red-100">
          <LucideAlertTriangle class="h-12 w-12 text-red-600" aria-hidden="true" />
        </div>
        
        <h2 class="mt-6 text-3xl font-extrabold text-charcoal-brown-900">
          Oops, algo salió mal.
        </h2>
        
        <p class="mt-4 text-charcoal-brown-500">
          {{ error?.statusCode === 404 ? 'La página que buscas no existe o fue movida.' : 'Ocurrió un error inesperado al procesar tu solicitud.' }}
        </p>

        <div v-if="error?.message && error.statusCode !== 404" class="mt-4 text-xs font-mono text-red-600 bg-red-50 p-2 rounded truncate max-w-full overflow-hidden text-center mx-auto">
          {{ error.message }}
        </div>
      </div>
      
      <div class="mt-8">
        <button
          @click="handleError"
          class="w-full flex justify-center py-3 px-4 border border-transparent rounded-lg shadow-sm text-sm font-semibold text-white bg-sky-reflection-500 hover:bg-sky-reflection-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-sky-reflection-500 transition-colors"
        >
          <LucideHome class="mr-2 h-5 w-5" />
          Volver al Inicio
        </button>
      </div>
    </div>
  </div>
</template>
