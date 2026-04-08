import type { Specialty } from "~/models/specialty.model"
import { useApiClient } from "./http/apiClient"

export const useSpecialtyService = () => {
  const api = useApiClient()

  // GET: Obtener todas las especialidades
  const getAllSpecialties = async (): Promise<Specialty[]> => {
    return await api('/specialty')
  }

  // GET: Obtener detalles de una especialidad por su ID
  const getSpecialtyById = async (specialtyId: number): Promise<Specialty> => {
    return await api(`/specialty/${specialtyId}`)
  }

  return {
    getAllSpecialties,
    getSpecialtyById,
  }
}
