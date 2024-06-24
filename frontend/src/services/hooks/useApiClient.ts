import { createFetch, FetchContext, FetchResponse, ResponseType } from 'ofetch';
import { useAuthContext } from '../contexts/AuthContext.tsx';

export default function useApiClient({ useOnResponseError = true } = {}) {
  const { logout } = useAuthContext();

  return createFetch({
    defaults: {
      baseURL: import.meta.env.VITE_APP_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
        'x-csrf-header': '',
      },
      onResponseError(
        context: FetchContext & { response: FetchResponse<ResponseType> }
      ): Promise<void> | void {
        if (context.response.status === 401 && useOnResponseError) {
          logout().then();
        }
      },
    },
  });
}
