<script setup>
import { onMounted } from 'vue'
import { Toaster } from 'vue-sonner'

const authStore = useAuthStore()

onMounted(async () => {
  await authStore.fetchUser()
})
</script>

<template>
  <NuxtLayout>
    <NuxtPage :page-key="route => route.fullPath" :transition="{ name: 'page', mode: 'out-in' }" />
    <Toaster position="bottom-right" richColors closeButton />
  </NuxtLayout>
</template>
