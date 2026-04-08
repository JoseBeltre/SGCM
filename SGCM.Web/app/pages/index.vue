<template>
  <div class="min-h-screen bg-charcoal-brown-50 md:pt-5 sm:px-6 lg:px-8">
    <div class="max-w-7xl mx-auto space-y-8">
      <!-- Encabezado de Bienvenida -->
      <section
        class="bg-white rounded-3xl shadow-sm border border-charcoal-brown-100 p-6 sm:p-8 flex flex-col justify-between gap-6">
        <div>
          <h1 class="text-2xl md:text-3xl font-extrabold text-charcoal-brown-900 tracking-tight">
            Hola,
            <span class="text-sky-reflection-600">
              {{ authStore.user?.userType === 'Medico' ? 'Dr/a. ' : '' }}
              {{ authStore.user?.fullName || "Usuario" }}
            </span>
            👋
          </h1>
          <p class="mt-2 leading-5 md:text-lg text-charcoal-brown-500">
            Bienvenido a tu portal de salud. ¿Qué necesitas hacer hoy?
          </p>
        </div>
        <div class="flex-1" v-if="authStore.user?.userType === 'Paciente'">
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
          <!-- Estado vacio -->
          <div v-else-if="allAppointments.length === 0" class="text-center py-16">
            <div class="mx-auto w-24 h-24 bg-charcoal-brown-50 rounded-full flex items-center justify-center mb-4">
              <LucideClock class="h-10 w-10 text-charcoal-brown-300" />
            </div>
            <h3 class="text-lg font-bold text-charcoal-brown-900 mb-2">
              No tienes citas
              {{ authStore.user?.userType === 'Paciente' ? 'programadas' : 'asignadas' }}.
            </h3>
            <p class="text-charcoal-brown-500 max-w-sm mx-auto">
              Aquí aparecerán todas tus consultas médicas.
              <span v-if="authStore.user?.userType === 'Paciente'">
                Para comenzar, agenda una cita con uno de nuestros
                especialistas.
              </span>
            </p>
            <div class="mt-8" v-if="authStore.user?.userType === 'Paciente'">
              <button @click="navigateTo('/booking')"
                class="text-sky-reflection-600 font-semibold hover:text-sky-reflection-700 transition-colors">
                Ver doctores disponibles ?
              </button>
            </div>
          </div>

          <!-- Listado de Citas -->
          <div v-else :class="[
            'gap-6',
            authStore.user?.userType === 'Paciente' ? 'grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3' : 'flex flex-col'
          ]">
            <template v-if="authStore.user?.userType === 'Paciente'">
              <PatientAppointmentCard v-for="apt in sortedAppointments" :key="apt.id"
                :appointment="apt"
                :doctorCache="doctorCache" @confirm="confirmAppointmentAction" @cancel="initiateCancel" />
            </template>
            <template v-else>
              <DoctorAppointmentCard v-for="apt in sortedAppointments" :key="apt.id"
                :appointment="apt"
                :patientCache="patientCache" />
            </template>
          </div>
        </div>
      </section>

      <!-- Modals Dinamicos -->
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
import PatientAppointmentCard from '~/components/appointment/PatientAppointmentCard.vue'
import DoctorAppointmentCard from '~/components/appointment/DoctorAppointmentCard.vue'
import type { Appointment } from '~/models/appointment.model'
import type { Doctor } from '~/models/doctor.model'
import type { Patient } from '~/models/patient.model'

const authStore = useAuthStore()
const { getPatientAppointments, getPatientById } = usePatient()
const { getDoctorAppointments, getDoctorById } = useDoctor()
const { confirmAppointment, cancelAppointment } = useAppointment()

const allAppointments = ref<Appointment[]>([])
const doctorCache = ref<Record<number, Doctor>>({})
const patientCache = ref<Record<number, Patient>>({})
const loading = ref(true)

type ModalVariant = 'success' | 'warning' | 'error' | 'info'

