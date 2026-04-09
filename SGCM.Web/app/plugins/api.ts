import { toast } from 'vue-sonner'

export default defineNuxtPlugin(() => {
  const api = $fetch.create({
    baseURL: "http://localhost:5055/api",
    credentials: "include",

    onRequestError({ error }) {
      if (error && error.message.includes('fetch failed')) {
        toast.error("Error de conexión: No se pudo contactar con el servidor backend.");
      }
    },

    onResponseError({ response }) {
      if (response.status === 400) {
        const message = response._data?.message || "Datos inválidos";
        throw new Error(message);
      }

      if (response.status === 401) {
        toast.warning("No autorizado. Por favor inicia sesión.");
        throw Error("Unauthorized");
      }

      if (response.status === 404) {
        toast.info("El recurso solicitado no fue encontrado.");
        throw Error("Not found");
      }

      if (response.status >= 500) {
        toast.error("Error inesperado en el servidor. Intente más tarde.");
        throw Error("Server error");
      }

      // Default fallback
      toast.error("Ocurrió un error inesperado al contactar con el sistema.");
      throw new Error("Unexpected API error");
    },
  });

  return {
    provide: {
      api,
    },
  };
});