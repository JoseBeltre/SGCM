import type { Availability } from '~/models/availability.model';
import type { Appointment } from '~/models/appointment.model';

const jsDayToEnum = [
  'Domingo',
  'Lunes',
  'Martes',
  'Miércoles',
  'Jueves',
  'Viernes',
  'Sábado'
];

export const parseTimeToMinutes = (value: string): number => {
  const normalizedValue = value.trim();
  const isoDate = new Date(normalizedValue);

  if (!Number.isNaN(isoDate.getTime()) && normalizedValue.includes('T')) {
    return isoDate.getHours() * 60 + isoDate.getMinutes();
  }

  const match = normalizedValue.match(/^(\d{1,2}):(\d{2})(?::\d{2})?\s*(AM|PM)?$/i);

  if (!match) {
    return 0;
  }

  let hours = Number(match[1]);
  const minutes = Number(match[2]);
  const meridiem = match[3]?.toUpperCase();

  if (meridiem === 'PM' && hours < 12) {
    hours += 12;
  }

  if (meridiem === 'AM' && hours === 12) {
    hours = 0;
  }

  return hours * 60 + minutes;
};

export const formatMinutesToTime = (minutes: number): string => {
  const hours24 = Math.floor(minutes / 60) % 24;
  const mins = minutes % 60;
  const period = hours24 >= 12 ? 'PM' : 'AM';
  const hours12 = hours24 % 12 || 12;

  return `${hours12}:${mins.toString().padStart(2, '0')} ${period}`;
};

export const isOverlapping = (
  slotStartMin: number, 
  slotEndMin: number, 
  appointments: Appointment[]
): boolean => {
  if (!appointments || appointments.length === 0) return false;

  return appointments.some(appt => {
    const apptStartMin = parseTimeToMinutes(appt.appointmentDate);
    const apptEndMin = apptStartMin + appt.durationMinutes;

    // Check overlap: slot starts before appt ends AND slot ends after appt starts
    return slotStartMin < apptEndMin && slotEndMin > apptStartMin;
  });
};

export const generateFilteredTimeSlots = (
  date: string, 
  availability: Availability[], 
  appointments: Appointment[], 
  durationMinutes: number = 45
): string[] => {
  if (!date || !availability) return [];

  const d = new Date(`${date}T12:00:00`);
  const dayNum = d.getDay();
  const dayEnum = jsDayToEnum[dayNum];

  // Gather all shift limits for the selected day
  const slotsConfig = availability.filter(
    slot => (slot.dayOfWeek === dayEnum || (slot.dayOfWeek as unknown as number) === dayNum) && slot.isActive
  );

  const generatedSlots: string[] = [];

  for (const config of slotsConfig) {
    const startMinutes = parseTimeToMinutes(config.startTime);
    const endMinutes = parseTimeToMinutes(config.endTime);

    for (let currentMinutes = startMinutes; (currentMinutes + durationMinutes) <= endMinutes; currentMinutes += durationMinutes) {
      if (!isOverlapping(currentMinutes, currentMinutes + durationMinutes, appointments)) {
        generatedSlots.push(formatMinutesToTime(currentMinutes));
      }
    }
  }

  return [...new Set(generatedSlots)].sort((left, right) => parseTimeToMinutes(left) - parseTimeToMinutes(right));
};
