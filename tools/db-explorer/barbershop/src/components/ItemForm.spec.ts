import { describe, expect, it } from 'vitest';
import { mount } from '@vue/test-utils';
import ItemForm from './ItemForm.vue';
import type { AppointmentData, CustomerData, StaffMemberData } from '../../shared/types';

function inputFor(wrapper: ReturnType<typeof mount>, label: string) {
  const target = wrapper
    .findAll('label')
    .find((candidate) => candidate.find('span').text() === label);
  if (!target) {
    throw new Error(`No field labeled "${label}" was rendered`);
  }
  return target.find('input');
}

describe('ItemForm', () => {
  it('populates CustomerId, ServiceId and StaffId for an existing appointment', () => {
    const formData: AppointmentData = {
      AppointmentId: 'appt-1',
      TenantId: 'sabunandsteel',
      CustomerId: 'cust-1',
      ServiceId: 'cut',
      StaffId: 'marcus',
      Date: '2026-07-25',
      TimeSlot: '09:00',
      CustomerDisplayName: 'Jane Doe',
      CreatedAt: '2026-07-01T00:00:00.000Z',
    };

    const wrapper = mount(ItemForm, {
      props: { entityType: 'Appointment', formData, isNew: false, saving: false },
    });

    expect(inputFor(wrapper, 'Customer ID').element.value).toBe('cust-1');
    expect(inputFor(wrapper, 'Service ID').element.value).toBe('cut');
    expect(inputFor(wrapper, 'Staff ID').element.value).toBe('marcus');
  });

  it('populates Name for an existing staff member', () => {
    const formData: StaffMemberData = {
      StaffId: 'marcus',
      TenantId: 'sabunandsteel',
      Name: 'Marcus',
    };

    const wrapper = mount(ItemForm, {
      props: { entityType: 'StaffMember', formData, isNew: false, saving: false },
    });

    expect(inputFor(wrapper, 'Name').element.value).toBe('Marcus');
  });

  it('populates DisplayName for an existing customer', () => {
    const formData: CustomerData = {
      PrincipalId: 'principal-1',
      Email: 'jane@example.com',
      DisplayName: 'Jane Doe',
      GoogleName: 'Jane G',
      CreatedAt: '2026-01-01T00:00:00.000Z',
      UpdatedAt: '2026-01-02T00:00:00.000Z',
    };

    const wrapper = mount(ItemForm, {
      props: { entityType: 'Customer', formData, isNew: false, saving: false },
    });

    expect(inputFor(wrapper, 'Display name').element.value).toBe('Jane Doe');
  });
});
