// vite.config.mjs
import react from '@vitejs/plugin-react';
import autoprefixer from 'autoprefixer';
import tailwindcss from 'tailwindcss';
import { defineConfig } from 'vite';

export default defineConfig({
  plugins: [react()],
  css: {
    postcss: {
      plugins: [tailwindcss(), autoprefixer()],
    },
  },
  build: {
    outDir: '../public/react/build',
    emptyOutDir: true,
    sourcemap: true,  // 本番ビルドでもソースマップを生成
    lib: {
      entry: 'src/main.jsx',
      name: 'FormLib',
      fileName: 'form',
      formats: ['iife'],
    },
  },
  define: {
    'process.env': {},
  },
});
