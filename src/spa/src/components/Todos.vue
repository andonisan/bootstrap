<template>
  <form class="flex justify-center">
    <Fieldset legend="New todo">
      <InputText id="new-todo" v-model="newTodoTitle" aria-describedby="New Todo" />
      <Button @click.prevent="addTodo">Add</Button>
    </Fieldset>
  </form>

  <DataTable :value="todos" stripedRows tableStyle="min-width: 50rem">
    <Column field="title" header="Title"></Column>
    <Column field="isComplete" header="Is Complete">
      <template #body="slotProps">
        <ToggleSwitch v-model="slotProps.data.isCompleted" @click="updateTodo(slotProps.data)" />
      </template>
    </Column>
    <Column header="Actions">
      <template #body="slotProps">
        <Button @click="deleteTodo(slotProps.data.id)">Delete</Button>
      </template>
    </Column>
  </DataTable>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';

interface Todo {
  id: string;
  title: string;
  isCompleted: boolean;
}

const todos = ref<Todo[]>([]);
const newTodoTitle = ref('');

const fetchTodos = async (): Promise<void> => {
  try {
    const response = await axios.get<Todo[]>('api/todos');
    todos.value = response.data;
  } catch (error) {
    console.error('Error fetching todos:', error);
  }
};

const addTodo = async (): Promise<void> => {
  try {
    if (!newTodoTitle.value.trim()) return;

    const newTodo = { title: newTodoTitle.value };
    await axios.post('api/todos', newTodo);
    newTodoTitle.value = '';
    await fetchTodos();
  } catch (error) {
    console.error('Error adding todo:', error);
  }
};

const updateTodo = async (todo: Todo): Promise<void> => {
  try {
    await axios.put(`api/todos/${todo.id}/complete`);
    await fetchTodos();
  } catch (error) {
    console.error('Error updating todo:', error);
  }
};

const deleteTodo = async (id: string): Promise<void> => {
  try {
    await axios.delete(`api/todos/${id}`);
    await fetchTodos();
  } catch (error) {
    console.error('Error deleting todo:', error);
  }
};

onMounted(() => {
  fetchTodos();
});
</script>