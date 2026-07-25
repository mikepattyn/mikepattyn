export type UiEnv = 'dev' | 'acc' | 'prod';

export const UI_ENV_LABELS: Record<UiEnv, string> = {
  dev: 'Dev',
  acc: 'Acc',
  prod: 'Prod',
};

export const UI_ENV_TO_DEPLOYMENT: Record<UiEnv, string> = {
  dev: 'Development',
  acc: 'Staging',
  prod: 'Production',
};

export type EntityType =
  | 'TenantProfile'
  | 'StaffMember'
  | 'Service'
  | 'Appointment'
  | 'Customer';

export const ENTITY_TYPES: EntityType[] = [
  'TenantProfile',
  'StaffMember',
  'Service',
  'Appointment',
  'Customer',
];

export const ENTITY_LABELS: Record<EntityType, string> = {
  TenantProfile: 'Profile',
  StaffMember: 'Staff',
  Service: 'Service',
  Appointment: 'Appointment',
  Customer: 'Customer',
};

export interface AwsConfig {
  env: UiEnv;
  tenantId: string;
  profile?: string;
  region: string;
}

export interface TenantProfileData {
  TenantId: string;
  DisplayName: string;
  Address: string;
  Hours: string;
  Email: string;
  Phone: string;
  HeroImageSrc: string;
  Timezone: string;
}

export interface StaffMemberData {
  StaffId: string;
  TenantId: string;
  Name: string;
}

export interface ServiceData {
  ServiceId: string;
  TenantId: string;
  DurationMinutes: number;
  PriceCents: number;
}

export interface AppointmentData {
  AppointmentId: string;
  TenantId: string;
  CustomerId: string;
  ServiceId: string;
  StaffId: string;
  Date: string;
  TimeSlot: string;
  CustomerDisplayName: string;
  CreatedAt: string;
}

export interface CustomerData {
  PrincipalId: string;
  Email: string;
  DisplayName: string;
  GoogleName: string;
  CreatedAt: string;
  UpdatedAt: string;
}

export interface SlotLockOrphan {
  PK: string;
  SK: string;
  TenantId: string;
  StaffId: string;
  Date: string;
  TimeSlot: string;
  AppointmentId: string;
}

export type EntityData =
  | TenantProfileData
  | StaffMemberData
  | ServiceData
  | AppointmentData
  | CustomerData;

export interface ListItem {
  PK: string;
  SK: string;
  entityType: EntityType;
  label: string;
  data: EntityData;
}

export interface MetaResponse {
  tableName: string;
  deploymentEnvironment: string;
  region: string;
}

export interface UpsertRequest {
  aws: AwsConfig;
  entityType: EntityType;
  isNew: boolean;
  data: EntityData;
}

export interface DeleteRequest {
  aws: AwsConfig;
  entityType: EntityType;
  PK: string;
  SK: string;
  appointment?: AppointmentData;
}

export interface FieldDef {
  key: string;
  label: string;
  type: 'text' | 'number' | 'datetime';
  keyField?: boolean;
  placeholder?: string;
}

export const DEFAULT_TENANT_ID = 'sabunandsteel';
export const DEFAULT_REGION = 'eu-central-1';
export const DEFAULT_TIMEZONE = 'Europe/Amsterdam';

export const ADR_0009_ENTITIES: EntityType[] = ['TenantProfile', 'StaffMember', 'Service'];

export const ADR_0009_NOTE =
  'Customer-facing names and marketing copy live in the Angular tenant bundle (ADR-0009). This tool edits operational DynamoDB data only.';

