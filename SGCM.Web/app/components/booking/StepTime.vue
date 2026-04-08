<template>
  <div>
    <h2 class="text-xl font-semibold mb-2">Horas Disponibles</h2>
    <div class="mb-4">
      <span class="text-sm text-gray-500 block">Día seleccionado: {{ date }}</span>
    </div>
    
    <div v-if="loadingTimeUI" class="text-gray-500">
      Cargando horarios...
    </div>
    <div v-else>
      <div v-if="availableTimeSlots.length === 0" class="text-gray-500">
        No hay horarios disponibles para este día
      </div>
      <div v-else class="grid grid-cols-3 gap-2">
        <button 
          v-for="timeSlot in availableTimeSlots" 
          :key="timeSlot" 
          class="border rounded p-2 text-center hover:bg-blue-50 focus:ring focus:ring-blue-200 transition" 
          :class="{ 'bg-blue-100 border-blue-500': time === timeSlot }"
          @click="selectTime(timeSlot)"
        >
          {{ timeSlot }}
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { DayOfWeek } from '~/models/availability.model'

const emit = defineEmits(['next', 'prev'])
const { doctor, date, time } = useBooking()
const { getAppointmentsByDateAndDoctor, appointments } = useAppointment()
const { availability, getDoctorAvailability } = useDoctor()

const loadingTimeUI = ref(true)

const jsDayToEnum = [
  'Domingo', 
  'Lunes', 
  'Martes', 
  'Miércoles', 
  'Jueves', 
  'Viernes', 
  'Sábado'
]

const parseTimeToMinutes = (value: string) => {
  const normalizedValue = value.trim()
  const isoDate = new Date(normalizedValue)

  if (!Number.isNaN(isoDate.getTime()) && normalizedValue.includes('T')) {
    return isoDate.getHours() * 60 + isoDate.getMinutes()
  }

  const match = normalizedValue.match(/^(\d{1,2}):(\d{2})(?::\d{2})?\s*(AM|PM)?$/i)

  if (!match) {
    return 0
  }

  let hours = Number(match[1])
  const minutes = Number(match[2])
  const meridiem = match[3]?.toUpperCase()

  if (meridiem === 'PM' && hours < 12) {
    hours += 12
  }

  if (meridiem === 'AM' && hours === 12) {
    hours = 0
  }

  return hours * 60 + minutes
}

const formatMinutesToTime = (minutes: number) => {
  const hours24 = Math.floor(minutes / 60) % 24
  const mins = minutes % 60
  const period = hours24 >= 12 ? 'PM' : 'AM'
  const hours12 = hours24 % 12 || 12

  return `${hours12}:${mins.toString().padStart(2, '0')} ${period}`
}

// Convert YYYY-MM-DD to DayOfWeek enum string
const getDayOfWeekName = (dateStr: string) => {
  // Ensure we append time to avoid timezone shifting
  const d = new Date(`${dateStr}T12:00:00`)
  return jsDayToEnum[d.getDay()]
}

// Helper to check if a specific timeblock overlaps an existing appointment
const isOverlapping = (slotStartMin: number, slotEndMin: number) => {
  if (!appointments.value) return false

  return appointments.value.some(appt => {
    const apptStartMin = parseTimeToMinutes(appt.appointmentDate)
    const apptEndMin = apptStartMin + appt.durationMinutes

    // Check overlap: slot starts before appt ends AND slot ends after appt starts
    return slotStartMin < apptEndMin && slotEndMin > apptStartMin
  })
}

const getFilteredTimeSlots = () => {
  if (!date.value) return []

  const d = new Date(`${date.value}T12:00:00`)
  const dayNum = d.getDay()
  const dayEnum = jsDayToEnum[dayNum]
  const duration = 45 // 45 minutes fixed duration

  // Gather all shift limits for the selected day
  const slotsConfig = availability.value
    .filter(slot => (slot.dayOfWeek === dayEnum || (slot.dayOfWeek as unknown as number) === dayNum) && slot.isActive)
  
  const generatedSlots: string[] = []

  for (const config of slotsConfig) {
    const startMinutes = parseTimeToMinutes(config.startTime)
    const endMinutes = parseTimeToMinutes(config.endTime)

    for (let currentMinutes = startMinutes; (currentMinutes + duration) <= endMinutes; currentMinutes += duration) {
      // Ensure we don't present slots that are already booked
      if (!isOverlapping(currentMinutes, currentMinutes + duration)) {
        generatedSlots.push(formatMinutesToTime(currentMinutes))
      }
    }
  }

  return [...new Set(generatedSlots)].sort((left, right) => parseTimeToMinutes(left) - parseTimeToMinutes(right))
}

const availableTimeSlots = ref<string[]>([])

onMounted(async () => {
  if (!doctor.value || !date.value) {
    emit('prev')
    return
  }

  loadingTimeUI.value = true
  // Fetch everything needed since composables use local refs instead of useState
  await getDoctorAvailability(doctor.value.id)
  await getAppointmentsByDateAndDoctor(date.value, doctor.value.id)
  
  availableTimeSlots.value = getFilteredTimeSlots()
  loadingTimeUI.value = false
})

const selectTime = (selectedSlot: string) => {
  time.value = selectedSlot
  console.log('Selected time:', selectedSlot)
  // Optional auto-advance
  // emit('next')
}
</script>
