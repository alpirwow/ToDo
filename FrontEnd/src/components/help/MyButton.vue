<script lang="ts">
import {defineComponent} from "vue";

export default defineComponent({
  name: "MyButton",
  props: {
    textButton: {
      type: String,
      default: 'Button'
    },
    typeButton:{
      type: String,
      default: 'close'
    }
  },
  computed: {
    getButtonColor() {
      switch (this.typeButton) {
        case 'complete':
          return 'hsla(160, 100%, 37%, 1)';
        case 'edit':
          return 'hsla(45, 100%, 50%, 1)';
        default:
          return '#181818';
      }
    },
    getTextColor() {
      switch (this.typeButton) {
        case 'complete':
          return '#181818';
        case 'edit':
          return '#181818';
        default:
          return 'hsla(160, 100%, 37%, 1)';
      }
    },
  }
})
</script>

<template>
  <button class="button-style-close" :style="{ backgroundColor: getButtonColor }">
    <span class="button-text" :style="{ color: getTextColor }">{{textButton}}</span>
  </button>
</template>

<style scoped>
.button-style-close{
  align-items: center;
  border-radius: 1em;
  box-sizing: border-box;
  display: inline-flex;
  gap: 1em;
  justify-content: center;
  overflow: hidden;
  padding: 1em 2em;
  min-width: 13em;
  background-color: #181818;
}

.button-style-close:active {
  transform: scale(0.95);
}

.button-text {
  color: hsla(160, 100%, 37%, 1);
  font-size: 1.02em;
  font-weight: 600;
  letter-spacing: 0;
  line-height: normal;
  text-align: center;
  white-space: nowrap;
  width: fit-content;
}
</style>