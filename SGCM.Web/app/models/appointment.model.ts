export interface Appointment {
  id: number,
  patientId: number,
  doctorId: number,
  appointmentDate: string,
  durationMinutes: number,
  status: AppointmentStatus,
  consultationReason: string,
  doctorNotes: string | null,
  cancellationReason: string | null
}

export interface CreateAppointment {
  patientId: number,
  doctorId: number,
  appointmentDate: string,
  durationMinutes: number,
  consultationReason: string
}

export enum AppointmentStatus {
    Pendiente = 'Pendiente',
    Confirmada = 'Confirmada',
    Cancelada = 'Cancelada',
    Completada = 'Completada'
}
