import type { Appointment, CreateAppointment } from '~/models/appointment.model'
import { useApiClient } from './http/apiClient'

export const useAppointementService = () => {
  const api = useApiClient()

  // POST: Crear una nueva cita
  const createAppointment = async (appointmentData: CreateAppointment): Promise<Appointment> => {
    return await api("/appointment", {
      method: "POST",
      body: JSON.stringify(appointmentData)
    })
  }

  // GET: Obtener una cita por su ID
  const getAppointmentById = async (appointmentId: number): Promise<Appointment> => {
    return await api(`/appointment/${appointmentId}`)
  }

  // GET: Obtener citas de una fecha específica y doctor
  const getAppointmentsByDateAndDoctor = async (date: string, doctorId: number): Promise<Appointment[]> => {
    return await api(`/appointment?date=${date}&doctorId=${doctorId}`)
  }

  return {
    createAppointment,
    getAppointmentById,
    getAppointmentsByDateAndDoctor
  }
}
