<script setup lang="ts">
import { computed, onMounted, ref, toRaw, watch } from 'vue';
import type { AppointmentData, AwsConfig, EntityData, EntityType, ListItem } from '../shared/types';
import {
  DEFAULT_REGION,
  DEFAULT_TENANT_ID,
  ENTITY_LABELS,
  ENTITY_TYPES,
  UI_ENV_LABELS,
  defaultEntityData,
} from '../shared/types';
import {
  deleteItem,
  fetchItems,
  fetchMeta,
  fetchOrphanSlotLocks,
  upsertItem,
  type SlotLockOrphan,
} from './api';
import ConfirmDialog from './components/ConfirmDialog.vue';
import ItemForm from './components/ItemForm.vue';
import OrphanPanel from './components/OrphanPanel.vue';

type UiEnv = AwsConfig['env'];
type PendingAction = 'save' | 'delete';

const env = ref<UiEnv>('dev');
const tenantId = ref(DEFAULT_TENANT_ID);
const profile = ref('');
const region = ref(DEFAULT_REGION);

const entityType = ref<EntityType>('StaffMember');
const items = ref<ListItem[]>([]);
const selected = ref<ListItem | null>(null);
const isNew = ref(false);
const formData = ref<EntityData>(defaultEntityData('StaffMember', DEFAULT_TENANT_ID));

const tableName = ref('');
const deploymentEnvironment = ref('');
const loading = ref(false);
const saving = ref(false);
const error = ref('');
const orphans = ref<SlotLockOrphan[]>([]);
const orphansLoading = ref(false);

const confirmOpen = ref(false);
const pendingAction = ref<PendingAction>('save');
const confirmTitle = ref('');
const confirmMessage = ref('');
const confirmRequireTyped = ref<string | undefined>();

const aws = computed<AwsConfig>(() => ({
  env: env.value,
  tenantId: tenantId.value,
  profile: profile.value || undefined,
  region: region.value,
}));

const envClass = computed(() => env.value);

async function loadMeta() {
  const meta = await fetchMeta(aws.value);
  tableName.value = meta.tableName;
  deploymentEnvironment.value = meta.deploymentEnvironment;
}

async function loadItems() {
  loading.value = true;
  error.value = '';
  try {
    await loadMeta();
    const response = await fetchItems(aws.value, entityType.value);
    items.value = response.items;
    if (selected.value) {
      const stillExists = items.value.find(
        (item: ListItem) => item.PK === selected.value?.PK && item.SK === selected.value?.SK,
      );
      if (stillExists) selectItem(stillExists);
      else if (isNew.value) {
        // keep new form
      } else {
        selected.value = null;
        isNew.value = false;
        formData.value = defaultEntityData(entityType.value, tenantId.value);
      }
    }
    if (entityType.value === 'Appointment') {
      await loadOrphans();
    } else {
      orphans.value = [];
    }
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to load items';
  } finally {
    loading.value = false;
  }
}

async function loadOrphans() {
  orphansLoading.value = true;
  try {
    const response = await fetchOrphanSlotLocks(aws.value);
    orphans.value = response.orphans;
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to load orphan slot locks';
  } finally {
    orphansLoading.value = false;
  }
}

function selectItem(item: ListItem) {
  selected.value = item;
  isNew.value = false;
  formData.value = structuredClone(toRaw(item.data));
}

function startNew() {
  selected.value = null;
  isNew.value = true;
  formData.value = defaultEntityData(entityType.value, tenantId.value);
}

function updateField(key: string, value: string | number) {
  formData.value = { ...formData.value, [key]: value } as EntityData;
}

function openProdConfirm(action: PendingAction) {
  pendingAction.value = action;
  confirmTitle.value = action === 'save' ? 'Save to production' : 'Delete from production';
  confirmMessage.value =
    action === 'save'
      ? 'You are about to write to the production DynamoDB table.'
      : 'You are about to permanently delete from production.';
  confirmRequireTyped.value = 'prod';
  confirmOpen.value = true;
}

function openDeleteConfirm() {
  if (env.value === 'prod') {
    openProdConfirm('delete');
    return;
  }
  if (!confirm('Delete this item permanently?')) return;
  void performDelete();
}

function requestSave() {
  if (env.value === 'prod') {
    openProdConfirm('save');
    return;
  }
  void performSave();
}

async function onConfirmDialog() {
  confirmOpen.value = false;
  if (pendingAction.value === 'save') await performSave();
  else await performDelete();
}

