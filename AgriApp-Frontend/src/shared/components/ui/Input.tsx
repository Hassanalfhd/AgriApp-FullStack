import React from "react";
interface IProps extends React.InputHTMLAttributes<HTMLInputElement> {
  id?: string;
  label?: string;
  error?: string | null;
}
export function Input({ label, error, ...props }: IProps) {
  return (
    <div className="space-y-1">
      {label && (
        <label htmlFor={props.id} className="block text-sm text-gray-700">
          {label}
        </label>
      )}
      <input
        {...props}
        className={`w-full p-3 border rounded-xl focus:ring-2 focus:ring-green-400 ${
          error ? "border-red-500" : "border-gray-300"
        }`}
      />
      {error && <p className="text-red-500 text-xs">{error}</p>}
    </div>
  );
}
