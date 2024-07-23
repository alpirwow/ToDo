<template>
  <div v-if="ownerToDoList" class="content">
    <div class="header-items">
      <MyButton v-if="isNeedNavButtons" text-button="Before" @click="beforeItem"/>
      <h1>{{ ownerToDoList.name }}</h1>
      <MyButton v-if="isNeedNavButtons" text-button="Next" @click="nextItem"/>
    </div>
    <div class="display-items">
      <template v-for="item in ownerToDoList.toDoItems" :key="item.id">
        <ToDoCard
            :to-do-item="item"
            @openModal="selectToDo"
            @updateData="updateData"
        />
      </template>
    </div>
    <div class="buttons">
      <MyButton text-button="Add new to do" @click="openModal"/>
      <MyButton text-button="Open editor priority" @click="closePriorityLevelModal"/>
      <MyButton text-button="Edit filter" type-button="edit" @click="openFilterModal"/>
    </div>
  </div>
  <ToDoModal
      v-if="isOpenModal"
      :item=selectedToDo
      @updateData="updateData"
  />
  <FilterModal
      v-if="isOpenFilterModal"
      @close="openFilterModal"
      @setFilters="setFilters"
  />
  <PriorityLevelModal
      v-if="isOpenPriorityLevelModal"
      @closeModalWindow="closePriorityLevelModal"
  />
</template>

<script lang="ts">
import {defineComponent, type PropType, ref} from "vue";
import type {ToDoItemGetDto} from "@/dto/toDoItem/ToDoItemGetDto";
import ToDoCard from "@/components/ToDoInformation/ToDoCard.vue";
import ToDoModal from "@/components/ToDoInformation/ToDoModal.vue";
import type {UserGetDto} from "@/dto/user/UserGetDto";
import MyButton from "@/components/help/MyButton.vue";
import FilterModal from "@/components/ToDoInformation/FilterModal.vue";
import PriorityLevelModal from "@/components/PriorityLevel/PriorityLevelModal.vue";

export default defineComponent({
  name: "ToDoInformationTable",
  emits: ['updateData', 'before', 'next', 'setFilters'],
  components: {PriorityLevelModal, FilterModal, MyButton, ToDoModal, ToDoCard},
  props: {
    ownerToDoList: {
      type: Object as PropType<UserGetDto | null>,
      required: true
    },
    isNeedNavButtons: {
      type: Boolean as PropType<boolean>,
      default: false
    }
  },
  setup(props, context) {
    const isOpenModal = ref<Boolean>(false);
    const isOpenFilterModal = ref<Boolean>(false);
    const isOpenPriorityLevelModal = ref<Boolean>(false);
    const selectedToDo = ref<ToDoItemGetDto | null>();

    function openModal() {
      selectedToDo.value = {
        id: '',
        title: '',
        description: '',
        isCompleted: false,
        dueDate: '',
        priorityLevel: 1,
        userId: props.ownerToDoList?.id ?? ''
      };
      isOpenModal.value = true;
    }

    function selectToDo(value: ToDoItemGetDto) {
      selectedToDo.value = value;
      isOpenModal.value = true;
    }

    function updateData() {
      isOpenModal.value = false;
      selectedToDo.value = null;
      context.emit('updateData');
    }

    function beforeItem() {
      context.emit('before');
    }

    function nextItem() {
      context.emit('next');
    }

    function openFilterModal() {
      isOpenFilterModal.value = !isOpenFilterModal.value;
    }

    function setFilters(filter: { isCompleted: boolean | null, priority: number | null }) {
      isOpenFilterModal.value = !isOpenFilterModal.value;
      context.emit("setFilters", filter);
    }

    function closePriorityLevelModal(){
      isOpenPriorityLevelModal.value = !isOpenPriorityLevelModal.value;
      context.emit('updateData');
    }

    return {
      isOpenModal,
      isOpenFilterModal,
      isOpenPriorityLevelModal,
      selectedToDo,
      openModal,
      selectToDo,
      updateData,
      beforeItem,
      nextItem,
      openFilterModal,
      setFilters,
      closePriorityLevelModal
    }
  }
})
</script>

<style scoped>
.content {
  display: flex;
  flex-direction: column;
  gap: 1em;
  width: 40em;
}

.header-items {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
}

.display-items {
  align-items: center;
  display: flex;
  flex-wrap: wrap;
  gap: 1em;
  height: 50em;
  overflow-y: auto;
}

.buttons {
  display: flex;
  flex-direction: row;
  gap: 1em;
  justify-content: space-evenly;
}
</style>