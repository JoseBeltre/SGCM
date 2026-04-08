<template>
  <div>
    <span v-if="doctor">Doctor seleccionado: {{ doctor.fullName }}</span>
    <span v-else>No se ha seleccionado un doctor</span>
    <h2 class="text-xl font-semibold mt-4 mb-2">Horario del Doctor</h2>
    <h3 class="mb-4">Selecciona un día</h3>
    
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
import {VueDatePicker} from '@vuepic/vue-datepicker'
import '@vuepic/vue-datepicker/dist/main.css'

const emit = defineEmits(['next', 'prev'])
const { doctor, date, time } = useBooking()
const { getDoctorAvailability, availability } = useDoctor()

if (!doctor.value) {
  emit('prev')
}

// 0 is Sunday in JS and VueDatePicker
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
  // If API hasn't loaded yet, or returned no availability, block everything
  if (allowedWeekdays.value.length === 0) return true;
  return !allowedWeekdays.value.includes(dateToValidate.getDay());
}

const onDateSelected = (newDate: Date) => {
  if (newDate) {
    // Keep local timezone YYYY-MM-DD
    const isoDate = new Date(newDate.getTime() - newDate.getTimezoneOffset() * 60000).toISOString().split('T')[0] as string
    date.value = isoDate
    // Reset time when day changes
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
        console.warn('API did not return a valid array of availability')
        return
      }

      const activeDays = availability.value
        .filter(slot => slot.isActive)
        .map(slot => slot.dayOfWeek)
      
      // JS integers representing the allowed days
      allowedWeekdays.value = [...new Set(activeDays)]
        .map(d => typeof d === 'number' ? d : dayMap[d as string])
        .filter((d): d is number => d !== undefined)
        
    } catch (e) {
      console.error("Error fetching doctor availability:", e)
    }
  } else {
    console.warn('No doctor selected in StepDate')
  }
})
</script>
