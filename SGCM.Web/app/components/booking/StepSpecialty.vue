<script setup lang="ts">
import { Stethoscope, Activity } from 'lucide-vue-next'
import type { Specialty } from '~/models/specialty.model'

const emit = defineEmits(["next", "prev"])
const { specialty } = useBooking()
const { getSpecialties, specialties, loading, error } = useSpecialty()

const selectSpecialty = (spec: Specialty) => {
  specialty.value = spec
  emit("next")
}

onMounted(async () => {
  await getSpecialties()
})
</script>
<template>
  <div class="animate-in fade-in slide-in-from-bottom-4 duration-500">
    <div class="mb-6">
      <h2 class="title-primary">Especialidad</h2>
      <p class="text-muted">¿Qué tipo de atención médica necesitas hoy?</p>
    </div>

    <!-- Loading Skeleton -->
    <div v-if="loading" class="grid grid-cols-1 sm:grid-cols-2 gap-4">
      <div v-for="i in 4" :key="i" class="h-24 rounded-2xl bg-charcoal-brown-100 animate-pulse border border-charcoal-brown-200"></div>
    </div>

    <!-- Sin especialidades -->
    <div v-else-if="!loading && specialties.length === 0" class="py-12 flex flex-col items-center justify-center text-center">
      <div class="w-16 h-16 bg-charcoal-brown-50 rounded-full flex items-center justify-center mb-4">
        <Stethoscope class="w-8 h-8 text-charcoal-brown-300" />
      </div>
      <h3 class="text-lg font-semibold text-charcoal-brown-800">No hay especialidades</h3>
      <p class="text-sm text-charcoal-brown-500 mt-1">Actualmente no hay especialidades disponibles.</p>
    </div>

    <!-- Lista -->
    <div v-else class="grid grid-cols-1 sm:grid-cols-2 gap-4">
      <button
        v-for="spec in specialties"
        :key="spec.id"
        @click="selectSpecialty(spec)"
        :class="[
          specialty?.id === spec.id ? 'border-sky-reflection-500 bg-sky-reflection-50 ring-2 ring-sky-reflection-200' : 'border-charcoal-brown-200 hover:border-sky-reflection-300 hover:bg-sky-reflection-50/50',
          'flex flex-col text-left p-5 rounded-2xl border transition-all duration-200 group focus:outline-none'
        ]"
      >
        <div class="flex items-center gap-3 mb-2">
          <div :class="[specialty?.id === spec.id ? 'bg-sky-reflection-500 text-white' : 'bg-charcoal-brown-100 text-charcoal-brown-600 group-hover:bg-sky-reflection-100 group-hover:text-sky-reflection-600', 'w-10 h-10 rounded-xl flex items-center justify-center transition-colors']">
            <Activity class="w-5 h-5" />
          </div>
          <span :class="[specialty?.id === spec.id ? 'text-sky-reflection-900 font-bold' : 'text-charcoal-brown-800 font-semibold', 'text-[1.05rem]']">{{ spec.name }}</span>
        </div>
        <p class="text-xs text-charcoal-brown-500 leading-relaxed truncate w-full" :title="spec.description || 'Sin descripción'">{{ spec.description || 'Consulta general de la especialidad.' }}</p>
      </button>
    </div>
  </div>
</template>
