export interface Patient {
  id: number,
  userId: number,
  nationalId: string,
  dateOfBirth: string,
  address: string,
  gender: number,
  emergencyPhone: string | null,
  emergencyContact: string | null,
  insuranceNumber: string,
  fullName: string,
  email: string,
  phone: string
}

