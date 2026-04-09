import type {
  Appointment,
  CreateAppointment,
} from "~/models/appointment.model";
import { useApiClient } from "./http/apiClient";

export const useAppointmentService = () => {
  const api = useApiClient();

  // POST: Crear una nueva cita
  const createAppointment = async (
    appointmentData: CreateAppointment,
  ): Promise<Appointment> => {
    return await api("/appointment", {
      method: "POST",
      body: appointmentData,
    });
  };

  // GET: Obtener una cita por su ID
  const getAppointmentById = async (
    appointmentId: number,
  ): Promise<Appointment> => {
    return await api(`/appointment/${appointmentId}`);
  };

  // GET: Obtener citas de una fecha específica y doctor
  const getAppointmentsByDateAndDoctor = async (
    date: string,
    doctorId: number,
  ): Promise<Appointment[]> => {
    return await api(`/appointment?date=${date}&doctorId=${doctorId}`);
  };

  // PATCH: Cancelar cita
  const cancelAppointment = async (
    appointmentId: number,
    reason: string,
  ): Promise<void> => {
    return await api(`/appointment/${appointmentId}/cancelar`, {
      method: "PATCH",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(reason),
    });
  };

  // PATCH: Confirmar cita
  const confirmAppointment = async (appointmentId: number): Promise<void> => {
    return await api(`/appointment/${appointmentId}/confirm`, {
      method: "PATCH",
    });
  };

  // PATCH: Reagendar cita
  const rescheduleAppointment = async (
    appointmentId: number,
    newDate: string,
  ): Promise<void> => {
    return await api(`/appointment/${appointmentId}/reschedule`, {
      method: "PATCH",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(newDate),
    });
  };

  return {
    createAppointment,
    getAppointmentById,
    getAppointmentsByDateAndDoctor,
    cancelAppointment,
    confirmAppointment,
    rescheduleAppointment,
  };
};
