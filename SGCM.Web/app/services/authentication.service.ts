export function useAuthenticationService() {
  /**
   * Envía las credenciales al backend para iniciar sesión.
   * @param credentials Contiene email y password
   * @returns Los datos de sesión (AuthSessionDto)
   */
  const login = async (credentials: any) => {
    return await $fetch<any>(`/api/auth/login`, {
      method: "POST",
      body: credentials,
    })
  }

  /**
   * Envía los datos de un nuevo paciente para crear una cuenta.
   * @param userData Contiene nombre, documento, correo, contraseña, etc.
   * @returns Los datos de sesión (AuthSessionDto)
   */
  const register = async (userData: any) => {
    return await $fetch<any>(`/api/auth/register`, {
      method: "POST",
      body: userData,
    })
  }

  /**
   * Cierra la sesión activa en el backend eliminando la Cookie.
   */
  const logout = async () => {
    return await $fetch(`/api/auth/logout`, {
      method: "POST",
    })
  }

  /**
   * Obtiene la sesión actual basándose en la Cookie segura.
   * @returns Los datos de sesión actuales (AuthSessionDto)
   */
  const fetchSession = async () => {
    return await $fetch<any>(`/api/auth/me`)
  }

  return {
    login,
    register,
    logout,
    fetchSession,
  }
}
