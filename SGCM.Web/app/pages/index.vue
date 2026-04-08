<template>
  <div class="min-h-screen bg-charcoal-brown-50 md:pt-5 sm:px-6 lg:px-8">
    <div class="max-w-7xl mx-auto space-y-8">
      <!-- Encabezado de Bienvenida -->
      <section
        class="bg-white rounded-3xl shadow-sm border border-charcoal-brown-100 p-6 sm:p-8 flex flex-col  justify-between gap-6"
      >
        <div>
          <h1
            class="text-2xl md:text-3xl font-extrabold text-charcoal-brown-900 tracking-tight"
          >
            Hola,
            <span class="text-sky-reflection-600">{{
              authStore.user?.fullName || "Paciente"
            }}</span>
            👋
          </h1>
          <p class="mt-2 leading-5 md:text-lg text-charcoal-brown-500">
            Bienvenido a tu portal de salud. ¿Qué necesitas hacer hoy?
          </p>
        </div>
        <div class="flex-1">
          <button
            @click="navigateTo('/booking')"
            class="inline-flex items-center justify-center w-full md:w-auto px-6 py-4 border border-transparent rounded-lg text-base font-bold text-white bg-sky-reflection-500 hover:bg-sky-reflection-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-sky-reflection-500 transition-all transform hover:-translate-y-1"
          >
            <LucideCalendar class="h-6 w-6 mr-2" />
            <span class="hidden md:block text-nowrap">Agendar Cita</span>
          </button>
        </div>
      </section>

      <!-- Panel Principal de Citas -->
      <section
        class="bg-white rounded-3xl shadow-sm border border-charcoal-brown-100 overflow-hidden"
      >
        <div
          class="border-b border-charcoal-brown-100 bg-charcoal-brown-50/50 px-8 py-6 flex justify-between items-center"
        >
          <h2 class="text-xl font-bold text-charcoal-brown-900">
            Próximas Citas Médicas
          </h2>
        </div>

        <div class="p-8">
          <div class="text-center py-16">
            <div
              class="mx-auto w-24 h-24 bg-charcoal-brown-50 rounded-full flex items-center justify-center mb-4"
            >
              <LucideClock class="h-10 w-10 text-charcoal-brown-300" />
            </div>
            <h3 class="text-lg font-bold text-charcoal-brown-900 mb-2">
              No tienes citas programadas
            </h3>
            <p class="text-charcoal-brown-500 max-w-sm mx-auto">
              Aquí aparecerán todas tus consultas médicas. Para comenzar, agenda
              una cita con uno de nuestros especialistas.
            </p>
            <div class="mt-8">
              <button
                @click="navigateTo('/booking')"
                class="text-sky-reflection-600 font-semibold hover:text-sky-reflection-700 transition-colors"
              >
                Ver doctores disponibles →
              </button>
            </div>
          </div>

          <!-- Mockup Estructural (Terreno Preparado para cuando se conecte la DB) -->
          <!-- 
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div class="border border-charcoal-brown-100 rounded-2xl p-6 hover:shadow-md transition-shadow">
               ...estructura de cita...
            </div>
          </div> 
          -->
        </div>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { navigateTo } from "#app";
import { LucideCalendar, LucideClock } from "lucide-vue-next";

const authStore = useAuthStore();
</script>
