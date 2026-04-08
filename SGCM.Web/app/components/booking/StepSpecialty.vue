<template>
  <div>
    <h2>Selecciona la especialidad</h2>
    <div class="flex flex-col *:border-b *:p-2 hover:*:bg-neutral-100">
      <button v-for="spec in specialties" :key="spec.id" @click="selectSpecialty(spec)">
        {{ spec.name }}
        <br>
        <small class="text-neutral-400">{{ spec.description }}</small>
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Specialty } from '~/models/specialty.model'

const emit = defineEmits(["next", "prev"])
const { specialty } = useBooking()

const { getSpecialties, specialties, loading, error } = useSpecialty()

const selectSpecialty = (spec: Specialty) => {
  specialty.value = spec
  console.log("Selected specialty:", specialty.value)
  if (specialty.value) {
    console.log("Emitting next with specialty:", specialty.value)
    emit("next")
  } else {
    console.warn("No specialty selected, cannot emit next")
  }
}

onMounted(async () => {
  await getSpecialties()
  console.table(specialties.value)
})
</script>
