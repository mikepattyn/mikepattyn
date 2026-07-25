<script setup lang="ts">
import { computed } from 'vue';
import type { EntityData, EntityType } from '../../shared/types';
import { ADR_0009_ENTITIES, ADR_0009_NOTE, entityFields } from '../../shared/types';

const props = defineProps<{
  entityType: EntityType;
  formData: EntityData;
  isNew: boolean;
  saving: boolean;
}>();

const emit = defineEmits<{
  save: [];
  delete: [];
  'update:field': [key: string, value: string | number];
}>();

const fields = computed(() => entityFields(props.entityType));
const showAdrNote = computed(() => ADR_0009_ENTITIES.includes(props.entityType));

function fieldValue(key: string): string | number {
  return (props.formData as unknown as Record<string, string | number>)[key];
}

function onInput(key: string, type: 'text' | 'number' | 'datetime', event: Event) {
  const target = event.target as HTMLInputElement;
  const value = type === 'number' ? Number(target.value) : target.value;
  emit('update:field', key, value);
}

function disabled(keyField?: boolean): boolean {
  return !props.isNew && Boolean(keyField);
}
</script>

<template>
  <section class="form-pane">
    <p v-if="showAdrNote" class="adr-note">{{ ADR_0009_NOTE }}</p>

    <form class="form-grid" @submit.prevent="emit('save')">
      <label v-for="field in fields" :key="field.key">
        <span>{{ field.label }}</span>
        <input
          :type="field.type === 'number' ? 'number' : 'text'"
          :value="fieldValue(field.key)"
          :disabled="disabled(field.keyField)"
          :placeholder="field.placeholder"
          @input="onInput(field.key, field.type, $event)"
        />
      </label>

      <div class="form-actions">
        <button class="btn btn-primary" type="submit" :disabled="saving">
          {{ saving ? 'Saving…' : 'Save' }}
        </button>
        <button
          v-if="!isNew"
          class="btn btn-danger"
          type="button"
          :disabled="saving"
          @click="emit('delete')"
        >
          Delete
        </button>
      </div>
    </form>
  </section>
</template>
