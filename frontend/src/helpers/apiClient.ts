import { createFetch } from 'ofetch';

const apiClient = createFetch({
  defaults: {
    baseURL: import.meta.env.VITE_APP_BASE_URL,
    headers: {
      'Content-Type': 'application/json',
    },
  },
});

export default apiClient;
