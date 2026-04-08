<template>
  <div class="min-h-screen bg-charcoal-brown-50 md:pt-5 sm:px-6 lg:px-8">
    <div class="max-w-7xl mx-auto space-y-8">
      <!-- Encabezado de Bienvenida -->
      <section
        class="bg-white rounded-3xl shadow-sm border border-charcoal-brown-100 p-6 sm:p-8 flex flex-col justify-between gap-6">
        <div>
          <h1 class="text-2xl md:text-3xl font-extrabold text-charcoal-brown-900 tracking-tight">
            Hola,
            <span class="text-sky-reflection-600">{{ authStore.user?.fullName || "Paciente" }}</span> 👋
          </h1>
          <p class="mt-2 leading-5 md:text-lg text-charcoal-brown-500">
            Bienvenido a tu portal de salud. ¿Qué necesitas hacer hoy?
          </p>
        </div>
        <div class="flex-1">
          <button @click="navigateTo('/booking')"
            class="inline-flex items-center justify-center w-full md:w-auto px-6 py-4 border border-transparent rounded-lg text-base font-bold text-white bg-sky-reflection-500 hover:bg-sky-reflection-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-sky-reflection-500 transition-all transform hover:-translate-y-1">
            <LucideCalendar class="h-6 w-6 mr-2" />
            <span class="hidden md:block text-nowrap">Agendar Cita</span>
          </button>
        </div>
      </section>

      <!-- Panel Principal de Citas -->
      <section class="bg-white rounded-3xl shadow-sm border border-charcoal-brown-100 overflow-hidden">
        <div
          class="border-b border-charcoal-brown-100 bg-charcoal-brown-50/50 px-8 py-6 flex justify-between items-center">
          <h2 class="text-xl font-bold text-charcoal-brown-900">
            Próximas Citas Médicas
          </h2>
        </div>

        <div class="p-8">
          <!-- Estado de carga -->
          <div v-if="loading" class="flex justify-center py-10">
            <LucideClock class="h-10 w-10 text-charcoal-brown-300 animate-spin" />
          </div>
          <!-- Estado vacío -->
          <div v-else-if="appointments.length === 0" class="text-center py-16">
            <div class="mx-auto w-24 h-24 bg-charcoal-brown-50 rounded-full flex items-center justify-center mb-4">
              <LucideClock class="h-10 w-10 text-charcoal-brown-300" />
            </div>
            <h3 class="text-lg font-bold text-charcoal-brown-900 mb-2">No tienes citas programadas</h3>
            <p class="text-charcoal-brown-500 max-w-sm mx-auto">
              Aquí aparecerán todas tus consultas médicas. Para comenzar, agenda una cita con uno de nuestros
              especialistas.
            </p>
            <div class="mt-8">
              <button @click="navigateTo('/booking')"
                class="text-sky-reflection-600 font-semibold hover:text-sky-reflection-700 transition-colors">
                Ver doctores disponibles →
              </button>
            </div>
          </div>

          <!-- Listado de Citas -->
          <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div v-for="apt in sortedAppointments" :key="apt.id"
              class="border border-charcoal-brown-100 rounded-2xl p-6 hover:shadow-md transition-shadow relative">
              <div class="flex justify-between items-start mb-4">
                <span :class="getStatusClass(apt.status)"
                  class="text-xs font-bold px-3 py-1 rounded-full uppercase tracking-wide">
                  {{ apt.status }}
                </span>
              </div>

              <div class="mb-4">
                <p class="text-2xl font-extrabold text-charcoal-brown-900">
                  {{ formatDate(apt.appointmentDate) }}
                </p>
                <p class="text-sm font-medium text-charcoal-brown-500 flex items-center mt-1">
                  <LucideClock class="w-4 h-4 mr-1" />
                  {{ formatTime(apt.appointmentDate) }}
                </p>
              </div>

              <div class="mb-6">
                <p class="text-sm text-charcoal-brown-500">Doctor</p>
                <p class="font-semibold text-charcoal-brown-900">
                  {{ getDoctorName(apt.doctorId, doctorCache) }}
                </p>
              </div>

              <!-- Acciones -->
              <div class="space-y-2 border-t border-charcoal-brown-50 pt-4"
                v-if="apt.status === 'Pendiente' || apt.status === 'Confirmada'">
                <button v-if="apt.status === 'Pendiente'" @click="confirmAppointmentAction(apt.id)"
                  class="w-full text-center py-2 bg-palm-leaf-100 text-palm-leaf-800 hover:bg-palm-leaf-200 rounded-xl font-bold transition-colors text-sm">
                  Confirmar Asistencia
                </button>
                <button @click="initiateCancel(apt)"
                  class="w-full text-center py-2 bg-white border border-red-200 text-red-600 hover:bg-red-50 rounded-xl font-bold transition-colors text-sm">
                  Cancelar Cita
                </button>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Modals Dinámicos -->
      <DynamicModal :isOpen="modalState.isOpen" :variant="modalState.variant" :title="modalState.title"
        :description="modalState.description" :showConfirm="modalState.showConfirm" :showCancel="modalState.showCancel"
        :confirmText="modalState.confirmText" @confirm="handleModalConfirm" @close="modalState.isOpen = false">
        <!-- Slot inyectado para la razón de cancelación -->
        <template #content v-if="modalState.type === 'cancel-reason'">
          <div class="mt-4">
            <label class="block text-sm font-medium text-charcoal-brown-700 mb-2">Motivo de cancelación</label>
            <textarea v-model="cancelReason" rows="3"
              class="w-full px-3 py-2 border border-charcoal-brown-200 rounded-xl shadow-sm focus:outline-none focus:ring-sky-reflection-500 focus:border-sky-reflection-500 sm:text-sm resize-none"
              placeholder="Indica brevemente por qué necesitas cancelar..."></textarea>
          </div>
        </template>
      </DynamicModal>

    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { navigateTo } from "#app"
