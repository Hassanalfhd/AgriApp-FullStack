import { Toaster } from "react-hot-toast";

export default function Toast() {
  return (
    <Toaster
      position="top-right"
      toastOptions={{
        // Default options
        duration: 4000,
        style: {
          background: "#333",
          color: "#fff",
          fontWeight: "500",
        },
        success: {
          duration: 3000,
          style: {
            background: "green",
            color: "#fff",
          },
          icon: "✅",
        },
        error: {
          duration: 5000,
          style: {
            background: "red",
            color: "#fff",
          },
          icon: "❌",
        },
      }}
    />
  );
}
