import React, { useState, useRef, useEffect } from "react";
import { Loader2, Search } from "lucide-react";

type SearchMode = "change" | "enter";

interface Props<T> {
  id?: string;
  value: string;
  onChange: (value: string) => void;
  onSearch: (value: string) => void;
  searchMode?: SearchMode;
  suggestionsData: T[];
  placeholder?: string;
  renderItem: (item: T) => React.ReactNode;
  onSelect: (item: T) => void;
  isLoading?: boolean;
}

function GenericSearchInput<T>({
  id,
  value,
  onChange,
  onSearch,
  searchMode = "enter",
  suggestionsData,
  placeholder = "Search...",
  renderItem,
  onSelect,
  isLoading = false,
}: Props<T>) {
  const [showSuggestions, setShowSuggestions] = useState(false);
  const [activeIndex, setActiveIndex] = useState(-1); // للتنقل بالأسهم
  const debounceTimer = useRef<ReturnType<typeof setTimeout> | null>(null);
  const containerRef = useRef<HTMLDivElement>(null);

  // إغلاق القائمة عند الضغط خارج المكون
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        containerRef.current &&
        !containerRef.current.contains(event.target as Node)
      ) {
        setShowSuggestions(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    // 1. البحث عند ضغط Enter
    if (e.key === "Enter") {
      if (activeIndex >= 0 && suggestionsData[activeIndex]) {
        onSelect(suggestionsData[activeIndex]);
      } else {
        onSearch(value);
      }
      setShowSuggestions(false);
      setActiveIndex(-1);
    }

    // 2. التنقل بالأسهم (Accessibility)
    if (e.key === "ArrowDown") {
      e.preventDefault();
      setActiveIndex((prev) =>
        prev < suggestionsData.length - 1 ? prev + 1 : prev,
      );
    } else if (e.key === "ArrowUp") {
      e.preventDefault();
      setActiveIndex((prev) => (prev > 0 ? prev - 1 : 0));
    } else if (e.key === "Escape") {
      setShowSuggestions(false);
    }
  };

  const handleChange = (val: string) => {
    onChange(val);
    setActiveIndex(-1); // إعادة تعيين المؤشر عند تغيير النص

    if (searchMode === "change") onSearch(val);

    if (debounceTimer.current) clearTimeout(debounceTimer.current);

    debounceTimer.current = setTimeout(() => {
      setShowSuggestions(!!val && suggestionsData.length > 0);
    }, 400);
  };

  return (
    <div className="relative w-full" ref={containerRef}>
      <div className="relative">
        <span className="absolute inset-y-0 left-3 flex items-center text-gray-400">
          <Search size={16} aria-hidden="true" />
        </span>

        <input
          id={id}
          type="text"
          role="combobox"
          aria-expanded={showSuggestions}
          aria-haspopup="listbox"
          aria-autocomplete="list"
          aria-controls="suggestion-list"
          aria-activedescendant={
            activeIndex >= 0 ? `option-${activeIndex}` : undefined
          }
          autoComplete="off"
          value={value}
          onChange={(e) => handleChange(e.target.value)}
          onKeyDown={handleKeyDown}
          onFocus={() => value && setShowSuggestions(true)}
          placeholder={placeholder}
          className="
            w-full pl-10 pr-10 py-2.5 bg-gray-50 border border-gray-200 rounded-lg text-sm
            focus:outline-none focus:ring-2 focus:ring-blue-500 focus:bg-white focus:border-transparent
            transition-all shadow-sm
          "
        />

        {isLoading && (
          <div
            className="absolute inset-y-0 right-3 flex items-center"
            aria-hidden="true"
          >
            <Loader2 size={16} className="animate-spin text-blue-500" />
          </div>
        )}
      </div>

      {/* القائمة المنسدلة */}
      {showSuggestions && suggestionsData.length > 0 && (
        <ul
          id="suggestion-list"
          role="listbox"
          className="
            absolute w-full mt-2 bg-white border border-gray-100 rounded-xl shadow-xl z-[100] 
            max-h-64 overflow-y-auto py-1 scrollbar-thin scrollbar-thumb-gray-200
          "
        >
          {suggestionsData.map((item, index) => (
            <li
              key={index}
              id={`option-${index}`}
              role="option"
              aria-selected={index === activeIndex}
              className={`
                px-4 py-3 cursor-pointer text-sm transition-colors flex items-center justify-between
                ${index === activeIndex ? "bg-blue-50 text-blue-700" : "text-gray-700 hover:bg-gray-50"}
                ${index !== suggestionsData.length - 1 ? "border-b border-gray-50" : ""}
              `}
              onClick={() => {
                onSelect(item);
                setShowSuggestions(false);
              }}
            >
              <div className="flex-1">{renderItem(item)}</div>
              {index === activeIndex && (
                <span className="text-[10px] bg-blue-100 px-1.5 py-0.5 rounded text-blue-500 uppercase font-bold ml-2">
                  Enter
                </span>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default GenericSearchInput;
