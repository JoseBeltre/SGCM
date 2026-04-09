<script setup lang="ts">
import { LucideClock } from "lucide-vue-next"
import type { Appointment } from '~/models/appointment.model'
import type { Doctor } from '~/models/doctor.model'
import { getStatusClass, formatDate, formatTime, getDoctorName, canModifyAppointment } from '~/utils/appointment.utils'
import AppointmentActionButton from './AppointmentActionButton.vue'

defineProps<{
  appointment: Appointment
  doctorCache: Record<number, Doctor>
}>()

defineEmits(['confirm', 'cancel'])
</script>
<template>
  <div class="flex flex-col border border-charcoal-brown-100 rounded-2xl p-6 hover:shadow-md transition-shadow relative">
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
      <p class="text-sm text-charcoal-brown-500">Doctor/a</p>
      <p class="font-semibold text-charcoal-brown-900">
        {{ getDoctorName(appointment.doctorId, doctorCache) }}
      </p>
    </div>

    <div class="space-y-2 border-t border-charcoal-brown-50 pt-4 mt-auto"
      v-if="appointment.status === 'Solicitada' || appointment.status === 'Confirmada'">
      <AppointmentActionButton v-if="appointment.status === 'Solicitada'" type="confirm" @action="$emit('confirm', appointment.id)" />
      
      <AppointmentActionButton v-if="canModifyAppointment(appointment.appointmentDate)" type="cancel" @action="$emit('cancel', appointment)" />
    </div>
  </div>
</template>
