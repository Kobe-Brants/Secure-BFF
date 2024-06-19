import { useQueryUsers } from '../../services/api/users.ts';

export default function UsersList() {
  const { data: users } = useQueryUsers();
  return <pre>{JSON.stringify(users, null, 2)}</pre>;
}
