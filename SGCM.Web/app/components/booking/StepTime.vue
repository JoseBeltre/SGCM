<template>
  <div class="animate-in fade-in slide-in-from-right-4 duration-500">
    <div class="mb-6">
      <h2 class="title-primary">Horarios Disponibles</h2>
      <p class="text-muted">
        Elige un bloque de 45 minutos para tu cita.
      </p>
    </div>

    <!-- UI Block: Check Date -->
    <div v-if="!date" class="py-12 flex flex-col items-center justify-center text-center">
      <div class="w-16 h-16 bg-charcoal-brown-50 rounded-full flex items-center justify-center mb-4">
        <CalendarX class="w-8 h-8 text-charcoal-brown-300" />
      </div>
      <h3 class="title-secondary">Fecha pendiente</h3>
      <p class="text-sm text-charcoal-brown-500 mt-1">Por favor regresa al paso anterior y selecciona una fecha válida.</p>
    </div>

    <div v-else>
      <div class="mb-4">
        <span class="highlight-secondary">
          Día seleccionado: {{ date }}
        </span>
      </div>

      <!-- Loading UI -->
      <div v-if="loadingTimeUI" class="grid grid-cols-2 sm:grid-cols-3 gap-3">
        <div v-for="i in 6" :key="i" class="h-12 bg-charcoal-brown-100 rounded-full animate-pulse border border-charcoal-brown-200"></div>
      </div>

      <!-- Content -->
      <div v-else>
        <div v-if="availableTimeSlots.length === 0" class="py-12 flex flex-col items-center justify-center text-center bg-charcoal-brown-50 rounded-2xl border border-charcoal-brown-100">
          <div class="w-16 h-16 bg-white rounded-full flex items-center justify-center mb-4 shadow-sm">
            <Clock class="w-8 h-8 text-charcoal-brown-400" />
          </div>
          <h3 class="title-secondary">Sin horarios disponibles</h3>
          <p class="text-sm text-charcoal-brown-500 mt-1 max-w-xs">No encontramos franjas horarias libres para este día, intenta seleccionar otra fecha.</p>
        </div>

        <div v-else class="grid grid-cols-2 sm:grid-cols-3 gap-3 max-h-[300px] overflow-y-auto pr-2 pb-4 scrollbar-thin scrollbar-thumb-charcoal-brown-200">
          <button
            v-for="(slot, idx) in availableTimeSlots"
            :key="idx"
            @click="selectTime(slot)"
            :class="[
              time === slot ? 'bg-honey-bronze-500 text-white font-bold shadow-md ring-2 ring-honey-bronze-200' : 'bg-white text-charcoal-brown-700 border border-charcoal-brown-200 hover:border-honey-bronze-300 hover:bg-honey-bronze-50 font-medium',
              'py-3 px-4 rounded-full transition-all duration-200 focus:outline-none flex items-center justify-center'
            ]"
          >
            {{ slot }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { CalendarX, Clock } from 'lucide-vue-next'
import { generateFilteredTimeSlots } from '~/utils/timeSlots'

const emit = defineEmits(['next', 'prev'])
const { doctor, date, time } = useBooking()
const { getAppointmentsByDateAndDoctor, appointments } = useAppointment()
const { availability, getDoctorAvailability } = useDoctor()

const loadingTimeUI = ref(true)
const availableTimeSlots = ref<string[]>([])

onMounted(async () => {
  if (!doctor.value || !date.value) {
    emit('prev')
    return
  }

  loadingTimeUI.value = true
  
  try {
    await getDoctorAvailability(doctor.value.id)
    await getAppointmentsByDateAndDoctor(date.value, doctor.value.id)
    
    // Leverage abstracted generator
    availableTimeSlots.value = generateFilteredTimeSlots(
      date.value, 
      availability.value, 
      appointments.value, 
      45 // 45 minutes fixed duration
    )
  } catch (error) {
    console.error("Error loading time slots", error)
  } finally {
    loadingTimeUI.value = false
  }
})

const selectTime = (selectedSlot: string) => {
  time.value = selectedSlot
  console.log('Selected time:', selectedSlot)
}
</script>
