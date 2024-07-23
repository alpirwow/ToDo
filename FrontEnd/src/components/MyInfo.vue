<template>
  <ToDoInformationTable
      @updateData="loadData"
      @setFilters="setFilters"
      :ownerToDoList="user"
  />
  <SelectUserModal/>
</template>

<script lang="ts">
import {defineComponent, ref, watch} from "vue";
import SelectUserModal from "@/components/MyInfoComponents/SelectUserModal.vue";
import type {UserGetDto} from "@/dto/user/UserGetDto";
import {useMyUserStore} from "@/stores/userStore";
import {apiClient} from "@/communication/ApiClient";
import type {ToDoItemGetDto} from "@/dto/toDoItem/ToDoItemGetDto";
import ToDoInformationTable from "@/components/ToDoInformation/ToDoInformationTable.vue";

export default defineComponent({
  name: "MyInfo",
  components: {ToDoInformationTable, SelectUserModal},
  setup() {
    const userStore = useMyUserStore();
    const user = ref<UserGetDto | null>(null);
    const toDoList = ref<ToDoItemGetDto[] | null>(null);
    const isCompleted = ref<boolean | null>(null);
    const priority = ref<number | null>(null);

    watch(
        () => userStore.user,
        async (newUser) => {
          if (newUser) {
            user.value = newUser;

            await loadData();
          }
        },
        {immediate: true}
    );

    async function loadData() {
      try {
        const response = await apiClient.getWithParams<ToDoItemGetDto[]>('/api/ToDoItem/get-my-todo', {
          isGetCompleted: isCompleted.value,
          priorityLevel: priority.value
        });

        if (response.data)
          user.value!.toDoItems = response.data;
      } catch (error) {
        console.error('Error fetching ToDo items:', error);
      }
    }

    async function setFilters(filter: { isCompleted: boolean | null, priority: number | null }) {
      isCompleted.value = filter.isCompleted;
      priority.value = filter.priority;

      await loadData();
    }

    return {
      user,
      toDoList,
      loadData,
      setFilters,
    }
  },
})
</script>

<style scoped>

</style>