<template>
  <div>
    <p v-if="specialty">Especialidad seleccionada: {{ specialty.name }}</p>
    <p v-else>No se ha seleccionado una especialidad</p>
    <h2>Selecciona el doctor</h2>
    <div class="flex flex-col *:border-b *:p-2 hover:*:bg-neutral-100">
      <p v-if="doctors.length === 0">No hay doctores disponibles para esta especialidad</p>
      <button v-for="doc in doctors" :key="doc.id" @click="selectDoctor(doc)">
        {{ doc.fullName }}
        <br>
        <small class="text-neutral-400">{{ doc.assignedOffice }}</small>
      </button>
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { Doctor } from '~/models/doctor.model'

const emit = defineEmits(['next', 'prev'])
const { doctor, specialty } = useBooking()
const { getDoctorsBySpecialtyId, doctors } = useDoctor()


const selectDoctor = (doc: Doctor) => {
  doctor.value = doc
  console.log('Selected doctor:', doctor.value)
  emit('next')
}

onMounted(async () => {
  console.log(specialty.value)
  if (specialty.value !== null) {
    console.log('Selected specialty in StepDoctor:', specialty.value)
    await getDoctorsBySpecialtyId(specialty.value.id)
    console.table(doctors.value)
  } else {
    console.warn('No specialty selected in StepDoctor')
  }
})
</script>
