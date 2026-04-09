import type {
  Appointment,
  CreateAppointment,
} from "~/models/appointment.model";
import { useAppointmentService } from "~/services/appointment.service";

export function useAppointment() {
  const appointmentService = useAppointmentService();
  const appointment = ref<Appointment | null>(null);
  const appointments = ref<Appointment[]>([]);
  const error = ref<string | null>(null);
  const loading = ref<boolean>(false);

  const createAppointment = async (
    appointmentData: CreateAppointment,
  ): Promise<Appointment | undefined> => {
    loading.value = true;
    error.value = null;

    try {
      const response = await appointmentService.createAppointment(appointmentData);
      appointment.value = response;
      return response;
    } catch (err: any) {
      error.value = err.message || null;
      return undefined;
    } finally {
      loading.value = false;
    }
  };

  const getAppointmentById = async (
    appointmentId: number,
  ): Promise<Appointment | undefined> => {
    loading.value = true;
    error.value = null;
    try {
      const response = await appointmentService.getAppointmentById(appointmentId);
      appointment.value = response;
      return response;
    } catch (err) {
      return undefined;
    } finally {
      loading.value = false;
    }
  };

  const getAppointmentsByDateAndDoctor = async (
    date: string,
    doctorId: number,
  ): Promise<Appointment[] | undefined> => {
    loading.value = true;
    error.value = null;
    try {
      const response = await appointmentService.getAppointmentsByDateAndDoctor(
        date,
        doctorId,
      );
      appointments.value = response;
      return response;
    } catch (err) {
      return undefined;
    } finally {
      loading.value = false;
    }
  };

  const confirmAppointment = async (
    appointmentId: number,
  ): Promise<boolean> => {
    loading.value = true;
    error.value = null;
    try {
      await appointmentService.confirmAppointment(appointmentId);
      return true;
    } catch (err: any) {
      error.value = err.message || null;
      return false;
    } finally {
      loading.value = false;
    }
  };

  const cancelAppointment = async (
    appointmentId: number,
    reason: string,
  ): Promise<boolean> => {
    loading.value = true;
    error.value = null;
    try {
      await appointmentService.cancelAppointment(appointmentId, reason);
      return true;
    } catch (err: any) {
      error.value = err.message || null;
      return false;
    } finally {
      loading.value = false;
    }
  };

  const rescheduleAppointment = async (
    appointmentId: number,
    newDate: string,
  ): Promise<boolean> => {
    loading.value = true;
    error.value = null;
    try {
      await appointmentService.rescheduleAppointment(appointmentId, newDate);
      return true;
    } catch (err: any) {
      error.value = err.message || null;
      return false;
    } finally {
      loading.value = false;
    }
  };

  return {
    appointment,
    appointments,
    error,
    loading,
    createAppointment,
    getAppointmentById,
    getAppointmentsByDateAndDoctor,
    confirmAppointment,
    cancelAppointment,
    rescheduleAppointment,
  };
}
