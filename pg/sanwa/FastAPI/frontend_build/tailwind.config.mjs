// tailwind.config.mjs
export default {
  important: true,
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
    "./public/index.html", // 必要に応じて追加
  ],
  safelist: [
    'text-red-600',
    'text-blue-600',
    'text-green-600',
    'text-yellow-600',
    // 必要なクラスを追加
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          light: '#3b82f6',
          DEFAULT: '#1d4ed8',
          dark: '#1e40af',
        },
        secondary: {
          light: '#f472b6',
          DEFAULT: '#ec4899',
          dark: '#db2777',
        },
      },
      fontFamily: {
        sans: ['Helvetica', 'Arial', 'sans-serif'],
        serif: ['Georgia', 'serif'],
      },
    },
  },
  plugins: [],
};
