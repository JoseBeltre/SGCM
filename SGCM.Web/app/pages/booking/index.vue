<script setup lang="ts">
import { ref } from 'vue'
import { Check, ChevronLeft } from 'lucide-vue-next'
import StepDoctor from '~/components/booking/StepDoctor.vue'
import StepDate from '~/components/booking/StepDate.vue'
import StepTime from '~/components/booking/StepTime.vue'
import StepSpecialty from '~/components/booking/StepSpecialty.vue'

const step = ref(1)
const steps = ['Especialidad', 'Doctor', 'Fecha', 'Hora']

const nextStep = () => step.value++
const prevStep = () => step.value--
const { time } = useBooking()

</script>

<template>
  <div class="bg-white rounded-3xl shadow-sm border border-charcoal-brown-100 p-6 sm:p-8 md:p-10 w-full overflow-hidden relative">

    <!-- Header -->
    <div class="flex items-center justify-between mb-8">
      <div class="flex items-center gap-3">
        <button
          v-if="step > 1"
          @click="prevStep"
          class="w-10 h-10 rounded-full flex items-center justify-center bg-charcoal-brown-50 text-charcoal-brown-600 hover:bg-sky-reflection-100 hover:text-sky-reflection-700 transition-colors"
        >
          <ChevronLeft class="w-5 h-5" />
        </button>
        <div>
          <h1 class="text-2xl font-bold text-charcoal-brown-900 tracking-tight">Cita Médica</h1>
          <p class="text-sm text-charcoal-brown-500 mt-1">Sigue los pasos para agendar.</p>
        </div>
      </div>
    </div>

    <!-- Stepper -->
    <div class="relative mb-10">
      <div class="absolute top-1/2 left-0 right-0 h-[2px] bg-charcoal-brown-100 -translate-y-1/2 rounded-full mx-2"></div>
      <div class="absolute top-1/2 left-0 h-[2px] bg-sky-reflection-500 -translate-y-1/2 rounded-full transition-all duration-500 mx-2" :style="{ width: `${((step - 1) / (steps.length - 1)) * 100}%` }"></div>

      <div class="relative flex justify-between items-center px-2">
        <div v-for="(label, index) in steps" :key="index" class="flex flex-col items-center gap-2 z-10 bg-white sm:px-2">
          <div
            :class="[
              step > index ? 'bg-sky-reflection-500 text-white shadow-md' : 'bg-charcoal-brown-50 text-charcoal-brown-400 border border-charcoal-brown-200',
              'w-10 h-10 rounded-full flex items-center justify-center font-bold transition-all duration-300'
            ]"
          >
            <Check v-if="step > index + 1" class="w-5 h-5" />
            <span v-else>{{ index + 1 }}</span>
          </div>
          <span
            :class="[
              step > index ? 'text-sky-reflection-800 font-semibold' : 'text-charcoal-brown-400 font-medium',
              'text-[0.65rem] sm:text-xs uppercase tracking-wider hidden sm:block'
            ]"
          >
            {{ label }}
          </span>
        </div>
      </div>
    </div>

    <!-- Component Transition Container -->
    <div class="relative min-h-[300px]">
      <Transition name="slide-fade" mode="out-in">
        <StepSpecialty v-if="step === 1" @next="nextStep" />
        <StepDoctor v-else-if="step === 2" @next="nextStep" />
        <StepDate v-else-if="step === 3" @next="nextStep" />
        <StepTime v-else-if="step === 4" @next="nextStep" />
      </Transition>
    </div>

    <!-- Actions -->
    <div class="mt-10 flex justify-end">
      <button
        v-if="step === 4"
        :disabled="!time"
        @click="nextStep"
        class="bg-honey-bronze-500 hover:bg-honey-bronze-600 text-white font-bold py-3 px-8 rounded-full shadow-lg shadow-honey-bronze-200 transition-all disabled:opacity-50 disabled:cursor-not-allowed disabled:shadow-none focus:ring-4 focus:ring-honey-bronze-100"
      >
        Confirmar Cita
      </button>
    </div>
  </div>
</template>
