<script setup lang="ts">
import { computed, ref, watch } from 'vue';

const props = defineProps<{
  open: boolean;
  title: string;
  message: string;
  confirmLabel?: string;
  requireTyped?: string;
}>();

const emit = defineEmits<{
  confirm: [];
  cancel: [];
}>();

const typed = ref('');

watch(
  () => props.open,
  (open) => {
    if (open) typed.value = '';
  },
);

const canConfirm = computed(() => {
  if (!props.requireTyped) return true;
  return typed.value === props.requireTyped;
});
</script>

<template>
  <div v-if="open" class="dialog-backdrop" @click.self="emit('cancel')">
    <div class="dialog" role="dialog" aria-modal="true">
      <h3>{{ title }}</h3>
      <p>{{ message }}</p>
      <input
        v-if="requireTyped"
        v-model="typed"
        type="text"
        :placeholder="`Type ${requireTyped} to confirm`"
        @keyup.enter="canConfirm && emit('confirm')"
      />
      <div class="dialog-actions">
        <button class="btn" type="button" @click="emit('cancel')">Cancel</button>
        <button
          class="btn btn-danger"
          type="button"
          :disabled="!canConfirm"
          @click="emit('confirm')"
        >
          {{ confirmLabel ?? 'Confirm' }}
        </button>
      </div>
    </div>
  </div>
</template>
