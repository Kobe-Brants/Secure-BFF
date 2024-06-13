import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import mkcert from 'vite-plugin-mkcert';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), mkcert()],
  server: {
    port: 4000,
    proxy: {
      '/api': {
        target: 'https://localhost:7207',
        changeOrigin: true,
        secure: false, //allow self-assigned certificates
        // rewrite: (path) => path.replace(/^\/api/, '')
      },
    },
  },
})
