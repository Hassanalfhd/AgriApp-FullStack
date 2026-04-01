/** @type {import('tailwindcss').Config} */
export default {
  darkMode: "class",
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        primary: "#16A34A", // أخضر رئيسي
        secondary: "#10B981", // أخضر فاتح
        accent: "#F59E0B", // برتقالي/ذهبي
        danger: "#DC2626", // أحمر
        neutral: {
          100: "#F3F4F6",
          200: "#E5E7EB",
          300: "#D1D5DB",
          400: "#9CA3AF",
          500: "#6B7280",
          600: "#4B5563",
          700: "#374151",
          800: "#1F2937",
          900: "#111827",
        },
      },
      fontFamily: {
        sans: ["Inter", "ui-sans-serif", "system-ui"],
        heading: ["Poppins", "ui-sans-serif", "system-ui"],
      },
      borderRadius: {
        xl: "1rem",
      },
      boxShadow: {
        card: "0 4px 6px rgba(0,0,0,0.1)",
        button: "0 2px 4px rgba(0,0,0,0.1)",
      },
    },
  },
  plugins: [
    require("@tailwindcss/forms"), // تحسين الفورمز
    // require("@tailwindcss/typography"), // نصوص طويلة
    // require("@tailwindcss/aspect-ratio"), // نسبة الأبعاد للصور والفيديو
  ],
};
