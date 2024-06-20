import { useState } from 'react';

type UseCookieReturnType<T> = [
  T | undefined,
  (value: T, options?: { expires?: number; path?: string }) => void,
  () => void
];

function useCookie<T>(cookieName: string): UseCookieReturnType<T> {
  const getCookie = (name: string): T | undefined => {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) {
      const cookieValue = parts.pop()!.split(';').shift()!;
      try {
        return JSON.parse(decodeURIComponent(cookieValue));
      } catch (error) {
        console.error(`Error parsing cookie '${name}':`, error);
        return undefined;
      }
    }
    return undefined;
  };

  const setCookie = (
    value: T,
    options?: { expires?: number; path?: string }
  ): void => {
    let cookieString = `${cookieName}=${JSON.stringify(value)}`;
    if (options?.expires) {
      const date = new Date();
      date.setTime(date.getTime() + options.expires * 24 * 60 * 60 * 1000);
      cookieString += `; expires=${date.toUTCString()}`;
    }
    if (options?.path) {
      cookieString += `; path=${options.path}`;
    }
    document.cookie = cookieString;
  };

  const [cookieValue, setCookieValue] = useState<T | undefined>(
    getCookie(cookieName)
  );

  const deleteCookie = (): void => {
    setCookieValue(undefined);
    document.cookie = `${cookieName}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
  };

  const updateCookie = (
    value: T,
    options?: { expires?: number; path?: string }
  ): void => {
    setCookieValue(value);
    setCookie(value, options);
  };

  return [cookieValue, updateCookie, deleteCookie];
}

export default useCookie;
