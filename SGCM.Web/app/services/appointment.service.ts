import type { Appointment, CreateAppointment } from '~/models/appointment.model'
import { useApiClient } from './http/apiClient'

export const useAppointementService = () => {
  const api = useApiClient()

  // POST: Crear una nueva cita
  const createAppointment = async (appointmentData: CreateAppointment): Promise<Appointment> => {
    return await api('/appointment', {
      method: 'POST',
      body: appointmentData
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

  // GET: Obtener citas del paciente
  const getAppointmentsByPatientId = async (patientId: number): Promise<Appointment[]> => {
    return await api(`/patient/${patientId}/appointments`)
  }

  // PATCH: Cancelar cita
  const cancelAppointment = async (appointmentId: number, reason: string): Promise<void> => {
    return await api(`/appointment/${appointmentId}/cancelar`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(reason)
    })
  }

  // PATCH: Confirmar cita
  const confirmAppointment = async (appointmentId: number): Promise<void> => {
    return await api(`/appointment/${appointmentId}/confirm`, {
      method: 'PATCH'
    })
  }

  return {
    createAppointment,
    getAppointmentById,
    getAppointmentsByDateAndDoctor,
    getAppointmentsByPatientId,
    cancelAppointment,
    confirmAppointment
  }
}
