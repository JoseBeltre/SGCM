import type { Patient } from "~/models/patient.model";
import type { Appointment } from "~/models/appointment.model";
import { useApiClient } from "./http/apiClient";

export const usePatientService = () => {
  const api = useApiClient();

  // GET: Obtener detalles de un paciente por su ID
  const getPatientById = async (patientId: number): Promise<Patient> => {
    return await api(`/patient/${patientId}`);
  };

  // GET: Obtener el paciente asociado a un identificador de usuario (UserId)
  const getPatientByUserId = async (userId: number): Promise<Patient> => {
    return await api(`/patient/user/${userId}`);
  };

  // GET: Obtener citas del paciente
  const getPatientAppointments = async (
    patientId: number,
  ): Promise<Appointment[]> => {
    return await api(`/patient/${patientId}/appointments`);
  };

  return {
    getPatientById,
    getPatientByUserId,
    getPatientAppointments,
  };
};
