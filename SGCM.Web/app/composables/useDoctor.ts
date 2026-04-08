import type { Availability } from "~/models/availability.model"
import type { Doctor } from "~/models/doctor.model"
import { useDoctorService } from "~/services/doctor.service"

export function useDoctor() {
  const doctorService = useDoctorService()

  const doctors = ref<Doctor[]>([])
  const doctor = ref<Doctor | null>(null)
  const error = ref<string | null>(null)
  const loading = ref<boolean>(false)
  const availability = ref<Availability[]>([])

  const getDoctorsBySpecialtyId = async (
    specialtyId: number,
  ): Promise<Doctor[] | undefined> => {
    loading.value = true
    error.value = null
    try {
      const response = await doctorService.getDoctorsBySpecialtyId(specialtyId)
      doctors.value = response
      return response
    } catch (err) {
      error.value = "Error al obtener los doctores"
    } finally {
      loading.value = false
    }
  }

  const getDoctorById = async (
    doctorId: number,
  ): Promise<Doctor | undefined> => {
    loading.value = true
    error.value = null
    try {
      const response = await doctorService.getDoctorById(doctorId)
      doctor.value = response
      return response
    } catch (err) {
      error.value = "Error al obtener el doctor"
    } finally {
      loading.value = false
    }
  }

  const getDoctorAvailability = async (
    doctorId: number,
  ): Promise<Availability[] | undefined> => {
    loading.value = true
    error.value = null
    try {
      const response = await doctorService.getDoctorAvailability(doctorId)
      availability.value = response
      return response
    } catch (err) {
      error.value = "Error al obtener la disponibilidad del doctor"
    } finally {
      loading.value = false
    }
  }

  const getDoctorAppointments = async (
    doctorId: number,
  ): Promise<any[] | undefined> => {
    loading.value = true
    error.value = null
    try {
      const response = await doctorService.getDoctorAppointments(doctorId)
      return response
    } catch (err) {
      error.value = "Error al obtener las citas del doctor"
    } finally {
      loading.value = false
    }
  }

  return {
    doctors,
    doctor,
    availability,
    error,
    loading,
    getDoctorsBySpecialtyId,
    getDoctorById,
    getDoctorAvailability,
    getDoctorAppointments,
  }
}
