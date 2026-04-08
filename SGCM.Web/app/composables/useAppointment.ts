import type {
  Appointment,
  CreateAppointment,
} from "~/models/appointment.model"
import { useAppointementService } from "~/services/appointment.service"

export function useAppointment() {
  const appointmentService = useAppointementService()
  const appointment = ref<Appointment | null>(null)
  const appointments = ref<Appointment[]>([])
  const error = ref<string | null>(null)
  const loading = ref<boolean>(false)

  const { patient, getPatientByUserId } = usePatient()

  const createAppointment = async (
    appointmentData: CreateAppointment,
  ): Promise<Appointment | undefined> => {
    loading.value = true
    error.value = null

    await getPatientByUserId(appointmentData.patientId)
    if (!patient) {
      error.value = "Paciente no encontrado para el ID de usuario proporcionado"
      loading.value = false
      return
    }
    console.log("Paciente encontrado:", patient)
    appointmentData.patientId = patient.value!.id
    console.log("Datos de cita actualizados con patientId:", appointmentData)

    try {
      const response =
        await appointmentService.createAppointment(appointmentData)
      appointment.value = response
      return response
    } catch (err) {
      error.value = "Error al crear la cita"
    } finally {
      loading.value = false
    }
  }

  const getAppointmentById = async (
    appointmentId: number,
  ): Promise<Appointment | undefined> => {
    loading.value = true
    error.value = null
    try {
      const response =
        await appointmentService.getAppointmentById(appointmentId)
      appointment.value = response
      return response
    } catch (err) {
      error.value = "Error al obtener la cita"
    } finally {
      loading.value = false
    }
  }

  const getAppointmentsByDateAndDoctor = async (
    date: string,
    doctorId: number,
  ): Promise<Appointment[] | undefined> => {
    loading.value = true
    error.value = null
    try {
      const response = await appointmentService.getAppointmentsByDateAndDoctor(
        date,
        doctorId,
      )
      appointments.value = response
      return response
    } catch (err) {
      error.value = "Error al obtener las citas"
    } finally {
      loading.value = false
    }
  }

  return {
    appointment,
    appointments,
    error,
    loading,
    createAppointment,
    getAppointmentById,
    getAppointmentsByDateAndDoctor,
  }
}
