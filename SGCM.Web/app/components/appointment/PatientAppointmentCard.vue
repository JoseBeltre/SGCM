<script setup lang="ts">
import { LucideClock } from "lucide-vue-next"
import type { Appointment } from '~/models/appointment.model'
import type { Doctor } from '~/models/doctor.model'
import { getStatusClass, formatDate, formatTime, getDoctorName } from '~/utils/appointment.utils'

defineProps<{
  appointment: Appointment
  doctorCache: Record<number, Doctor>
}>()

defineEmits(['confirm', 'cancel'])
</script>
<template>
  <div class="border border-charcoal-brown-100 rounded-2xl p-6 hover:shadow-md transition-shadow relative">
    <div class="flex justify-between items-start mb-4">
      <span :class="getStatusClass(appointment.status)"
        class="text-xs font-bold px-3 py-1 rounded-full uppercase tracking-wide">
        {{ appointment.status }}
      </span>
    </div>

    <div class="mb-4">
      <p class="text-2xl font-extrabold text-charcoal-brown-900">
        {{ formatDate(appointment.appointmentDate) }}
      </p>
      <p class="text-sm font-medium text-charcoal-brown-500 flex items-center mt-1">
        <LucideClock class="w-4 h-4 mr-1" />
        {{ formatTime(appointment.appointmentDate) }}
      </p>
    </div>

    <div class="mb-6">
      <p class="text-sm text-charcoal-brown-500">Doctor</p>
      <p class="font-semibold text-charcoal-brown-900">
        {{ getDoctorName(appointment.doctorId, doctorCache) }}
      </p>
    </div>

    <!-- Acciones -->
    <div class="space-y-2 border-t border-charcoal-brown-50 pt-4"
      v-if="appointment.status === 'Solicitada' || appointment.status === 'Confirmada'">
      <button v-if="appointment.status === 'Solicitada'" @click="$emit('confirm', appointment.id)"
        class="w-full text-center py-2 bg-palm-leaf-100 text-palm-leaf-800 hover:bg-palm-leaf-200 rounded-xl font-bold transition-colors text-sm">
        Confirmar Asistencia
      </button>
      <button @click="$emit('cancel', appointment)"
        class="w-full text-center py-2 bg-white border border-red-200 text-red-600 hover:bg-red-50 rounded-xl font-bold transition-colors text-sm">
        Cancelar Cita
      </button>
    </div>
  </div>
</template>
