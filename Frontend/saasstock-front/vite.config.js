import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'         //importo tailwind

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(),
    tailwindcss(),],                              //se lo sumo a los plugins
})