import { LucideCalendar, LucideClock } from "lucide-vue-next"
import DynamicModal from '~/components/ui/DynamicModal.vue'
import { useAppointementService } from '~/services/appointment.service'
import { useDoctorService } from '~/services/doctor.service'
import type { Appointment } from '~/models/appointment.model'
import type { Doctor } from '~/models/doctor.model'
import { getStatusClass, formatDate, formatTime, getDoctorName } from '~/utils/appointment.utils'

const authStore = useAuthStore()
const appointmentService = useAppointementService()
const doctorService = useDoctorService()

const appointments = ref<Appointment[]>([])
const doctorCache = ref<Record<number, Doctor>>({})
const loading = ref(true)

// Estado del Modal Dinámico
const modalState = ref({
  isOpen: false,
  variant: 'info' as any,
  title: '',
  description: '',
  type: '', // 'cancel-reason', 'error', 'success', 'confirm'
  showConfirm: true,
  showCancel: true,
  confirmText: 'Aceptar',
  targetAppointmentId: null as number | null
})
const cancelReason = ref('')

const sortedAppointments = computed(() => {
  return [...appointments.value].sort((a, b) =>
    new Date(a.appointmentDate).getTime() - new Date(b.appointmentDate).getTime()
  )
})

const loadData = async () => {
  if (!authStore.user?.patientId) {
    loading.value = false
    return
  }
  loading.value = true
  try {
    const data = await appointmentService.getAppointmentsByPatientId(authStore.user.patientId)
    appointments.value = data

    // Extraer IDs únicos de doctores
    const doctorIds = [...new Set(data.map(apt => apt.doctorId))]

    // Cargar info de los doctores que faltan en caché
    for (const docId of doctorIds) {
      if (!doctorCache.value[docId]) {
        doctorCache.value[docId] = await doctorService.getDoctorById(docId)
      }
    }
  } catch (error) {
    console.error("Error loading panel data", error)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadData()
})

// --- LÓGICA DE CONFIRMACIÓN ---
const confirmAppointmentAction = async (id: number) => {
  try {
    await appointmentService.confirmAppointment(id)
    await loadData()
    showModal('success', '¡Cita Confirmada!', 'Nos vemos pronto. Gracias por confirmar su asistencia.', 'Aceptar')
  } catch (error) {
    showModal('error', 'Error', 'Ocurrió un problema intentando confirmar la cita.')
  }
}

// --- LÓGICA DE CANCELACIÓN (48H RULES) ---
const initiateCancel = (apt: Appointment) => {
  const diffHours = (new Date(apt.appointmentDate).getTime() - new Date().getTime()) / (1000 * 60 * 60)

  if (diffHours < 48) {
    // Regla de Negocio: Restricción de 48 horas
    showModal('warning', 'No se puede cancelar', 'No puedes cancelar una cita faltando 48 horas o menos para su realización. Por favor contacta al centro de atención si tienes una emergencia.', 'Lo Entiendo', false)
  } else {
    // Regla de Negocio: Permitir solicitando el motivo
    cancelReason.value = ''
    modalState.value = {
      isOpen: true,
      variant: 'error',
      title: 'Cancelar Cita Médica',
      description: '¿Estás seguro de que deseas cancelar esta cita? Esta acción no se puede deshacer.',
      type: 'cancel-reason',
      showConfirm: true,
      showCancel: true,
      confirmText: 'Sí, Cancelar Definitivamente',
      targetAppointmentId: apt.id
    }
  }
}

const handleModalConfirm = async () => {
  if (modalState.value.type === 'cancel-reason' && modalState.value.targetAppointmentId) {
    if (!cancelReason.value.trim()) {
      alert("Por favor ingresa un motivo para poder cancelar.")
      modalState.value.isOpen = true // Prevent closing visually
      return
    }

    try {
      await appointmentService.cancelAppointment(modalState.value.targetAppointmentId, cancelReason.value)
      await loadData()
      showModal('success', 'Cita Cancelada', 'La cita fue cancelada exitosamente.', 'Aceptar')
    } catch (e) {
      showModal('error', 'Error', 'Hubo un problema cancelando la cita.')
    }
  }
}

const showModal = (variant: any, title: string, description: string, confirmText = 'Aceptar', showCancel = false) => {
  modalState.value = {
    isOpen: true,
    variant,
    title,
    description,
    type: 'generic',
    showConfirm: true,
    showCancel,
    confirmText,
    targetAppointmentId: null
  }
}
</script>
