<template>
  <div class="min-h-[80vh] flex flex-col justify-center py-12 sm:px-6 lg:px-8">

    <div class="sm:mx-auto sm:w-full sm:max-w-xl">
      <div
      class="bg-white py-8 px-4 shadow-xl shadow-charcoal-brown-100/50 sm:rounded-3xl sm:px-10 border border-charcoal-brown-100">

      <div class="sm:mx-auto sm:w-full sm:max-w-md">
        <h2 class="text-center text-3xl font-extrabold text-sky-600 tracking-tight">
          Crear una cuenta
        </h2>
        <p class="mb-8 text-center text-sm text-charcoal-brown-500">
          Registrate como paciente para gestionar tus citas
        </p>
      </div>
      <div v-if="authStore.error" class="bg-red-50 border-l-4 border-red-500 p-4 mb-4">
          <p class="text-sm text-red-700">{{ authStore.error }}</p>
        </div>

        <form class="space-y-5" @submit.prevent="handleRegister">

          <div class="grid grid-cols-1 sm:grid-cols-2 gap-5">
            <div>
              <label for="firstName" class="block text-sm font-medium text-charcoal-brown-700">Nombre completo</label>
              <div class="mt-1">
                <input id="firstName" v-model="form.fullName" name="firstName" type="text"
                  class="appearance-none block w-full px-3 py-3 border border-charcoal-brown-200 rounded-xl shadow-sm placeholder-charcoal-brown-300 focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm bg-charcoal-brown-50/30 transition-colors"
                  :class="{ 'border-red-500 focus:ring-red-500': validationErrors.fullName }">
              </div>
              <p v-if="validationErrors.fullName" class="mt-2 text-sm text-red-600">{{ validationErrors.fullName }}</p>
            </div>
            <div>
              <label for="dateOfBirth" class="block text-sm font-medium text-charcoal-brown-700">Fecha de
                Nacimiento</label>
              <div class="mt-1">
                <input id="dateOfBirth" v-model="form.dateOfBirth" name="dateOfBirth" type="date"
                  class="appearance-none block w-full px-3 py-3 border border-charcoal-brown-200 rounded-xl shadow-sm placeholder-charcoal-brown-300 focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm bg-charcoal-brown-50/30 transition-colors"
                  :class="{ 'border-red-500 focus:ring-red-500': validationErrors.dateOfBirth }">
              </div>
              <p v-if="validationErrors.dateOfBirth" class="mt-2 text-sm text-red-600">{{ validationErrors.dateOfBirth
                }}</p>
            </div>
          </div>

          <div>
            <label for="email" class="block text-sm font-medium text-charcoal-brown-700">
              Correo electrónico
            </label>
            <div class="mt-1">
              <input id="email" v-model="form.email" name="email" type="email" autocomplete="email"
                class="appearance-none block w-full px-3 py-3 border border-charcoal-brown-200 rounded-xl shadow-sm placeholder-charcoal-brown-300 focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm bg-charcoal-brown-50/30 transition-colors"
                :class="{ 'border-red-500 focus:ring-red-500': validationErrors.email }">
            </div>
            <p v-if="validationErrors.email" class="mt-2 text-sm text-red-600">{{ validationErrors.email }}</p>
          </div>

          <div class="grid grid-cols-1 sm:grid-cols-2 gap-5">
            <div>
              <label for="document" class="block text-sm font-medium text-charcoal-brown-700">
                Cédula de Identidad
              </label>
              <div class="mt-1">
                <input id="document" v-model="form.nationalId" name="document" type="text"
                  placeholder="Ej: 40212345678"
                  class="appearance-none block w-full px-3 py-3 border border-charcoal-brown-200 rounded-xl shadow-sm placeholder-charcoal-brown-300 focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm bg-charcoal-brown-50/30 transition-colors"
                  :class="{ 'border-red-500 focus:ring-red-500': validationErrors.nationalId }">
              </div>
              <p class="mt-1 text-xs text-charcoal-brown-400">Sin guiones. Máximo 11 números.</p>
              <p v-if="validationErrors.nationalId" class="mt-2 text-sm text-red-600">{{ validationErrors.nationalId }}
              </p>
            </div>
            <div>
              <label for="phone" class="block text-sm font-medium text-charcoal-brown-700">
                Celular (Opcional)
              </label>
              <div class="mt-1">
                <input id="phone" v-model="form.phone" name="phone" type="tel"
                  class="appearance-none block w-full px-3 py-3 border border-charcoal-brown-200 rounded-xl shadow-sm placeholder-charcoal-brown-300 focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm bg-charcoal-brown-50/30 transition-colors"
                  :class="{ 'border-red-500 focus:ring-red-500': validationErrors.phone }">
              </div>
              <p v-if="validationErrors.phone" class="mt-2 text-sm text-red-600">{{ validationErrors.phone }}</p>
            </div>
          </div>

          <div>
            <label for="password" class="block text-sm font-medium text-charcoal-brown-700">
              Contraseña
            </label>
            <div class="mt-1">
              <input id="password" v-model="form.password" name="password" type="password"
                class="appearance-none block w-full px-3 py-3 border border-charcoal-brown-200 rounded-xl shadow-sm placeholder-charcoal-brown-300 focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm bg-charcoal-brown-50/30 transition-colors"
                :class="{ 'border-red-500 focus:ring-red-500': validationErrors.password }">
            </div>
            <p v-if="validationErrors.password" class="mt-2 text-sm text-red-600">{{ validationErrors.password }}</p>
          </div>

          <div class="pt-2">
            <button type="submit" :disabled="authStore.loading"
              class="w-full flex justify-center py-3 px-4 border border-transparent rounded-xl shadow-md shadow-sky-reflection-200 text-sm font-bold text-white bg-sky-reflection-500 hover:bg-sky-reflection-600 disabled:opacity-50 disabled:cursor-not-allowed focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-sky-reflection-500 transition-all">
              {{ authStore.loading ? 'Registrando...' : 'Registrar cuenta de paciente' }}
            </button>
          </div>
        </form>

        <div class="mt-6 text-center">
          <span class="text-sm text-charcoal-brown-500">¿Ya tienes una cuenta? </span>
          <button @click="navigateTo('/auth/login')"
            class="text-sm font-bold text-sky-reflection-600 hover:text-sky-reflection-500 transition-colors">
            Inicia sesión aquí
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { registerSchema } from '~/schemas/auth.schema'
import { navigateTo } from '#app'

definePageMeta({ layout: 'auth' })


const authStore = useAuthStore()

const form = ref({
  fullName: '',
  nationalId: '',
  dateOfBirth: '',
  phone: '',
  email: '',
  password: ''
})

const validationErrors = ref<Record<string, string>>({})

const handleRegister = async () => {
  validationErrors.value = {}

  const result = registerSchema.safeParse(form.value)
  if (!result.success) {
    const formattedErrors: Record<string, string> = {}
    result.error.issues.forEach((err: any) => {
      if (err.path[0]) {
        formattedErrors[err.path[0].toString()] = err.message
      }
    })
    validationErrors.value = formattedErrors
    return
  }

  await authStore.register(form.value)
}
</script>
