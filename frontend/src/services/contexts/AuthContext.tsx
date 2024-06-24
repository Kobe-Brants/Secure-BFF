import React, { createContext, useContext } from 'react';
import { SessionUser } from '../../types/SessionUser.ts';
import {
  useMutationSignIn,
  useMutationSignOut,
  useQuerySessionUser,
} from '../api/authentication.ts';
import { useNavigate } from 'react-router-dom';

interface IAuthContext {
  user: SessionUser | undefined;
  loading: boolean;
  logout: () => Promise<void>;
  signIn: () => Promise<void>;
}

const AuthContext = createContext<IAuthContext>({} as IAuthContext);

const AuthContextProvider = ({ children }: { children: React.ReactNode }) => {
  const { mutateAsync: signInWithBFF } = useMutationSignIn();
  const { mutateAsync: signOutWithBFF } = useMutationSignOut();
  const { data: user, isLoading, refetch } = useQuerySessionUser();
  const navigate = useNavigate();

  const logout = async () => {
    await signOutWithBFF().then(() => {
      refetch();
      navigate('/sign-in');
    });
  };

  const signIn = async () => {
    await signInWithBFF().then(() => refetch());
    navigate('/');
  };

  return (
    <AuthContext.Provider value={{ user, loading: isLoading, logout, signIn }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuthContext = () => {
  const { user, loading, logout, signIn } = useContext(AuthContext);

  return {
    user,
    loading,
    logout,
    signIn,
  };
};

export { AuthContextProvider, AuthContext };