export function entityFields(entityType: EntityType): FieldDef[] {
  switch (entityType) {
    case 'TenantProfile':
      return [
        { key: 'TenantId', label: 'Tenant ID', type: 'text', keyField: true },
        { key: 'DisplayName', label: 'Display name', type: 'text' },
        { key: 'Address', label: 'Address', type: 'text' },
        { key: 'Hours', label: 'Hours', type: 'text' },
        { key: 'Email', label: 'Email', type: 'text' },
        { key: 'Phone', label: 'Phone', type: 'text' },
        { key: 'HeroImageSrc', label: 'Hero image src', type: 'text' },
        { key: 'Timezone', label: 'Timezone', type: 'text' },
      ];
    case 'StaffMember':
      return [
        { key: 'StaffId', label: 'Staff ID', type: 'text', keyField: true },
        { key: 'TenantId', label: 'Tenant ID', type: 'text', keyField: true },
        { key: 'Name', label: 'Name', type: 'text' },
      ];
    case 'Service':
      return [
        { key: 'ServiceId', label: 'Service ID', type: 'text', keyField: true },
        { key: 'TenantId', label: 'Tenant ID', type: 'text', keyField: true },
        { key: 'DurationMinutes', label: 'Duration (minutes)', type: 'number' },
        { key: 'PriceCents', label: 'Price (cents)', type: 'number' },
      ];
    case 'Appointment':
      return [
        { key: 'AppointmentId', label: 'Appointment ID', type: 'text', keyField: true },
        { key: 'TenantId', label: 'Tenant ID', type: 'text', keyField: true },
        { key: 'CustomerId', label: 'Customer ID', type: 'text', keyField: true },
        { key: 'ServiceId', label: 'Service ID', type: 'text', keyField: true },
        { key: 'StaffId', label: 'Staff ID', type: 'text', keyField: true },
        { key: 'Date', label: 'Date', type: 'text', keyField: true, placeholder: 'YYYY-MM-DD' },
        { key: 'TimeSlot', label: 'Time slot', type: 'text', keyField: true, placeholder: 'HH:mm' },
        { key: 'CustomerDisplayName', label: 'Customer display name', type: 'text' },
        { key: 'CreatedAt', label: 'Created at', type: 'datetime', keyField: true },
      ];
    case 'Customer':
      return [
        { key: 'PrincipalId', label: 'Principal ID', type: 'text', keyField: true },
        { key: 'Email', label: 'Email', type: 'text' },
        { key: 'DisplayName', label: 'Display name', type: 'text' },
        { key: 'GoogleName', label: 'Google name', type: 'text' },
        { key: 'CreatedAt', label: 'Created at', type: 'datetime', keyField: true },
        { key: 'UpdatedAt', label: 'Updated at', type: 'datetime' },
      ];
  }
}

export function defaultEntityData(entityType: EntityType, tenantId: string): EntityData {
  const now = new Date().toISOString();
  switch (entityType) {
    case 'TenantProfile':
      return {
        TenantId: tenantId,
        DisplayName: '',
        Address: '',
        Hours: '',
        Email: '',
        Phone: '',
        HeroImageSrc: '',
        Timezone: DEFAULT_TIMEZONE,
      };
    case 'StaffMember':
      return { StaffId: '', TenantId: tenantId, Name: '' };
    case 'Service':
      return { ServiceId: '', TenantId: tenantId, DurationMinutes: 30, PriceCents: 0 };
    case 'Appointment':
      return {
        AppointmentId: crypto.randomUUID(),
        TenantId: tenantId,
        CustomerId: '',
        ServiceId: '',
        StaffId: '',
        Date: '',
        TimeSlot: '',
        CustomerDisplayName: '',
        CreatedAt: now,
      };
    case 'Customer':
      return {
        PrincipalId: '',
        Email: '',
        DisplayName: '',
        GoogleName: '',
        CreatedAt: now,
        UpdatedAt: now,
      };
  }
}

export function itemLabel(entityType: EntityType, data: EntityData): string {
  switch (entityType) {
    case 'TenantProfile':
      return (data as TenantProfileData).DisplayName || (data as TenantProfileData).TenantId;
    case 'StaffMember':
      return `${(data as StaffMemberData).Name || 'Staff'} (${(data as StaffMemberData).StaffId})`;
    case 'Service':
      return `${(data as ServiceData).ServiceId} — ${(data as ServiceData).DurationMinutes}m / ${(data as ServiceData).PriceCents}c`;
    case 'Appointment':
      return `${(data as AppointmentData).Date} ${(data as AppointmentData).TimeSlot} — ${(data as AppointmentData).CustomerDisplayName || (data as AppointmentData).AppointmentId}`;
    case 'Customer':
      return `${(data as CustomerData).DisplayName || 'Customer'} (${(data as CustomerData).PrincipalId})`;
  }
}
