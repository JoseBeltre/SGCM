import type { Patient } from "~/models/patient.model";
import type { Appointment } from "~/models/appointment.model";
import { usePatientService } from "~/services/patient.service";

export function usePatient() {
  const patient = ref<Patient | null>(null);
  const loading = ref(false);
  const error = ref<string | null>(null);
  const appointments = ref<Appointment[]>([]);

  const patientService = usePatientService();

  const getPatientById = async (
    patientId: number,
  ): Promise<Patient | undefined> => {
    loading.value = true;
    error.value = null;
    try {
      const response = await patientService.getPatientById(patientId);
      patient.value = response;
      return response;
    } catch (err) {
      error.value = "Error al cargar los datos del paciente.";
    } finally {
      loading.value = false;
    }
  };

  const getPatientByUserId = async (
    userId: number,
  ): Promise<Patient | undefined> => {
    loading.value = true;
    error.value = null;
    try {
      const response = await patientService.getPatientByUserId(userId);
      patient.value = response;
      return response;
    } catch (err) {
      error.value = "Error al cargar los datos del paciente desde el usuario.";
    } finally {
      loading.value = false;
    }
  };

  const getPatientAppointments = async (
    patientId: number,
  ): Promise<Appointment[] | undefined> => {
    loading.value = true;
    error.value = null;
    try {
      const response = await patientService.getPatientAppointments(patientId);
      appointments.value = response;
      return response;
    } catch (err) {
      error.value = "Error al obtener las citas del paciente.";
    } finally {
      loading.value = false;
    }
  };

  return {
    patient,
    appointments,
    loading,
    error,
    getPatientById,
    getPatientByUserId,
    getPatientAppointments,
  };
}
