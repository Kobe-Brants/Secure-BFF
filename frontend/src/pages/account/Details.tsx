import { useAuthContext } from '../../services/contexts/AuthContext.tsx';

export default function Details() {
  const { user } = useAuthContext();

  return <pre>{JSON.stringify(user, null, 2)}</pre>;
}
