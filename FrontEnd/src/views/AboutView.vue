<template>
  <div class="scroll">
    <ToDoInformationTable
        :isNeedNavButtons="isNeedNavButtons"
        :ownerToDoList="nowToDo"
        @updateData="loadData"
        @setFilters="setFilters"
        @before="downIndex"
        @next="upIndex"
    />
  </div>
</template>

<script lang="ts">
import ToDoInformationTable from "@/components/ToDoInformation/ToDoInformationTable.vue";
import {defineComponent, onMounted, ref} from "vue";
import {apiClient} from "@/communication/ApiClient";
import type {UserGetDto} from "@/dto/user/UserGetDto";
import type {ToDoItemGetDto} from "@/dto/toDoItem/ToDoItemGetDto";

export default defineComponent({
  components: {ToDoInformationTable},
  setup() {
    const userWithToDoList = ref<UserGetDto[] | null>(null);
    const isNeedNavButtons = ref<boolean>(false);
    const index = ref(0);
    const nowToDo = ref<UserGetDto | null>(null);
    const isCompleted = ref<boolean | null>(null);
    const priority = ref<number | null>(null);

    onMounted(async () => {
      await loadData();
    });

    async function loadData() {
      let response = await apiClient.get<UserGetDto[]>('/api/User')
      userWithToDoList.value = response.data;

      isNeedNavButtons.value = userWithToDoList.value.length > 1;

      nowToDo.value = userWithToDoList.value[index.value];

      await loadUserToDoList();
    }

    async function loadUserToDoList(){
      const response = await apiClient.getWithParams<ToDoItemGetDto[]>('/api/ToDoItem', {
        isGetCompleted: isCompleted.value,
        priorityLevel: priority.value,
        userId: nowToDo.value?.id ?? null
      });

      if(nowToDo.value?.id && response.data)
        nowToDo.value!.toDoItems = response.data;
    }

    async function upIndex() {
      index.value++;

      if (userWithToDoList.value && index.value > userWithToDoList.value?.length - 1)
        index.value = 0;

      if (userWithToDoList.value)
        nowToDo.value = userWithToDoList.value[index.value];

      await loadUserToDoList();
    }

    async function downIndex() {
      index.value--;

      if (userWithToDoList.value && index.value < 0)
        index.value = userWithToDoList.value?.length - 1;

      if (userWithToDoList.value)
        nowToDo.value = userWithToDoList.value[index.value];

      await loadUserToDoList();
    }

    async function setFilters(filter: { isCompleted: boolean | null, priority: number | null }) {
      isCompleted.value = filter.isCompleted;
      priority.value = filter.priority;

      await loadUserToDoList();
    }

    return {
      userWithToDoList,
      isNeedNavButtons,
      index,
      nowToDo,
      loadData,
      upIndex,
      downIndex,
      setFilters,
    }
  }
})

</script>

<style>
.scroll {
  display: flex;
  margin-right: 1em;
  width: 100%;
}
</style>