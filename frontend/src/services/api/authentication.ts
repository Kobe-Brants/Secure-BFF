import apiClient from '../../helpers/apiClient.ts';

export const signInWithBFF = async () => {
  try {
    const redirectUrl = await apiClient<string>('/authentication/sign-in', {
      method: 'GET',
    });
    window.open(redirectUrl, '_self');
  } catch (error) {
    console.error('Error during sign-in:', error);
  }
};

export const signOutWithBFF = async () => {
  try {
    await apiClient<string>('/authentication/sign-out', {
      method: 'GET',
    });
  } catch (error) {
    console.error('Error during sign-in:', error);
  }
};
