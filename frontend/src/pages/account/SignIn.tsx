import { useAuthContext } from '../../services/contexts/AuthContext.tsx';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export default function SignIn() {
  const { signIn, user, loading } = useAuthContext();
  const navigate = useNavigate();

  useEffect(() => {
    if (user && !loading) {
      navigate(`/`);
    }
  }, [navigate, user]);

  return (
    <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
      <button type="button" onClick={signIn} className="btn-primary">
        Sign in
      </button>
    </div>
  );
}
