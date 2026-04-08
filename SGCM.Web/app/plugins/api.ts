export default defineNuxtPlugin(() => {
  const api = $fetch.create({
    baseURL: "http://localhost:5055/api",
    credentials: "include",

    onRequest({ options }) {
      options.headers = {
        ...options.headers,
      };
    },

    // Manejo global de errores
    onResponseError({ response }) {
      if (response.status === 401) {
        console.error("No autorizado");
      }

      if (response.status === 500) {
        console.error("Error del servidor");
      }
    },
  });

  return {
    provide: {
      api,
    },
  };
});
