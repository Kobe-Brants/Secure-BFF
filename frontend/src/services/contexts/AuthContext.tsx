import React, { createContext, useContext, useEffect, useState } from 'react';
import { signInWithBFF, signOutWithBFF } from '../api/authentication.ts';
import useCookie from '../hooks/useCookie.ts';
import { SessionUser } from '../../types/SessionUser.ts';

interface IAuthContext {
  user: SessionUser | undefined;
  loading: boolean;
  logout: () => Promise<void>;
  signIn: () => Promise<void>;
}

const AuthContext = createContext<IAuthContext>({} as IAuthContext);

const AuthContextProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<SessionUser | undefined>(undefined);
  const [loading, setLoading] = useState(true);
  const [session] = useCookie<SessionUser | undefined>('session_user');

  useEffect(() => {
    if (session) {
      setUser(session);
      setLoading(false);
    } else {
      setLoading(false);
    }
  }, [session, user]);

  const logout = async () => {
    await signOutWithBFF();
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
