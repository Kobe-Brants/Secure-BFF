import { ReactNode, useEffect } from 'react';
import { useAuthContext } from '../services/contexts/AuthContext.tsx';
import { useNavigate } from 'react-router-dom';

interface RouteGuardProps {
  component: ReactNode;
  isAdminOnly?: boolean;
}

export default function RouteGuard({
  component,
  isAdminOnly = false,
}: RouteGuardProps) {
  const { user, loading, isAdmin } = useAuthContext();
  const navigate = useNavigate();

  useEffect(() => {
    if (loading) return;

    if (!user) {
      navigate('/sign-in');
    } else if (isAdminOnly && !isAdmin) {
      navigate('/');
    }
  }, [loading, user, isAdmin, isAdminOnly, navigate]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (isAdminOnly && !isAdmin) {
    return null;
  }

  return <>{component}</>;
}
