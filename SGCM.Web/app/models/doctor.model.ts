export interface Doctor {
  id: number
  userId: number
  specialtyId: number
  nationalId: string
  licenseNumber: string
  hireDate: Date
  assignedOffice: string
  isActive: boolean
  fullName: string
  email: string
  phone: string
}
