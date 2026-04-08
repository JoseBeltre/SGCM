<template>
  <Transition name="modal-fade">
    <div v-if="isOpen" class="fixed inset-0 z-50 overflow-y-auto" aria-labelledby="modal-title" role="dialog" aria-modal="true">
      <!-- Overlay blur y sombreado -->
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div class="fixed inset-0 bg-charcoal-brown-900 bg-opacity-75 transition-opacity backdrop-blur-sm" aria-hidden="true" @click="closeOnClickOutside && close()"></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <!-- Panel del Modal -->
        <div class="inline-block align-bottom bg-white rounded-3xl text-left overflow-hidden shadow-2xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full border border-charcoal-brown-100">
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              
              <!-- Icono dinámico según variante -->
              <div :class="[
                  'mx-auto flex-shrink-0 flex items-center justify-center h-12 w-12 rounded-full sm:mx-0 sm:h-10 sm:w-10 shadow-md',
                  variantStyles.iconBg
                ]"
              >
                <component :is="variantStyles.icon" :class="['h-6 w-6', variantStyles.iconColor]" aria-hidden="true" />
              </div>

              <!-- Contenido Textual -->
              <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left w-full">
                <h3 class="text-xl leading-6 font-extrabold tracking-tight text-charcoal-brown-900" id="modal-title">
                  {{ title }}
                </h3>
                <div class="mt-2">
                  <!-- Slot para HTML o string de description normal -->
                  <p v-if="description" class="text-sm text-charcoal-brown-500">
                    {{ description }}
                  </p>
                  <slot name="content"></slot>
                </div>
              </div>
            </div>
          </div>

          <!-- Acciones/Botones Dinámicos -->
          <div class="bg-charcoal-brown-50/50 px-4 py-4 sm:px-6 sm:flex sm:flex-row-reverse border-t border-charcoal-brown-100">
            <!-- Slot Custom Acciones -->
            <slot name="actions">
              <!-- Default: Botón de OK u Opciones Dinámicas -->
              <button 
                v-if="showConfirm" 
                @click="onConfirm"
                type="button" 
                :class="[
                  'w-full inline-flex justify-center rounded-xl border border-transparent shadow-sm px-4 py-2 text-base font-bold text-white focus:outline-none focus:ring-2 focus:ring-offset-2 sm:ml-3 sm:w-auto sm:text-sm transition-all',
                  variantStyles.btnBg
                ]"
              >
                {{ confirmText }}
              </button>
              <button 
                v-if="showCancel" 
                @click="close"
                type="button" 
                class="mt-3 w-full inline-flex justify-center rounded-xl border border-charcoal-brown-200 shadow-sm px-4 py-2 bg-white text-base font-bold text-charcoal-brown-700 hover:bg-charcoal-brown-50 hover:text-charcoal-brown-900 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-charcoal-brown-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm transition-all"
              >
                {{ cancelText }}
              </button>
            </slot>
          </div>
        </div>
      </div>
    </div>
  </Transition>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { Check, AlertTriangle, Info, XCircle } from 'lucide-vue-next'

const props = defineProps({
  isOpen: { type: Boolean, required: true },
  variant: {
    type: String as () => 'success' | 'warning' | 'error' | 'info',
    default: 'info'
  },
  title: { type: String, required: true },
  description: { type: String, default: '' },
  confirmText: { type: String, default: 'Aceptar' },
  cancelText: { type: String, default: 'Cancelar' },
  showConfirm: { type: Boolean, default: true },
  showCancel: { type: Boolean, default: false },
  closeOnClickOutside: { type: Boolean, default: true }
})

const emit = defineEmits(['close', 'confirm'])

const close = () => {
  emit('close')
}

const onConfirm = () => {
  emit('confirm')
  close()
}

// Mapear estilos según variante (Utilizando la paleta temática)
const variantStyles = computed(() => {
  switch (props.variant) {
    case 'success':
      return {
        icon: Check,
        iconBg: 'bg-palm-leaf-100',
        iconColor: 'text-palm-leaf-600',
        btnBg: 'bg-palm-leaf-600 hover:bg-palm-leaf-700 focus:ring-palm-leaf-500 shadow-palm-leaf-200'
      }
    case 'warning':
      return {
        icon: AlertTriangle,
        iconBg: 'bg-honey-bronze-100',
        iconColor: 'text-honey-bronze-600',
        btnBg: 'bg-honey-bronze-500 hover:bg-honey-bronze-600 focus:ring-honey-bronze-500 shadow-honey-bronze-200'
      }
    case 'error':
      return {
        icon: XCircle,
        iconBg: 'bg-red-100',
        iconColor: 'text-red-600',
        btnBg: 'bg-red-600 hover:bg-red-700 focus:ring-red-500 shadow-red-200'
      }
    case 'info':
    default:
      return {
        icon: Info,
        iconBg: 'bg-sky-reflection-100',
        iconColor: 'text-sky-reflection-600',
        btnBg: 'bg-sky-reflection-500 hover:bg-sky-reflection-600 focus:ring-sky-reflection-500 shadow-sky-reflection-200'
      }
  }
})
</script>

<style scoped>
.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.3s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
}
</style>
