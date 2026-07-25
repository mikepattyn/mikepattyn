<script setup lang="ts">
import type { AwsConfig, SlotLockOrphan } from '../../shared/types';
import { deleteOrphanSlotLock } from '../api';

const props = defineProps<{
  aws: AwsConfig;
  orphans: SlotLockOrphan[];
  loading: boolean;
}>();

const emit = defineEmits<{
  refresh: [];
  error: [message: string];
}>();

async function remove(orphan: SlotLockOrphan) {
  if (!confirm(`Delete orphan slot lock ${orphan.SK}?`)) return;
  try {
    await deleteOrphanSlotLock(props.aws, orphan.PK, orphan.SK);
    emit('refresh');
  } catch (error) {
    emit('error', error instanceof Error ? error.message : 'Delete failed');
  }
}
</script>

<template>
  <section class="orphan-panel">
    <h3>Orphan slot locks</h3>
    <p>Locks without a matching appointment. Delete-only cleanup.</p>
    <p v-if="loading" class="empty-state">Loading orphans…</p>
    <p v-else-if="orphans.length === 0" class="empty-state">No orphan slot locks.</p>
    <ul v-else class="orphan-list">
      <li v-for="orphan in orphans" :key="orphan.SK">
        <span>{{ orphan.Date }} {{ orphan.TimeSlot }} · {{ orphan.StaffId }} · {{ orphan.AppointmentId }}</span>
        <button class="btn btn-danger" type="button" @click="remove(orphan)">Delete</button>
      </li>
    </ul>
  </section>
</template>