// Estado del Modal Dinámico
const modalState = ref({
  isOpen: false,
  variant: 'info' as ModalVariant,
  title: '',
  description: '',
  type: '',
  showConfirm: true,
  showCancel: true,
  confirmText: 'Aceptar',
  targetAppointmentId: null as number | null
})
const cancelReason = ref('')

const sortedAppointments = computed(() => {
  return [...allAppointments.value].sort((a, b) =>
    new Date(a.appointmentDate).getTime() - new Date(b.appointmentDate).getTime()
  )
})

const loadData = async () => {
  if (!authStore.user?.profileId) {
    loading.value = false
    return
  }
  loading.value = true
  try {
    let result: Appointment[] | undefined = []

    if (authStore.user.userType === 'Paciente') {
      result = await getPatientAppointments(authStore.user.profileId)
    } else {
      result = await getDoctorAppointments(authStore.user.profileId)
    }

    if (result) {
      allAppointments.value = result
    } else {
      allAppointments.value = []
    }

    if (authStore.user.userType === 'Paciente') {
      // Extraer IDs únicos de doctores
      const doctorIds = [...new Set(allAppointments.value.map((apt: Appointment) => apt.doctorId))].filter((id): id is number => !!id)

      // Cargar info de los doctores que faltan en caché
      for (const docId of doctorIds) {
        if (!doctorCache.value[docId]) {
          const doctor = await getDoctorById(docId)
          if (doctor) {
            doctorCache.value[docId] = doctor
          }
        }
      }
    } else {
      // Extraer IDs únicos de pacientes
      const patientIds = [...new Set(allAppointments.value.map((apt: Appointment) => apt.patientId))].filter((id): id is number => !!id)

      // Cargar info de los pacientes
      for (const patId of patientIds) {
        if (!patientCache.value[patId]) {
          const patient = await getPatientById(patId)
          if (patient) {
            patientCache.value[patId] = patient
          }
        }
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

// --- LÓGICA DE CONFIRMACION ---
const confirmAppointmentAction = async (id: number) => {
  try {
    await confirmAppointment(id)
    await loadData()
    showModal('success', '¡Cita Confirmada!', 'Nos vemos pronto. Gracias por confirmar su asistencia.', 'Aceptar')
  } catch (error) {
    showModal('error', 'Error', 'Ocurrió un problema intentando confirmar la cita.')
  }
}

// --- LÓGICA DE CANCELACIÓN ---
const initiateCancel = (apt: Appointment) => {
  modalState.value = {
    isOpen: true,
    variant: 'warning',
    title: '¿Cancelar Cita?',
    description: 'Estás a punto de cancelar tu cita médica. Por favor, indica el motivo de la cancelación.',
    type: 'cancel-reason',
    showConfirm: true,
    showCancel: true,
    confirmText: 'Proceder con la Cancelación',
    targetAppointmentId: apt.id
  }
  cancelReason.value = ''
}

const executeCancel = async () => {
  if (!modalState.value.targetAppointmentId) return

  if (cancelReason.value.trim().length < 5) {
    alert("Por favor provee un motivo de cancelación más detallado.")
    return
  }

  try {
    await cancelAppointment(modalState.value.targetAppointmentId, cancelReason.value)
    await loadData()
    showModal('success', 'Cita Cancelada', 'La cita fue cancelada correctamente.', 'Aceptar')
  } catch (error) {
    showModal('error', 'Error', 'No se pudo cancelar la cita. Inténtalo nuevamente.')
  }
}

// --- UTILS PARA MODAL ---
const showModal = (variant: ModalVariant, title: string, description: string, confirmText = 'Aceptar', type = 'info') => {
  modalState.value = {
    isOpen: true,
    variant,
    title,
    description,
    type,
    showConfirm: true,
    showCancel: variant === 'warning',
    confirmText,
    targetAppointmentId: null
  }
}

const handleModalConfirm = () => {
  if (modalState.value.type === 'cancel-reason') {
    executeCancel()
  } else {
    modalState.value.isOpen = false
  }
}
</script>
