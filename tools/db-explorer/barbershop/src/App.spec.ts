import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { flushPromises, mount } from '@vue/test-utils';
import App from './App.vue';

function jsonResponse(body: unknown) {
  return Promise.resolve({
    ok: true,
    json: () => Promise.resolve(body),
  } as Response);
}

describe('App', () => {
  beforeEach(() => {
    vi.stubGlobal(
      'fetch',
      vi.fn((url: string) => {
        if (url.includes('/api/meta')) {
          return jsonResponse({
            tableName: 'Kapsalon-Test',
            deploymentEnvironment: 'Development',
            region: 'eu-central-1',
          });
        }
        if (url.includes('entityType=StaffMember')) {
          return jsonResponse({
            items: [
              {
                PK: 'TENANT#sabunandsteel',
                SK: 'STAFF#andre',
                entityType: 'StaffMember',
                label: 'Andre (andre)',
                data: { StaffId: 'andre', TenantId: 'sabunandsteel', Name: 'Andre' },
              },
            ],
          });
        }
        return jsonResponse({ items: [] });
      }),
    );
  });

  afterEach(() => {
    vi.unstubAllGlobals();
  });

  it('populates the form with the selected staff member\'s real data, not stale defaults', async () => {
    const wrapper = mount(App);
    await flushPromises();

    const staffButton = wrapper.findAll('.list-items button').find((b) => b.text().includes('Andre'));
    expect(staffButton).toBeTruthy();
    await staffButton!.trigger('click');
    await flushPromises();

    const nameInput = wrapper
      .findAll('.form-grid label')
      .find((l) => l.find('span').text() === 'Name')!
      .find('input');
    const staffIdInput = wrapper
      .findAll('.form-grid label')
      .find((l) => l.find('span').text() === 'Staff ID')!
      .find('input');

    expect(staffIdInput.element.value).toBe('andre');
    expect(nameInput.element.value).toBe('Andre');
  });
});
