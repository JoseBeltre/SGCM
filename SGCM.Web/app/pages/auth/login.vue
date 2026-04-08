<template>
  <div class="min-h-[80vh] flex flex-col justify-center py-12 sm:px-6 lg:px-8">

    <div class="sm:mx-auto sm:w-full sm:max-w-md">
      <div class="bg-white py-8 px-4 shadow-xl shadow-charcoal-brown-100/50 sm:rounded-3xl sm:px-10 border border-charcoal-brown-100">
        <div class="sm:mx-auto sm:w-full sm:max-w-md">
          <h2 class="text-center text-3xl font-extrabold text-palm-leaf-700 tracking-tight">
            Bienvenido de vuelta
          </h2>
          <p class="mb-8 text-center text-sm text-charcoal-brown-500">
            Ingresa a tu cuenta para gestionar tus citas médicas
          </p>
        </div>
        <form class="space-y-6" @submit.prevent="handleLogin">

          <div v-if="authStore.error" class="bg-red-50 border-l-4 border-red-500 p-4 mb-4">
            <p class="text-sm text-red-700">{{ authStore.error }}</p>
          </div>

          <div>
            <label for="email" class="block text-sm font-medium text-charcoal-brown-700">
              Correo electrónico
            </label>
            <div class="mt-1">
              <input id="email" v-model="form.email" name="email" type="email" autocomplete="email" class="appearance-none block w-full px-3 py-3 border border-charcoal-brown-200 rounded-xl shadow-sm placeholder-charcoal-brown-300 focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm bg-charcoal-brown-50/30 transition-colors" :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': validationErrors.email }">
            </div>
            <p v-if="validationErrors.email" class="mt-2 text-sm text-red-600">{{ validationErrors.email }}</p>
          </div>

          <div>
            <label for="password" class="block text-sm font-medium text-charcoal-brown-700">
              Contraseña
            </label>
            <div class="mt-1">
              <input id="password" v-model="form.password" name="password" type="password" autocomplete="current-password" class="appearance-none block w-full px-3 py-3 border border-charcoal-brown-200 rounded-xl shadow-sm placeholder-charcoal-brown-300 focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm bg-charcoal-brown-50/30 transition-colors" :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': validationErrors.password }">
            </div>
            <p v-if="validationErrors.password" class="mt-2 text-sm text-red-600">{{ validationErrors.password }}</p>
          </div>

          <div class="flex items-center justify-between">
            <div class="flex items-center">
              <input id="remember-me" name="remember-me" type="checkbox" class="h-4 w-4 focus:ring-sky-reflection-500 border-charcoal-brown-300 rounded text-sky-reflection-500 bg-charcoal-brown-50/50">
              <label for="remember-me" class="ml-2 block text-sm text-charcoal-brown-700">
                Recordarme
              </label>
            </div>
          </div>

          <div>
            <button type="submit" :disabled="authStore.loading" class="w-full flex justify-center items-center py-3 px-4 border border-transparent rounded-xl shadow-md shadow-palm-leaf-200 text-sm font-bold text-white bg-palm-leaf-600 hover:bg-palm-leaf-700 disabled:opacity-50 disabled:cursor-not-allowed focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-palm-leaf-500 transition-all">
              <span v-if="authStore.loading" class="mr-2">Cargando...</span>
              Ingresar al sistema
            </button>
          </div>
        </form>

        <div class="mt-6">
          <div class="relative">
            <div class="absolute inset-0 flex items-center">
              <div class="w-full border-t border-charcoal-brown-200"></div>
            </div>
            <div class="relative flex justify-center text-sm">
              <span class="px-2 bg-white text-charcoal-brown-500">
                ¿Eres paciente nuevo?
              </span>
            </div>
          </div>

          <div class="mt-6">
            <button @click="navigateTo('/auth/register')" class="w-full flex justify-center py-3 px-4 border border-charcoal-brown-200 rounded-xl shadow-sm text-sm font-bold text-charcoal-brown-700 bg-white hover:bg-charcoal-brown-50 focus:outline-none transition-colors">
              Crear una cuenta
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { loginSchema } from '~/schemas/auth.schema'
import { navigateTo } from '#app'

definePageMeta({layout: 'auth'})

const authStore = useAuthStore()

const form = ref({
  email: '',
  password: ''
})

const validationErrors = ref<Record<string, string>>({})

const handleLogin = async () => {
  validationErrors.value = {}

  const result = loginSchema.safeParse(form.value)
  if (!result.success) {
    const formattedErrors: Record<string, string> = {}
    result.error.issues.forEach(err => {
      if (err.path[0]) {
        formattedErrors[err.path[0].toString()] = err.message
      }
    })
    validationErrors.value = formattedErrors
    return
  }

  await authStore.login(form.value)
}
</script>
