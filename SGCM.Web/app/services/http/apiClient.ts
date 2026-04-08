export function useApiClient() {
  const { $api } = useNuxtApp()
  return $api
}
