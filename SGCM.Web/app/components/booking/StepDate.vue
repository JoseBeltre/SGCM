<template>
  <div class="animate-in fade-in slide-in-from-right-4 duration-500">
    <div class="mb-6">
      <h2 class="title-primary">Día de la cita</h2>
      <p class="text-muted">Selecciona un día dentro de la disponibilidad del Dr/a. <span class="highlight-primary">{{ doctor?.fullName || 'la especialidad' }}</span>.</p>
    </div>

    <div class="flex justify-center max-w-sm mx-auto">
      <ClientOnly>
        <VueDatePicker
          v-model="internalDate"
          inline
          auto-apply
          :enable-time-picker="false"
          :min-date="new Date()"
          :disabled-dates="isDateDisabled"
          @update:model-value="onDateSelected"
        />
        <template #fallback>
          <div class="text-gray-500">Cargando calendario...</div>
        </template>
      </ClientOnly>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { VueDatePicker } from '@vuepic/vue-datepicker'
import '@vuepic/vue-datepicker/dist/main.css'

const emit = defineEmits(['next', 'prev'])
const { doctor, date, time } = useBooking()
const { getDoctorAvailability, availability } = useDoctor()

if (!doctor.value) {
  emit('prev')
}

// 0 es Domingo en JS y en VueDatePicker
const dayMap: Record<string, number> = {
  'Domingo': 0,
  'Lunes': 1,
  'Martes': 2,
  'Miércoles': 3,
  'Jueves': 4,
  'Viernes': 5,
  'Sábado': 6
}

const allowedWeekdays = ref<number[]>([])
const internalDate = ref<Date | null>(date.value ? new Date(`${date.value}T12:00:00`) : null)

const isDateDisabled = (dateToValidate: Date): boolean => {
  // Si la API no ha cargado, o no hay disponibilidad, bloqueamos todo
  if (allowedWeekdays.value.length === 0) return true;
  return !allowedWeekdays.value.includes(dateToValidate.getDay());
}

const onDateSelected = (newDate: Date) => {
  if (newDate) {
    // Mantenemos la zona horaria local retornando YYYY-MM-DD
    const isoDate = new Date(newDate.getTime() - newDate.getTimezoneOffset() * 60000).toISOString().split('T')[0] as string
    date.value = isoDate
    // Reiniciamos la hora cuando el día cambia
    time.value = null
    console.log('Selected date:', isoDate)

    emit('next')
  }
}

onMounted(async () => {
  if (doctor.value) {
    try {
      await getDoctorAvailability(doctor.value.id)

      if (!availability.value || !Array.isArray(availability.value)) {
        console.warn('La API no retornó un arreglo válido de disponibilidad')
        return
      }

      const activeDays = availability.value
        .filter(slot => slot.isActive)
        .map(slot => slot.dayOfWeek)

      // Enteros de JS que representan los días permitidos
      allowedWeekdays.value = [...new Set(activeDays)]
        .map(d => typeof d === 'number' ? d : dayMap[d as string])
        .filter((d): d is number => d !== undefined)

    } catch (e) {
      console.error("Error obteniendo la disponibilidad del doctor:", e)
    }
  } else {
    console.warn('No se ha seleccionado ningún doctor en StepDate')
  }
})
</script>
