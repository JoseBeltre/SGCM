<script setup lang="ts">
import StepDoctor from '~/components/booking/StepDoctor.vue';
import StepDate from '~/components/booking/StepDate.vue';
import StepTime from '~/components/booking/StepTime.vue';
import StepSpecialty from '~/components/booking/StepSpecialty.vue'

const step = ref(1)
const nextStep = () => step.value++
const prevStep = () => step.value--
const { time } = useBooking()

</script>

<template>
  <div>
    <h1 class="text-3xl font-bold mb-4">Agendar cita</h1>
  </div>
  <StepSpecialty v-if="step === 1" @next="nextStep" @prev="prevStep" />
  <StepDoctor v-if="step === 2" @next="nextStep" @prev="prevStep" />
  <StepDate v-if="step === 3" @next="nextStep" @prev="prevStep" />
  <StepTime v-if="step === 4" @next="nextStep" @prev="prevStep" />
  <div class="mt-4">
    <button v-if="step > 1" @click="prevStep" class="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded">
      Anterior
    </button>
    <button v-if="step === 4" :disabled="!time" @click="nextStep" class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded ml-2 disabled:opacity-50 disabled:cursor-not-allowed">
      Confirmar Cita
    </button>
  </div>
</template>
