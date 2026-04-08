import type { Specialty } from "~/models/specialty.model";
import { useSpecialtyService } from "~/services/specialty.service";

export function useSpecialty() {
  const specialtyService = useSpecialtyService();
  const specialty = ref<Specialty | null>(null);
  const specialties = ref<Specialty[]>([]);

  const error = ref<string | null>(null);
  const loading = ref<boolean>(false);

  const getSpecialties = async (): Promise<Specialty[] | undefined> => {
    loading.value = true;
    error.value = null;
    try {
      const response = await specialtyService.getAllSpecialties();
      specialties.value = response;
      return response;
    } catch (err) {
      error.value = "Error al obtener las especialidades";
    } finally {
      loading.value = false;
    }
  };

  const getSpecialtyById = async (
    specialtyId: number,
  ): Promise<Specialty | undefined> => {
    loading.value = true;
    error.value = null;
    try {
      const response = await specialtyService.getSpecialtyById(specialtyId);
      specialties.value = [response];
      return response;
    } catch (err) {
      error.value = "Error al obtener la especialidad";
    } finally {
      loading.value = false;
    }
  };

  return {
    specialty,
    specialties,
    error,
    loading,
    getSpecialties,
    getSpecialtyById,
  };
}
