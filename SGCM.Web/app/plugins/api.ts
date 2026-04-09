export default defineNuxtPlugin(() => {
  const api = $fetch.create({
    baseURL: "http://localhost:5055/api",
    credentials: "include",

    onResponseError({ response }) {
      let message = "Error inesperado";

      if (response.status === 401) {
        message = "No autorizado. Inicia sesión.";
      }

      if (response.status === 400) {
        message = response._data?.message || "Datos inválidos";
      }

      if (response.status === 404) {
        message = "Recurso no encontrado";
      }

      if (response.status === 500 || response.status === 502 || response.status === 503 || response.status === 504) {
        message = "Error del servidor";
      }

      throw new Error(message);
    },
  });

  return {
    provide: {
      api,
    },
  };
});