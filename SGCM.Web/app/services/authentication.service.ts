import { useApiClient } from "./http/apiClient";
import type { LoginCredentials, RegisterPatient, AuthSession } from "~/models/auth.model";

export function useAuthenticationService() {
  const api = useApiClient();

  /**
   * Envía las credenciales al backend para iniciar sesión.
   * @param credentials Contiene email y password
   * @returns Los datos de sesión (AuthSessionDto)
   */
  const login = async (credentials: LoginCredentials): Promise<AuthSession> => {
    return await api<AuthSession>(`/auth/login`, {
      method: "POST",
      body: credentials,
    })
  }

  /**
   * Envía los datos de un nuevo paciente para crear una cuenta.
   * @param userData Contiene nombre, documento, correo, contraseña, etc.
   * @returns Los datos de sesión (AuthSessionDto)
   */
  const register = async (userData: RegisterPatient): Promise<AuthSession> => {
    return await api<AuthSession>(`/auth/register`, {
      method: "POST",
      body: userData,
    })
  }

  /**
   * Cierra la sesión activa en el backend eliminando la Cookie.
   */
  const logout = async () => {
    return await api(`/auth/logout`, {
      method: "POST",
    })
  }

  /**
   * Obtiene la sesión actual basándose en la Cookie segura.
   * @returns Los datos de sesión actuales (AuthSessionDto)
   */
  const fetchSession = async (): Promise<AuthSession> => {
    return await api<AuthSession>(`/auth/me`)
  }

  return {
    login,
    register,
    logout,
    fetchSession,
  }
}
