/** @type {import('tailwindcss').Config} */
export default {
  content: ['./orders.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    colors: {
      white: '#fff',
      blue: '#1fb6ff',
      purple: '#7e5bef',
      pink: '#ff49db',
      orange: '#ff7849',
      red: '#ff4949',
      green: '#00ff00',
      'gray-dark': '#273444',
      gray: '#8492a6',
      'gray-light': '#dcdcde',
    },
    fontFamily: {
      sans: ['Graphik', 'sans-serif'],
      serif: ['Merriweather', 'serif'],
    },
  },
};
