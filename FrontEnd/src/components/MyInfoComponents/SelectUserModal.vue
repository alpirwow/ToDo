<template>
  <div v-if="isOpenModal" class="modal">
    <div class="component">
      <h1 class="title-modal">Do CRUD users or close</h1>
      <div class="form">
        <template v-for="user in users" :key="user.id">
          <div class="user-row">
            <template v-if="userDto.id === user.id">
              <input type="text" v-model="userDto.name">
              <MyButton text-button="Change user name" type-button="complete" @click="updateUser"/>
              <MyButton text-button="Close" @click="closeEdit"/>
            </template>
            <template v-else>
              <MyButton :text-button="'Select ' + user.name" type-button="complete" @click="selectUser(user)"/>
              <MyButton :text-button="'Edit ' + user.name" type-button="edit" @click="openChangeUser(user)"/>
              <MyButton :text-button="'Delete ' + user.name" @click="deleteUser(user.id)"/>
            </template>
          </div>
        </template>
      </div>
      <div class="buttons">
        <template v-if="isOpenCreateUser">
          <input type="text" v-model="userDto.name">
          <MyButton text-button="Add user" type-button="complete" @click="createUser"/>
          <MyButton :textButton="'Close creator'" @click="closeCreator"/>
        </template>
        <template v-else>
          <MyButton text-button="Open creator" type-button="complete" @click="openCreator"/>
          <MyButton :textButton="'Close'" @click="closeModal"/>
        </template>
      </div>
      <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
    </div>
  </div>
</template>

<script lang="ts">
import {defineComponent, onMounted, ref} from "vue";
import {apiClient} from "@/communication/ApiClient";
import type {UserGetDto} from "@/dto/user/UserGetDto";
import {useMyUserStore} from "@/stores/userStore";
import MyButton from "@/components/help/MyButton.vue";
import type {UserPutDto} from "@/dto/user/UserPutDto";

export default defineComponent({
  name: "SelectUserModal",
  components: {MyButton},
  setup() {
    const userStore = useMyUserStore();
    const isOpenModal = ref(!userStore.getUser());
    const isOpenCreateUser = ref(false);
    const users = ref<UserGetDto[]>();
    const userDto = ref<UserPutDto>({id: '', name: ''});
    const errorMessage = ref<string | null>(null);

    onMounted(async () => {
      await loadData();
    });

    async function loadData() {
      let response = await apiClient.get<UserGetDto[]>("/api/User");

      if (response && response.data.length > 0)
        users.value = response.data;

      closeEdit();
    }

    function selectUser(value: UserGetDto) {
      if (value) {
        userStore.setUser(value);

        closeModal();
      }
    }

    function closeModal() {
      isOpenModal.value = false;
    }

    async function deleteUser(userId: string) {
      await apiClient.delete("/api/User/" + userId);
      await loadData();
    }

    async function openChangeUser(user: UserGetDto) {
      userDto.value.id = user.id;
      userDto.value.name = user.name;
      isOpenCreateUser.value = false;
      errorMessage.value = null;
    }

    function closeEdit() {
      userDto.value.id = '';
      userDto.value.name = '';
      errorMessage.value = null;
    }

    async function updateUser() {
      if (userDto.value.id && userDto.value.name) {
        await apiClient.put("/api/User/", userDto.value);
        await loadData();
        errorMessage.value = null;
      } else {
        errorMessage.value = "User name cannot be empty.";
      }
    }

    function openCreator() {
      userDto.value.id = '';
      userDto.value.name = '';
      isOpenCreateUser.value = true;
      errorMessage.value = null;
    }

    function closeCreator() {
      userDto.value.id = '';
      userDto.value.name = '';
      isOpenCreateUser.value = false;
      errorMessage.value = null;
    }

    async function createUser() {
      if (!userDto.value.id && userDto.value.name) {
        await apiClient.post("/api/User/", {name: userDto.value.name});
        await loadData();
        errorMessage.value = null;
      } else {
        errorMessage.value = "User name cannot be empty.";
      }
    }

    return {
      isOpenModal,
      users,
      userDto,
      isOpenCreateUser,
      closeModal,
      selectUser,
      deleteUser,
      openChangeUser,
      closeEdit,
      updateUser,
      openCreator,
      closeCreator,
      createUser,
      errorMessage
    };
  },
})
</script>

<style scoped>
.modal {
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
  width: 45em;
  background-color: #181818;
  font-weight: bold;
  gap: 1em;
}

.title-modal {
  color: white;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 0.625em;
  height: 40vh;
  overflow-y: auto;
  width: 100%;
}

.user-row {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
}

.user-style {
  color: hsla(160, 100%, 37%, 1);
}

label, input, select, button {
  margin: 0.625em;
}

input[type="text"], input[type="date"], select {
  padding: 0.625em;
  border-radius: 0.4em;
  border: 0.0625em solid #ccc;
  width: 100%;
}

.buttons {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
}

</style>