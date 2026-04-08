<script setup>
import { onMounted } from 'vue'

const authStore = useAuthStore()

onMounted(async () => {
  await authStore.fetchUser()
})
</script>

<template>
  <NuxtLayout>
    <NuxtPage :page-key="route => route.fullPath" :transition="{ name: 'page', mode: 'out-in' }" />
  </NuxtLayout>
</template>
