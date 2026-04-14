<script setup>
import { onMounted } from "vue";
import { Toaster } from "vue-sonner";
import "vue-sonner/style.css";

const authStore = useAuthStore();

onMounted(async () => {
  await authStore.fetchUser();
});
</script>

<template>
  <NuxtLayout>
    <NuxtPage :page-key="(route) => route.fullPath" :transition="{ name: 'page', mode: 'out-in' }" />
  </NuxtLayout>
  <Toaster position="bottom-right" richColors closeButton />
</template>
