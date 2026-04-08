import type { Doctor } from "~/models/doctor.model";
import { useApiClient } from "./http/apiClient";
import type { Availability } from "~/models/availability.model";

export const useDoctorService = () => {
  const api = useApiClient();

  // GET: Obtener doctores por especialidad
  const getDoctorsBySpecialtyId = async (
    specialtyId: number,
  ): Promise<Doctor[]> => {
    return await api(`/doctor?specialtyId=${specialtyId}`);
  };

  // GET: Obtener detalles de un doctor por su ID
  const getDoctorById = async (doctorId: number): Promise<Doctor> => {
    return await api(`/doctor/${doctorId}`);
  };

  // GET: Disponibilidad de un doctor
  const getDoctorAvailability = async (
    doctorId: number,
  ): Promise<Availability[]> => {
    return await api(`/doctor/${doctorId}/availability`);
  };

  // GET: Obtener todas las citas de un doctor
  const getDoctorAppointments = async (doctorId: number): Promise<any[]> => {
    return await api(`/doctor/${doctorId}/appointments`);
  };

  return {
    getDoctorsBySpecialtyId,
    getDoctorById,
    getDoctorAvailability,
    getDoctorAppointments,
  };
};
