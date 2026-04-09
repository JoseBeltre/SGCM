import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useRouter } from '#app'
import { useAuthenticationService } from '~/services/authentication.service'

export const useAuthStore = defineStore('auth', () => {
  const router = useRouter()
  const authService = useAuthenticationService()
  
  const user = ref<any | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)
  const isInitialized = ref(false)

  const isAuthenticated = computed(() => !!user.value)

  /**
   * Inicia sesión gestionando el estado de carga y errores HTTP
   */
  const login = async (credentials: any) => {
    loading.value = true
    error.value = null
    try {
      const response = await authService.login(credentials)
      user.value = response
      isInitialized.value = true
      router.push('/')
      return true
    } catch (e: any) {
      if (e.name === 'FetchError' && !e.response) {
        error.value = 'El servidor no está disponible en este momento.';
      } else if (e.message === 'Unauthorized') {
        error.value = 'Credenciales inválidas';
      } else {
        error.value = e.message || 'Credenciales inválidas';
      }
      return false
    } finally {
      loading.value = false
    }
  }

  /**
   * Registra y auto-loggea a un paciente nuevo
   */
  const register = async (userData: any) => {
    loading.value = true
    error.value = null
    try {
      const response = await authService.register(userData)
      user.value = response
      isInitialized.value = true
      router.push('/')
      return true
    } catch (e: any) {
      if (e.name === 'FetchError' && !e.response) {
        error.value = 'El servidor no está disponible en este momento.';
      } else {
        error.value = e.message || 'Error al registrar la cuenta';
      }
      return false
    } finally {
      loading.value = false
    }
  }

  /**
   * Cierra completamente la sesión y envía al usuario al portal de acceso
   */
  const logout = async () => {
    try {
      await authService.logout()
    } catch (e) {
      console.error('Error al cerrar sesión', e)
    } finally {
      user.value = null
      router.push('/auth/login')
    }
  }

  /**
   * Recupera silenciosamente la sesión leyendo la Cookie
   */
  const fetchUser = async () => {
    if (isInitialized.value) return
    try {
      const response = await authService.fetchSession()
      user.value = response
    } catch (e) {
      user.value = null
    } finally {
      isInitialized.value = true
    }
  }

  return {
    user,
    loading,
    error,
    isInitialized,
    isAuthenticated,
    login,
    register,
    logout,
    fetchUser
  }
})
