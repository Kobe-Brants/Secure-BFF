import { useAuthContext } from '../../services/contexts/AuthContext.tsx';

export default function SignIn() {
  const { signIn } = useAuthContext();

  const onLogin = async () => {
    await signIn();
  };

  return (
    <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
      <button
        type="button"
        onClick={onLogin}
        className="flex w-full justify-center rounded-md bg-yellow px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
      >
        Sign in
      </button>
    </div>
  );
}
