import React from "react";

interface AddFormLayoutProps {
  title: string;
  description?: string;
  children: React.ReactNode;
  isSubmitting: boolean;
  onSubmit: (e: React.FormEvent) => void;
}

export default function AddFormLayout({
  title,
  description,
  children,
  isSubmitting,
  onSubmit,
}: AddFormLayoutProps) {
  return (
    <div
      className="
      mx-auto 
      p-4 sm:p-6 md:p-8 
      bg-white shadow-md rounded-xl
      max-w-md sm:max-w-lg md:max-w-xl lg:max-w-2xl
    "
    >
      <h1 className="text-xl font-bold mb-2">{title}</h1>
      {description && <p className="text-gray-600 mb-4">{description}</p>}

      <form onSubmit={onSubmit} className="space-y-6">
        {children}

        <button
          type="submit"
          disabled={isSubmitting}
          className={`
    w-full py-3 rounded-xl 
    font-semibold text-white
    transition-all duration-200
    ${
      isSubmitting
        ? "bg-gray-400 cursor-not-allowed"
        : "bg-green-600 hover:bg-green-700"
    }
  `}
        >
          {isSubmitting ? "Saving..." : "Save"}
        </button>
      </form>
    </div>
  );
}
