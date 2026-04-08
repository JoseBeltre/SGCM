import type { Doctor } from '~/models/doctor.model'

export const getStatusClass = (status: string) => {
  switch (status) {
    case 'Confirmada': return 'bg-palm-leaf-100 text-palm-leaf-800'
    case 'Pendiente': return 'bg-honey-bronze-100 text-honey-bronze-800'
    case 'Cancelada': return 'bg-red-100 text-red-800'
    case 'Completada': return 'bg-charcoal-brown-200 text-charcoal-brown-800'
    default: return 'bg-sky-reflection-100 text-sky-reflection-800'
  }
}

export const formatDate = (dateStr: string) => {
  const d = new Date(dateStr)
  return d.toLocaleDateString('es-ES', { weekday: 'short', day: '2-digit', month: 'short' })
}

export const formatTime = (dateStr: string) => {
  const d = new Date(dateStr)
  return d.toLocaleTimeString('es-ES', { hour: '2-digit', minute: '2-digit', hour12: true })
}

export const getDoctorName = (id: number, doctorCache: Record<number, Doctor>) => {
  return doctorCache[id] ? doctorCache[id].fullName : 'Cargando...'
}
