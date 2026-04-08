import type { Doctor } from "~/models/doctor.model";
import type { Specialty } from "~/models/specialty.model";

export function useBooking() {
  const specialty = useState<Specialty | null>('booking-specialty', () => null);
  const doctor = useState<Doctor | null>('booking-doctor', () => null);
  const date = useState<string | null>('booking-date', () => null);
  const time = useState<string | null>('booking-time', () => null);

  return {
    specialty,
    doctor,
    date,
    time,
  };
}
