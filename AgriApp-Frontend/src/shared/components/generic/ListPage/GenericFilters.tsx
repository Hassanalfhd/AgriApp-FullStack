import React, { useId } from "react";
import type { FilterField } from "../types/FilterField";
import GenericSearchInput from "./GenericSearchInput";
import { Search, RotateCcw } from "lucide-react"; // Optional icons for better UX

interface Props {
  filters: FilterField[];
  values: Record<string, any>;
  onChange: (key: string, value: any) => void;
  onSearchClick: () => void;
  onReset: () => void;
  onSearch: (key: string, value: any) => void;
}

const GenericFilters: React.FC<Props> = ({
  filters,
  values,
  onChange,
  onSearchClick,
  onReset,
  onSearch,
}) => {
  const baseId = useId(); // Generate unique IDs for accessibility

  return (
    <section
      role="search"
      aria-label="Data Filters"
      className="max-w-[1600px] mx-auto p-4 bg-white rounded-xl shadow-sm border border-gray-100 mb-6"
    >
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3  gap-6">
        {filters.map((filter, index) => {
          const inputId = `${baseId}-${filter.key}-${index}`;

          return (
            <div key={filter.key} className="flex flex-col gap-1.5">
              <label
                htmlFor={inputId}
                className="text-sm font-semibold text-gray-700 ml-1"
              >
                {filter.label}
              </label>

              {filter.type === "text" ? (
                <div className="focus-within:ring-2 focus-within:ring-blue-500 rounded-lg transition-shadow">
                  <GenericSearchInput
                    id={inputId} // Pass ID to the internal input
                    value={values[filter.key] || ""}
                    onChange={(val) => onChange(filter.key, val)}
                    onSearch={(val) => onSearch(filter.key, val)}
                    searchMode={filter.searchMode ?? "enter"}
                    suggestionsData={filter.suggestions || []}
                    isLoading={filter.isLoading}
                    placeholder={
                      filter.placeholder || `Search ${filter.label}...`
                    }
                    renderItem={(item: any) => (
                      <div className="flex flex-col py-1">
                        <span className="text-sm">
                          {typeof item === "string"
                            ? item
                            : item.name || item.title || item.label}
                        </span>
                      </div>
                    )}
                    onSelect={(item: any) => {
                      const selectedValue =
                        typeof item === "string"
                          ? item
                          : item.name || item.title;
                      onChange(filter.key, selectedValue);
                      onSearch(filter.key, selectedValue);
                    }}
                  />
                </div>
              ) : filter.type === "number" ? (
                <input
                  id={inputId}
                  type="number"
                  className="
                    w-full px-3 py-2 bg-gray-50 border border-gray-200 rounded-lg text-sm
                    focus:outline-none focus:ring-2 focus:ring-blue-500 focus:bg-white
                    transition-all placeholder:text-gray-400
                  "
                  value={values[filter.key] || ""}
                  placeholder={filter.placeholder || "Enter number..."}
                  onChange={(e) =>
                    onChange(
                      filter.key,
                      e.target.value ? Number(e.target.value) : "",
                    )
                  }
                />
              ) : null}
            </div>
          );
        })}
      </div>

      {/* Action Buttons */}
      <div className="mt-6 pt-4 border-t border-gray-50 flex flex-col sm:flex-row justify-end gap-3">
        <button
          type="button"
          onClick={onReset}
          className="
            inline-flex items-center justify-center gap-2 px-5 py-2.5 
            text-sm font-medium text-gray-600 bg-gray-100 rounded-lg
            hover:bg-gray-200 focus:ring-4 focus:ring-gray-100 outline-none
            transition-colors order-2 sm:order-1
          "
          aria-label="Reset all filters"
        >
          <RotateCcw size={16} />
          Clear Filters
        </button>

        <button
          type="button"
          onClick={onSearchClick}
          className="
            inline-flex items-center justify-center gap-2 px-8 py-2.5 
            text-sm font-medium text-white bg-blue-600 rounded-lg
            hover:bg-blue-700 focus:ring-4 focus:ring-blue-200 outline-none
            transition-all shadow-sm shadow-blue-200 order-1 sm:order-2
          "
          aria-label="Apply filters"
        >
          <Search size={16} />
          Search
        </button>
      </div>
    </section>
  );
};

export default GenericFilters;
