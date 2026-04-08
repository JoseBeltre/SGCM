<script setup lang="ts">
import { LucideClock } from "lucide-vue-next"
import type { Appointment } from '~/models/appointment.model'
import type { Patient } from '~/models/patient.model'
import { getStatusClass, formatDate, formatTime, getPatientName } from '~/utils/appointment.utils'

defineProps<{
  appointment: Appointment
  patientCache: Record<number, Patient>
}>()
</script>
<template>
  <div
    class="border border-charcoal-brown-100 bg-white rounded-2xl p-4 md:p-6 hover:shadow-md transition-shadow relative flex flex-col md:flex-row md:items-center justify-between gap-4">

    <!-- Info Section -->
    <div class="flex flex-col md:flex-row md:items-center gap-4 md:gap-8 flex-1">

      <!-- Date & Time -->
      <div class="min-w-[160px]">
        <div class="flex items-center mb-1">
          <span :class="getStatusClass(appointment.status)"
            class="text-[10px] sm:text-xs font-bold px-2.5 py-0.5 rounded-full uppercase tracking-wide">
            {{ appointment.status }}
          </span>
        </div>
        <p class="text-xl sm:text-2xl font-extrabold text-charcoal-brown-900">
          {{ formatDate(appointment.appointmentDate) }}
        </p>
        <p class="text-sm font-medium text-charcoal-brown-500 flex items-center mt-1">
          <LucideClock class="w-4 h-4 mr-1 text-charcoal-brown-400" />
          {{ formatTime(appointment.appointmentDate) }}
        </p>
      </div>

      <!-- Divider (hidden on small screens) -->
      <div class="hidden md:block w-px h-16 bg-charcoal-brown-100"></div>

      <!-- Patient Info -->
      <div class="flex-1">
        <p class="text-xs text-charcoal-brown-400 uppercase tracking-widest font-semibold mb-1">Paciente</p>
        <p class="font-bold text-charcoal-brown-900 text-lg sm:text-xl">
          {{ getPatientName(appointment.patientId, patientCache) }}
        </p>
      </div>
    </div>
  </div>
</template>
