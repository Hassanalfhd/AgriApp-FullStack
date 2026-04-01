import clsx from "clsx";
import React from "react";
export default function Button({
  children,
  className,
  ...props
}: React.ButtonHTMLAttributes<HTMLButtonElement>) {
  return (
    <button
      
      {...props}
      className={clsx(
        "py-3 px-4 rounded-xl font-semibold transition disabled:opacity-70   text-white hover:bg-green-700",
        className
      )}
    >
      {children}
    </button>
  );
}
