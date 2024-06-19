import apiClient from '../../helpers/apiClient.ts';
import { useQuery } from 'react-query';
import { QueryOpt } from '../../types/api.ts';
import { User } from '../../types/user.ts';

export const usersKeys = {
  all: ['users'],
};

export const getUsers = async () => {
  const result = await apiClient<User[]>('/users', {
    method: 'GET',
  });
  return result || undefined;
};

export const useQueryUsers = (options?: QueryOpt<User[] | undefined>) => {
  return useQuery(usersKeys.all, () => getUsers(), options);
};