async function performSave() {
  saving.value = true;
  error.value = '';
  try {
    await upsertItem({
      aws: aws.value,
      entityType: entityType.value,
      isNew: isNew.value,
      data: formData.value,
    });
    isNew.value = false;
    await loadItems();
    const saved = items.value.find((item: ListItem) => {
      if (entityType.value === 'TenantProfile') {
        return item.entityType === 'TenantProfile';
      }
      const idKey =
        entityType.value === 'StaffMember'
          ? 'StaffId'
          : entityType.value === 'Service'
            ? 'ServiceId'
            : entityType.value === 'Appointment'
              ? 'AppointmentId'
              : 'PrincipalId';
      return (item.data as unknown as Record<string, string>)[idKey] ===
        (formData.value as unknown as Record<string, string>)[idKey];
    });
    if (saved) selectItem(saved);
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Save failed';
  } finally {
    saving.value = false;
  }
}

async function performDelete() {
  if (!selected.value) return;
  saving.value = true;
  error.value = '';
  try {
    await deleteItem({
      aws: aws.value,
      entityType: entityType.value,
      PK: selected.value.PK,
      SK: selected.value.SK,
      appointment:
        entityType.value === 'Appointment'
          ? (formData.value as AppointmentData)
          : undefined,
    });
    selected.value = null;
    isNew.value = false;
    formData.value = defaultEntityData(entityType.value, tenantId.value);
    await loadItems();
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Delete failed';
  } finally {
    saving.value = false;
  }
}

function resetForEntity(type: EntityType) {
  selected.value = null;
  isNew.value = false;
  formData.value = defaultEntityData(type, tenantId.value);
}

function switchEntity(type: EntityType) {
  entityType.value = type;
}

watch([env, tenantId, profile, region], () => {
  void loadItems();
});

watch(entityType, (type) => {
  resetForEntity(type);
  void loadItems();
});

onMounted(() => {
  void loadItems();
});
</script>

<template>
  <div class="app-shell">
    <header class="top-bar">
      <h1>Barbershop DB Explorer</h1>

      <div class="field-group">
        <label for="env">Environment</label>
        <select id="env" v-model="env">
          <option v-for="(label, key) in UI_ENV_LABELS" :key="key" :value="key">
            {{ label }}
          </option>
        </select>
      </div>

      <div class="field-group">
        <label for="tenant">Tenant ID</label>
        <input id="tenant" v-model="tenantId" type="text" :disabled="entityType === 'Customer'" />
      </div>

      <div class="field-group">
        <label for="profile">AWS profile</label>
        <input id="profile" v-model="profile" type="text" placeholder="optional" />
      </div>

      <div class="field-group">
        <label for="region">Region</label>
        <input id="region" v-model="region" type="text" />
      </div>
    </header>

    <div class="env-banner" :class="envClass">
      Target: {{ UI_ENV_LABELS[env] }} ({{ deploymentEnvironment || '…' }})
      <span v-if="env === 'prod'"> — production writes require typing <code>prod</code></span>
    </div>

    <p v-if="tableName" class="meta-line">Table: {{ tableName }}</p>
    <p v-if="error" class="error-banner">{{ error }}</p>

    <nav class="entity-tabs">
      <button
        v-for="type in ENTITY_TYPES"
        :key="type"
        type="button"
        :class="{ active: entityType === type }"
        @click="switchEntity(type)"
      >
        {{ ENTITY_LABELS[type] }}
      </button>
    </nav>

    <div class="split-pane">
      <aside class="list-pane">
        <div class="list-header">
          <h2>{{ ENTITY_LABELS[entityType] }}</h2>
          <button class="btn btn-primary" type="button" @click="startNew">New</button>
        </div>
        <p v-if="loading" class="empty-state">Loading…</p>
        <p v-else-if="items.length === 0" class="empty-state">No items.</p>
        <ul v-else class="list-items">
          <li v-for="item in items" :key="item.SK">
            <button
              type="button"
              :class="{ selected: selected?.SK === item.SK && !isNew }"
              @click="selectItem(item)"
            >
              {{ item.label }}
            </button>
          </li>
        </ul>
      </aside>

      <main>
        <ItemForm
          v-if="selected || isNew"
          :entity-type="entityType"
          :form-data="formData"
          :is-new="isNew"
          :saving="saving"
          @update:field="updateField"
          @save="requestSave"
          @delete="openDeleteConfirm"
        />
        <p v-else class="empty-state">Select an item or create a new one.</p>

        <OrphanPanel
          v-if="entityType === 'Appointment'"
          :aws="aws"
          :orphans="orphans"
          :loading="orphansLoading"
          @refresh="loadOrphans"
          @error="(message) => (error = message)"
        />
      </main>
    </div>

    <ConfirmDialog
      :open="confirmOpen"
      :title="confirmTitle"
      :message="confirmMessage"
      :require-typed="confirmRequireTyped"
      confirm-label="Continue"
      @confirm="onConfirmDialog"
      @cancel="confirmOpen = false"
    />
  </div>
</template>
