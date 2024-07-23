<template>
  <div class="modal-container">
    <div class="component">
      <div class="priority-manager">
        <h1 style="display: flex; justify-content: space-around;">Priority Manager</h1>
        <div class="priority-row">
          <p class="priority-row-p"><span>Priority Level</span></p>
          <p class="priority-row-p"><span>Count used</span></p>
        </div>
        <template v-for="priority in priorities" :key="priority.id">
          <div class="priority-row">
            <template v-if="oldPriority && oldPriority.level === priority.level">
              <input type="number" v-model="newPriority" min="1"/>
              <MyButton text-button="Save changes" type-button="complete" @click="updatePriority(priority)"/>
              <MyButton text-button="Close" @click="closeUpdateInput"/>
            </template>
            <template v-else>
              <p class="priority-row-p"><span>{{ priority.level }}</span></p>
              <p class="priority-row-p"><span>{{ priority.countUsed }}</span></p>
              <MyButton text-button="Change priority" type-button="edit" @click="openUpdateInput(priority)"/>
              <MyButton text-button="Delete" @click="deletePriority(priority.level)"/>
            </template>
          </div>
        </template>
        <div class="form-group">
          <input type="number" v-model.number="newPriority" placeholder="Enter new priority" min="1"/>
          <MyButton text-button="Add Priority" type-button="complete" @click="createPriority"/>
          <MyButton text-button="Close" @click="closeModalWindow"/>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import {compile, defineComponent, onMounted, ref} from 'vue';
import MyButton from '@/components/help/MyButton.vue';
import type {PriorityDto} from "@/dto/Prioryti/Priority";
import {apiClient} from "@/communication/ApiClient";

export default defineComponent({
  name: 'PriorityLevelModal',
  emits: ['closeModalWindow'],
  methods: {compile},
  components: {MyButton},
  setup(props, context) {
    const priorities = ref<PriorityDto[]>([]);
    const oldPriority = ref<PriorityDto | null>(null);
    const newPriority = ref<number | null>(null);
    const isOpenUpdateInput = ref<boolean>(false);
    const errorMessage = ref<string | null>(null);

    onMounted(async () => {
      await loadPriority();
    })

    async function loadPriority() {
      const res = await apiClient.get<PriorityDto[]>("/api/Priority");

      if (res.data)
        priorities.value = res.data;
    }

    async function createPriority() {
      if (newPriority.value === null || newPriority.value < 0) {
        errorMessage.value = "Priority level cannot be less than 0.";
        return;
      }

      const isExist = priorities.value.some(p => p.level === newPriority.value);
      if (isExist) {
        errorMessage.value = "This priority level already exists.";
        return;
      }

      await apiClient.post("/api/Priority", {level: newPriority.value});
      await loadPriority();
      errorMessage.value = null;
    }

    async function updatePriority(priority: PriorityDto) {
      if (newPriority.value === null || newPriority.value < 0) {
        errorMessage.value = "Priority level cannot be less than 0.";
        return;
      }

      const isExist = priorities.value.some(p => p.level === newPriority.value);
      if (isExist) {
        errorMessage.value = "This priority level already exists.";
        return;
      }

      const dto = {
        level: newPriority.value,
        OldLevel: priority.level
      }

      await apiClient.put("/api/Priority", dto);
      await loadPriority();
      errorMessage.value = null;
    }

    async function deletePriority(id: number) {
      await apiClient.delete("/api/Priority/" + id);
      await loadPriority();
    }

    function openUpdateInput(priority: PriorityDto) {
      newPriority.value = priority.level;
      oldPriority.value = priority;
    }

    function closeUpdateInput() {
      oldPriority.value = null;
    }

    function closeModalWindow(){
      context.emit("closeModalWindow");
    }

    return {
      priorities,
      oldPriority,
      newPriority,
      isOpenUpdateInput,
      errorMessage,
      openUpdateInput,
      closeUpdateInput,
      createPriority,
      updatePriority,
      deletePriority,
      closeModalWindow
    };
  }
});
</script>


<style scoped>
.modal-container {
  display: flex;
  justify-content: center;
  align-items: center;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: 1000;
}

.component {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 1.25em;
  border-radius: 0.625em;
  position: relative;
  background-color: #181818;
  font-weight: bold;
}

.priority-manager {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.form-group {
  display: flex;
  gap: 0.5rem;
  justify-content: space-around;
}

.priority-row {
  display: flex;
  flex-direction: row;
  gap: 0.5em;
}

.priority-row-p {
  display: flex;
  flex-direction: row;
  width: 10em;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
</style>
