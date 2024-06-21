import React, { createContext, useContext, useEffect, useState } from 'react';
import { SessionUser } from '../../types/SessionUser.ts';
import {
  useMutationSignIn,
  useMutationSignOut,
  useQuerySessionUser,
} from '../api/authentication.ts';

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
  const [user, setUser] = useState<SessionUser | undefined>(undefined);
  const [loading, setLoading] = useState(true);
  const { data: sessionUser } = useQuerySessionUser();

  useEffect(() => {
    if (sessionUser) {
      setUser(sessionUser);
      setLoading(false);
    } else {
      setUser(undefined);
      setLoading(false);
    }
  }, [sessionUser, user]);

  const logout = async () => {
    setLoading(true);
    await signOutWithBFF()
      .then(() => {
        setUser(undefined);
      })
      .finally(() => setLoading(false));
  };

  const signIn = async () => {
    await signInWithBFF();
  };

  return (
    <AuthContext.Provider value={{ user, loading, logout, signIn }}>
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
