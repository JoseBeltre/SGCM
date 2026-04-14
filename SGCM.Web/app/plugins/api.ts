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
        const message = response._data || "Datos inválidos";
        throw new Error(message);
      }

      if (response.status === 401) {
        const message = response._data || "No autorizado";
        toast.warning(message);
        throw Error("Unauthorized");
      }

      if (response.status === 404) {
        const message = response._data || "Recurso no encontrado";
        toast.info(message);
        throw Error("Not found");
      }

      if (response.status >= 500) {
        const message = response._data || "Error inesperado en el servidor. Intente más tarde.";
        toast.error(message);
        throw Error("Server error");
      }

      // Default fallback
      const message = response._data || "Ocurrió un error inesperado al contactar con el sistema.";
      toast.error(message);
      throw new Error("Unexpected API error");
    },
  });

  return {
    provide: {
      api,
    },
  };
});
