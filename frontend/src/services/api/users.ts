import { useQuery } from 'react-query';
import { QueryOpt } from '../../types/api.ts';
import { User } from '../../types/user.ts';
import { $Fetch } from 'ofetch';
import UseApiClient from '../hooks/useApiClient.ts';

export const usersKeys = {
  all: ['users'],
};

export const getUsers = async (apiClient: $Fetch) => {
  const result = await apiClient<User[]>('/users', {
    method: 'GET',
  });
  return result || undefined;
};

export const useQueryUsers = (options?: QueryOpt<User[] | undefined>) => {
  const apiClient = UseApiClient();

  return useQuery(usersKeys.all, () => getUsers(apiClient), options);
};
