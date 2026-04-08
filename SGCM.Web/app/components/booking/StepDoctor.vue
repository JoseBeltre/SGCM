<script lang="ts" setup>
import { UserX, Check, MapPin } from 'lucide-vue-next'
import type { Doctor } from '~/models/doctor.model'

const emit = defineEmits(['next', 'prev'])
const { doctor, specialty } = useBooking()
const { getDoctorsBySpecialtyId, doctors, loading } = useDoctor()

const selectDoctor = (doc: Doctor) => {
  doctor.value = doc
  emit('next')
}

onMounted(async () => {
  if (specialty.value !== null) {
    await getDoctorsBySpecialtyId(specialty.value.id)
  }
})
</script>
<template>
  <div class="animate-in fade-in slide-in-from-right-4 duration-500">
    <div class="mb-6">
      <h2 class="title-primary">Médico Especialista</h2>
      <p class="text-muted">Escoge al profesional de <span class="highlight-primary">{{ specialty?.name || 'la especialidad' }}</span>.</p>
    </div>

    <!-- Loading Skeleton -->
    <div v-if="loading" class="grid grid-cols-1 sm:grid-cols-2 gap-4">
      <div v-for="i in 4" :key="i" class="h-28 rounded-2xl bg-charcoal-brown-100 animate-pulse border border-charcoal-brown-200"></div>
    </div>

    <!-- Sin especialidades -->
    <div v-else-if="!loading && doctors.length === 0" class="py-12 flex flex-col items-center justify-center text-center">
      <div class="w-16 h-16 bg-charcoal-brown-50 rounded-full flex items-center justify-center mb-4">
        <UserX class="w-8 h-8 text-charcoal-brown-300" />
      </div>
      <h3 class="text-lg font-semibold text-charcoal-brown-800">Sin especialistas</h3>
      <p class="text-sm text-charcoal-brown-500 mt-1 max-w-sm">No hay doctores disponibles para esta especialidad en este momento. Intenta con otra.</p>
    </div>

    <!-- Lista -->
    <div v-else class="grid grid-cols-1 sm:grid-cols-2 gap-4">
      <button
        v-for="doc in doctors"
        :key="doc.id"
        @click="selectDoctor(doc)"
        :class="[
          doctor?.id === doc.id ? 'border-sky-reflection-500 bg-sky-reflection-50 ring-2 ring-sky-reflection-200' : 'border-charcoal-brown-200 hover:border-sky-reflection-300 hover:bg-sky-reflection-50/50',
          'flex items-center text-left p-4 rounded-2xl border transition-all duration-200 group focus:outline-none'
        ]"
      >
        <div class="relative">
          <div :class="[doctor?.id === doc.id ? 'bg-sky-reflection-200 text-sky-reflection-700' : 'bg-charcoal-brown-100 text-charcoal-brown-600 group-hover:bg-sky-reflection-100', 'w-12 h-12 rounded-full flex items-center justify-center text-lg font-bold transition-colors']">
            {{ doc.fullName.charAt(0) }}
          </div>
          <div v-if="doctor?.id === doc.id" class="absolute -bottom-1 -right-1 w-5 h-5 bg-palm-leaf-500 rounded-full border-2 border-white flex items-center justify-center">
            <Check class="w-3 h-3 text-white" />
          </div>
        </div>
        <div class="ml-4 flex-1 truncate">
          <h4 :class="[doctor?.id === doc.id ? 'text-sky-reflection-900 font-bold' : 'text-charcoal-brown-900 font-semibold', 'text-[1.05rem] truncate']">
            Dr/a. {{ doc.fullName }}
          </h4>
          <p class="text-xs text-charcoal-brown-500 flex items-center gap-1 mt-0.5 truncate">
            <MapPin class="w-3 h-3" /> {{ doc.assignedOffice || 'Consultorio General' }}
          </p>
        </div>
      </button>
    </div>
  </div>
</template>
