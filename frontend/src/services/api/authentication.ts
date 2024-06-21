import UseApiClient from '../hooks/useApiClient.ts';
import { useMutation, UseMutationOptions, useQuery } from 'react-query';
import { $Fetch } from 'ofetch';
import { QueryOpt } from '../../types/api.ts';
import { SessionUser } from '../../types/SessionUser.ts';

const signInWithBFF = async (apiClient: $Fetch) => {
  const redirectUrl = await apiClient<string>('/authentication/sign-in', {
    method: 'GET',
  });
  window.open(redirectUrl, '_self');
};

export function useMutationSignIn(
  options?: UseMutationOptions<void, Error> | undefined
) {
  const apiClient = UseApiClient();

  return useMutation<void, Error>(() => signInWithBFF(apiClient), options);
}

const signOutWithBFF = async (apiClient: $Fetch) => {
  await apiClient('/authentication/sign-out', {
    method: 'GET',
  });
};

export function useMutationSignOut(
  options?: UseMutationOptions<void, Error> | undefined
) {
  const apiClient = UseApiClient();

  return useMutation<void, Error>(() => signOutWithBFF(apiClient), options);
}

export const authenticationKeys = {
  me: ['me'],
};

export const getSessionUser = async (apiClient: $Fetch) => {
  try {
    const result = await apiClient<SessionUser>('/authentication/me', {
      method: 'GET',
    });
    return result || undefined;
  } catch (error) {
    return undefined;
  }
};

export const useQuerySessionUser = (
  options?: QueryOpt<SessionUser | undefined>
) => {
  const apiClient = UseApiClient();

  return useQuery(
    authenticationKeys.me,
    () => getSessionUser(apiClient),
    options
  );
};
