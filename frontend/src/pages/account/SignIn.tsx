import { useAuthContext } from '../../services/contexts/AuthContext.tsx';

export default function SignIn() {
  const { signIn, user, logout } = useAuthContext();

  if (user)
    return (
      <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
        <button type="button" onClick={logout} className="btn-danger">
          Sign out
        </button>
      </div>
    );

  return (
    <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
      <button type="button" onClick={signIn} className="btn-primary">
        Sign in
      </button>
    </div>
  );
}
