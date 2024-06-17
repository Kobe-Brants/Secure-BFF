import apiClient from '../../helpers/apiClient.ts';

export const signInWithBFF = async () => {
  try {
    const redirectUrl = await apiClient<string>('/authentication/sign-in', {
      method: 'GET',
    });
    // window.open(redirectUrl, '_blank');
  } catch (error) {
    console.error('Error during sign-in:', error);
  }
};
