import { motion } from "framer-motion";

export default function Loader() {
  return (
    <div className="flex items-center justify-center h-screen bg-gray-50">
      <motion.div
        className="w-12 h-12 border-4 border-green-500 border-t-transparent rounded-full"
        animate={{ rotate: 360 }}
        transition={{
          duration: 1,
          repeat: Infinity,
          ease: "linear",
        }}
      />
      <span className="ml-3 text-gray-600 font-medium text-lg">Loading...</span>
    </div>
  );
}
