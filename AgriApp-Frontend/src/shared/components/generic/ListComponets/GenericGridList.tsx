import React from "react";
import { type Breakpoints } from "../../../types/styleTypes/style";

interface GridListProps<T> {
  items: T[];
  renderItem: (item: T) => React.ReactNode;
  getKey?: (item: T) => string | number;
  cols?: Breakpoints;
  emptyMessage?: string;
  className?: string;
  gap?: number | string; // Use numbers for Tailwind spacing scale
}

export default function GenericGridList<T>({
  items,
  renderItem,
  getKey = (item: any) => item.id,
  cols = { base: 1, sm: 2, md: 3, lg: 4, xl: 6 },
  emptyMessage = "No data available to display.",
  className = "",
  gap = "6",
}: GridListProps<T>) {
  // a11y: Using a list role helps screen readers announce the number of items
  if (!items || items.length === 0) {
    return (
      <div
        role="status"
        aria-live="polite"
        className="w-full text-center text-gray-500 py-16 bg-gray-50 rounded-xl border-2 border-dashed border-gray-200"
      >
        <p className="text-lg font-medium">{emptyMessage}</p>
      </div>
    );
  }

  // Refined Grid Logic: Using CSS Variables or standard Tailwind classes
  // Note: Standard Tailwind classes are safer than string interpolation for the compiler
  const gridClasses = `
    grid grid-cols-${cols.base || 1}
    ${cols.sm ? `sm:grid-cols-${cols.sm}` : "sm:grid-cols-2"}
    ${cols.md ? `md:grid-cols-${cols.md}` : "md:grid-cols-3"}
    ${cols.lg ? `lg:grid-cols-${cols.lg}` : "lg:grid-cols-4"}
    ${cols.xl ? `xl:grid-cols-${cols.xl}` : "xl:grid-cols-6"}
    gap-${gap}
  `
    .replace(/\s+/g, " ")
    .trim();

  return (
    <ul
      role="list"
      aria-label="Items grid"
      className={`${gridClasses} ${className}`}
    >
      {items.map((item, index) => (
        <li
          key={getKey(item) ?? index}
          className="list-none focus-within:ring-2 focus-within:ring-blue-500 rounded-lg transition-shadow"
          role="listitem"
        >
          {renderItem(item)}
        </li>
      ))}
    </ul>
  );
}
