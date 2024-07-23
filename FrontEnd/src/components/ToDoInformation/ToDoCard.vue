<template>
  <div class="card">
    <div class="info">
      <div class="frame-2">
        <div class="frame-3">
          <p :class="{'element': true, 'completed': toDoItem.isCompleted}">{{ toDoItem.title }}</p>
        </div>
        <div class="text-wrapper-2">Due date: {{ toDoItem.dueDate }}
        </div>
      </div>
      <div class="frame-4">
        <p class="p">
          <span class="span">Priority level: </span>
          <span class="text-wrapper-3">{{ toDoItem.priorityLevel }}</span>
        </p>
        <div class="div-description">
          <p class="p">
            <span class="text-wrapper-3">{{ toDoItem.description }} </span>
          </p>
        </div>
      </div>
    </div>
    <div class="buttons">
      <MyButton :text-button="toDoItem.isCompleted ? 'Resume': 'Finish'" typeButton="complete" @click="finishToDo"/>
      <MyButton text-button="Edit" typeButton="edit" @click="openModal"/>
      <MyButton text-button="Delete" @click="deleteToDo"/>
    </div>
  </div>
</template>

<script lang="ts">
import {defineComponent, type PropType, ref} from "vue";
import type {ToDoItemGetDto} from "@/dto/toDoItem/ToDoItemGetDto";
import MyButton from "@/components/help/MyButton.vue";
import {apiClient} from "@/communication/ApiClient";

export default defineComponent({
  name: "ToDoCard",
  components: {MyButton},
  emits: ["openModal","updateData"],
  props: {
    toDoItem: {
      type: Object as PropType<ToDoItemGetDto>,
      required: true
    },
  },
  setup(props, context) {
    const isOpenModal = ref<boolean>(false);

    function openModal() {
      context.emit("openModal", props.toDoItem);
    }

    async function deleteToDo(){
      await apiClient.delete("/api/ToDoItem/"+ props.toDoItem.id);
      context.emit("updateData");
    }

    async function finishToDo(){
      await apiClient.put("/api/ToDoItem/" + props.toDoItem.id);
      context.emit("updateData");
    }

    return {
      isOpenModal,
      openModal,
      deleteToDo,
      finishToDo
    }
  }
})
</script>

<style scoped>
.card {
  align-items: flex-start;
  background-color: #181818;
  border: 0.0625em solid rgba(84, 84, 84, 0.48);
  border-radius: 0.4em;
  display: flex;
  flex-direction: column;
  gap: 1em;
  overflow: hidden;
  padding: 1.25em;
  position: relative;
  width: 39em;
}

.card .info {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 1;
  flex-direction: column;
  gap: 1.3125em;
  position: relative;
  width: 100%;
  margin: 0;
}

.card .frame-2 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  flex-direction: column;
  gap: 0.1em;
  position: relative;
  width: 100%;
  margin: 0;
}

.card .frame-3 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 0 0 auto;
  gap: 0.3125em;
  position: relative;
  width: 100%;
  margin: 0;
}

.card .element {
  align-items: center;
  align-self: stretch;
  color: hsla(160, 100%, 37%, 1);
  flex: 1;
  font-size: 1.25em;
  font-weight: 700;
  letter-spacing: 0;
  line-height: 1.5em;
  margin-top: -0.0625em;
  position: relative;
  margin-bottom: 0;
}

.card .element::before {
  content: "";
  display: inline-block;
  vertical-align: middle;
  margin-right: 0.125em;
  margin-bottom: 0.125em;
  width: 0.625em;
  height: 0.625em;
  border-radius: 50%;
  background: hsla(160, 100%, 37%, 1);
}

.card .element.completed::before {
  background: #181818;
}

.card .text-wrapper-2 {
  color: rgba(235, 235, 235, 0.64);;
  font-size: 0.75em;
  font-weight: bold;
  letter-spacing: 0;
  line-height: normal;
  position: relative;
  white-space: nowrap;
  width: fit-content;
  margin: 0;
}

.card .frame-4 {
  align-items: flex-start;
  align-self: stretch;
  display: flex;
  flex: 1;
  flex-direction: column;
  gap: 0.875em;
  position: relative;
  width: 100%;
  margin: 0;
}

.card .div-description {
  height: 10em;
  overflow-y: auto;
}

.card .p {
  color: rgba(235, 235, 235, 0.64);;
  font-size: 1em;
  font-weight: 400;
  letter-spacing: 0;
  line-height: normal;
  margin-top: -0.0625em;
  position: relative;
  white-space: pre-wrap;
  width: fit-content;
  margin-bottom: 0;
  height: auto;
}

.card .span {
  color: rgba(235, 235, 235, 0.64);;
  font-size: 1em;
  font-weight: 400;
  letter-spacing: 0;
}

.card .text-wrapper-3 {
  font-weight: 700;
}

.card .buttons {
  display: flex;
  flex-direction: row;
  gap: 1em;
}

</style>