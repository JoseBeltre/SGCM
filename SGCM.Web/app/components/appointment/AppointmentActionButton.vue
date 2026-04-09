<script setup lang="ts">
defineProps<{
  type: 'confirm' | 'cancel'
}>()

defineEmits(['action'])

const getButtonClass = (type: string) => {
  return type === 'confirm' ? 'btn-appointment-confirm' : 'btn-appointment-cancel'
}

const getButtonText = (type: string) => {
  return type === 'confirm' ? 'Confirmar Asistencia' : 'Cancelar Cita'
}
</script>

<template>
  <button :class="getButtonClass(type)" @click="$emit('action')">
    <template v-if="type === 'confirm'">
      Confirmar <br> Asistencia
    </template>
    <template v-else>
      {{ getButtonText(type) }}
    </template>
  </button>
</template>
