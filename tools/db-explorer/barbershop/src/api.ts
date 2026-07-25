import type {
  AwsConfig,
  DeleteRequest,
  EntityData,
  EntityType,
  ListItem,
  MetaResponse,
  SlotLockOrphan,
  UpsertRequest,
} from '../shared/types';

function queryString(aws: AwsConfig, extra: Record<string, string> = {}): string {
  const params = new URLSearchParams({
    env: aws.env,
    tenantId: aws.tenantId,
    region: aws.region,
    ...extra,
  });
  if (aws.profile) params.set('profile', aws.profile);
  return params.toString();
}

async function request<T>(url: string, init?: RequestInit): Promise<T> {
  const response = await fetch(url, init);
  const body = await response.json();
  if (!response.ok) {
    throw new Error(body.error ?? `Request failed (${response.status})`);
  }
  return body as T;
}

export function fetchMeta(aws: AwsConfig): Promise<MetaResponse> {
  return request<MetaResponse>(`/api/meta?${queryString(aws)}`);
}

export function fetchItems(aws: AwsConfig, entityType: EntityType): Promise<{ items: ListItem[] }> {
  return request(`/api/items?${queryString(aws, { entityType })}`);
}

export function upsertItem(payload: UpsertRequest): Promise<{ ok: boolean }> {
  return request('/api/items', {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });
}

export function deleteItem(payload: DeleteRequest): Promise<{ ok: boolean }> {
  return request('/api/items', {
    method: 'DELETE',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });
}

export function fetchOrphanSlotLocks(
  aws: AwsConfig,
): Promise<{ orphans: SlotLockOrphan[] }> {
  return request(`/api/orphans/slot-locks?${queryString(aws)}`);
}

export function deleteOrphanSlotLock(
  aws: AwsConfig,
  PK: string,
  SK: string,
): Promise<{ ok: boolean }> {
  return request('/api/orphans/slot-locks', {
    method: 'DELETE',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ aws, PK, SK }),
  });
}

export type { EntityData, ListItem, SlotLockOrphan };
