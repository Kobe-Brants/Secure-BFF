import React, { createContext, useContext, useEffect, useState } from 'react';
import { signInWithBFF } from '../api/authentication.ts';

interface IAuthContext {
  user: any | null;
  loading: boolean;
  isAdmin: boolean;
  logout: () => Promise<void>;
  signIn: () => Promise<void>;
}

const AuthContext = createContext<IAuthContext>({} as IAuthContext);

const AuthContextProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<any | null>(null);
  const [isAdmin, setIsAdmin] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Todo
    setUser(null);
    setLoading(false);
    setIsAdmin(false);
  }, []);

  const logout = async () => {
    // await auth.signOut();
  };

  const signIn = async () => {
    await signInWithBFF();
  };

  return (
    <AuthContext.Provider value={{ user, loading, isAdmin, logout, signIn }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuthContext = () => {
  const { user, loading, isAdmin, logout, signIn } = useContext(AuthContext);

  return {
    user,
    loading,
    isAdmin,
    logout,
    signIn,
  };
};

export { AuthContextProvider, AuthContext };
