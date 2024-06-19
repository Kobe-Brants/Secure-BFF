import { ReactNode, useEffect } from 'react';
import { useAuthContext } from '../services/contexts/AuthContext.tsx';
import { useNavigate } from 'react-router-dom';

interface RouteGuardProps {
  component: ReactNode;
  isAdminOnly?: boolean;
}

export default function RouteGuard({ component }: RouteGuardProps) {
  const { user, loading } = useAuthContext();
  const navigate = useNavigate();

  useEffect(() => {
    if (loading) return;

    if (!user) {
      navigate('/sign-in');
    }
  }, [loading, user, navigate]);

  if (loading) {
    return <div>Loading...</div>;
  }

  return <>{component}</>;
}
