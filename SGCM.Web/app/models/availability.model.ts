export interface Availability {
    id: number,
    doctorId: number,
    startTime: string,
    endTime: string,
    dayOfWeek: DayOfWeek,
    isActive: boolean,
    appointmentDuration: number
}

export enum DayOfWeek {
    Lunes = 'Lunes',
    Martes = 'Martes',
    Miercoles = 'Miércoles',
    Jueves = 'Jueves',
    Viernes = 'Viernes',
    Sabado = 'Sábado',
    Domingo = 'Domingo'
}
