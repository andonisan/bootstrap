<script setup lang="ts">
import Todos from './components/Todos.vue';
</script>

<template>
  <header>
    <div class="wrapper">
      <Todos></Todos>
    </div>
  </header>
</template>

<style scoped>
body #app header {
  margin: 0;
  padding: 0;
}

header {
  height: 100vh;
  width: 100vw;
  display: flex;
  align-items: center;
  justify-content: center;
}

.wrapper {
  padding-left: 30px;
}
</style>