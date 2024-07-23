<template>
  <div class="modal-container">
    <div class="component">
      <form @submit.prevent="saveChanges">
        <div>
          <h1> {{ isUpdate ? 'Update to do' : 'Create new to do' }}</h1>
        </div>
        <div class="div-form-input">
          <label for="title">Title:</label>
          <input id="title" v-model="itemDto.title" type="text"/>
          <span v-if="errors.title">{{ errors.title }}</span>
        </div>
        <div class="div-form-input">
          <label for="description">Description:</label>
          <textarea id="description" v-model="itemDto.description" class="large-input"></textarea>
          <span v-if="errors.description">{{ errors.description }}</span>
        </div>
        <div class="div-form-input">
          <div>
            <label for="dueDate">Due Date:</label>
            <input id="dueDate" v-model="itemDto.dueDate" type="date"/>
          </div>
          <span v-if="errors.dueDate">{{ errors.dueDate }}</span>
        </div>
        <div class="div-form-input">
          <div>
            <label for="priorityLevel">Priority Level:</label>
            <input id="priorityLevel" v-model="itemDto.priorityLevel" type="number"/>
          </div>
          <span v-if="errors.priorityLevel">{{ errors.priorityLevel }}</span>
        </div>
        <div>
          <label for="user">User:</label>
          <select id="user" v-model="itemDto.userId">
            <option v-for="user in users" :key="user.id" :value="user.id">{{ user.name }}</option>
          </select>
          <span v-if="errors.userId">{{ errors.userId }}</span>
        </div>
        <div style="display: flex; justify-content: space-around;">
          <MyButton
              :textButton="isUpdate ? 'Update' : 'Create'"
              :typeButton="isUpdate ? 'edit' : 'complete'"/>
          <MyButton :textButton="'Close'" @click="closeModal"/>
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import {defineComponent, onMounted, type PropType, ref, watch} from "vue";
import type {ToDoItemGetDto} from "@/dto/toDoItem/ToDoItemGetDto";
import type {ToDoItemPutDto} from "@/dto/toDoItem/ToDoItemPutDto";
import type {UserGetDto} from "@/dto/user/UserGetDto";
import {apiClient} from "@/communication/ApiClient";
import MyButton from "@/components/help/MyButton.vue";
import {useMyUserStore} from "@/stores/userStore";

export default defineComponent({
  name: "ToDoModal",
  components: {MyButton},
  emits: ['updateData'],
  props: {
    item: {
      type: Object as PropType<ToDoItemGetDto | null>,
      required: false,
      default: null
    },
  },
  setup(props, context) {
    const itemDto = ref<ToDoItemPutDto>({
      id: '',
      title: '',
      description: '',
      isCompleted: false,
      dueDate: '',
      priorityLevel: 1,
      userId: ''
    });
    const users = ref<UserGetDto[]>();
    const errors = ref<{ [key: string]: string }>({});
    const user = useMyUserStore().getUser();
    const isUpdate = ref<boolean>(false);

    watch(
        () => props.item,
        async (newItem) => {
          itemDto.value = {
            id: newItem?.id || '',
            title: newItem?.title || '',
            description: newItem?.description || '',
            isCompleted: newItem?.isCompleted || false,
            dueDate: newItem?.dueDate || '',
            priorityLevel: newItem?.priorityLevel || 1,
            userId: newItem?.userId || user?.id || '',
          };

          isUpdate.value = newItem?.id !== '';
        },
        {immediate: true}
    );

    function validate() {
      errors.value = {};
      if (!itemDto.value.title || itemDto.value.title.length > 50)
        errors.value.title = "Title length can't be null or more than 50 characters.";

      if (itemDto.value.description && itemDto.value.description.length > 500)
        errors.value.description = "Description length can't be more than 500 characters.";

      if (!itemDto.value.dueDate)
        errors.value.dueDate = "Due date must be today or in the future.";

      if (itemDto.value.dueDate) {
        const dueDate = new Date(itemDto.value.dueDate);
        const today = new Date();
        today.setHours(0, 0, 0, 0); // Обнуляем время для текущей даты
        dueDate.setHours(0, 0, 0, 0); // Обнуляем время для даты задачи

        if (dueDate < today)
          errors.value.dueDate = "Due date must be today or in the future.";
      }

      if (itemDto.value.priorityLevel && itemDto.value.priorityLevel < 1)
        errors.value.priorityLevel = "Priority level must be a positive number.";

      if (!itemDto.value.userId)
        errors.value.userId = "User must be selected.";

      return Object.keys(errors.value).length === 0;
    }

    onMounted(async () => {
      await loadUsers();
    })

    async function loadUsers() {
      try {
        const response = await apiClient.get<UserGetDto[]>("/api/User");

        if (response && response.data.length > 0)
          users.value = response.data;

      } catch (error) {
        console.error('Error fetching users:', error);
      }
    }

    async function saveChanges() {
      if (validate()) {
        if (isUpdate.value)
          await update();
        else
          await create();

        closeModal();
      }
    }

    async function create() {
      await apiClient.post("/api/ToDoItem", itemDto.value);
    }

    async function update() {
      await apiClient.put(`/api/ToDoItem`, itemDto.value);
    }

    function closeModal() {
      itemDto.value = {
        id: '',
        title: '',
        description: '',
        isCompleted: false,
        dueDate: '',
        priorityLevel: 1,
        userId: ''
      };
      context.emit('updateData');
    }

    return {
      itemDto,
      users,
      errors,
      isUpdate,
      saveChanges,
      closeModal,
    }
  }
})
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
  width: 30em;
  background-color: #181818;
  font-weight: bold;
}

form {
  display: flex;
  flex-direction: column;
  gap: 0.625em;
  overflow-y: auto;
  width: 100%;
}

h1 {
  color: white;
}

.div-form-input {
  display: flex;
  flex-direction: column;
}

label {
  font-weight: bold;
}

input, textarea, select {
  padding: 0.5em;
  border: 0.0625em solid #ccc;
  border-radius: 0.4em;
  margin: 0.625em;

}

textarea.large-input {
  height: 15em; /* Устанавливаем высоту для большого текстового поля */
}

button {
  padding: 0.75em 1.5em;
  background-color: hsla(45, 100%, 50%, 1);
  color: #fff;
  border: none;
  border-radius: 0.25em;
  cursor: pointer;
}

button:hover {
  background-color: hsla(45, 100%, 40%, 1);
}

span {
  color: red;
  font-size: 0.875em;
}
</style>
