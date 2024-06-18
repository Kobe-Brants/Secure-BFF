import React, { createContext, useContext, useEffect, useState } from 'react';
import { signInWithBFF, signOutWithBFF } from '../api/authentication.ts';
import useCookie from '../hooks/useCookie.ts';
import { User } from '../../types/user.ts';

interface IAuthContext {
  user: User | null;
  loading: boolean;
  logout: () => Promise<void>;
  signIn: () => Promise<void>;
}

const AuthContext = createContext<IAuthContext>({} as IAuthContext);

const AuthContextProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [session] = useCookie<string>('session_user');

  useEffect(() => {
    if (session) {
      setUser({ name: 'todo' });
      setLoading(false);
    } else {
      setLoading(false);
    }
  }, [session]);

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
