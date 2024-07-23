<template>
  <div class="modal-container">
    <div class="component">
      <div class="filter-form">
        <div class="form-group">
          <label for="completed">Filter by Completion:</label>
          <select v-model="isCompleted" id="completed">
            <option :value="null">All</option>
            <option :value="true">Completed</option>
            <option :value="false">Not Completed</option>
          </select>
        </div>
        <div class="form-group">
          <label for="priority">Filter by Priority:</label>
          <input type="number" v-model.number="priority" id="priority" min="1" placeholder="Enter positive number"/>
        </div>
        <div class="buttons">
          <MyButton text-button="Apply filters" type-button="complete" @click="applyFilters"/>
          <MyButton text-button="Close filter" @click="close"/>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import {defineComponent, ref} from "vue";
import MyButton from "@/components/help/MyButton.vue";

export default defineComponent({
  name: "FilterModal",
  emits: ["close", "setFilters"],
  components: {MyButton},
  setup(props, context) {
    const isCompleted = ref<boolean | null>(null);
    const priority = ref<number | null>(null);

    function applyFilters() {
      context.emit("setFilters", {isCompleted: isCompleted.value, priority: priority.value});
    }

    function close() {
      context.emit("close");
    }

    return {
      isCompleted,
      priority,
      applyFilters,
      close
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
  width: 30em;
  background-color: #181818;
  font-weight: bold;
}

.filter-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.form-group {
  display: flex;
  flex-direction: column;
}

label {
  margin-bottom: 0.5rem;
}

input, select {
  padding: 0.5rem;
  font-size: 1rem;
}

.buttons {
  display: flex;
  flex-direction: row;
  gap: 1em;
}
</style>