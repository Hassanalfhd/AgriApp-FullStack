import { ChevronLeft, ChevronRight } from "lucide-react";

interface PaginationProps {
  current: number;
  total: number;
  onPageChange: (page: number) => void;
}

const Pagination = ({ current, total, onPageChange }: PaginationProps) => {
  if (total <= 1) return null;

  // Calculate the range of pages to show around the current page
  const getPages = () => {
    const pages = [];
    const startPage = Math.max(1, current - 2);
    const endPage = Math.min(total, current + 2);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  };

  const btnBaseClass =
    "flex items-center justify-center px-4 py-2 text-sm font-medium border rounded-lg transition-all focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1";
  const activeClass = "bg-blue-600 text-white border-blue-600 shadow-sm";
  const inactiveClass =
    "bg-white text-gray-700 border-gray-300 hover:bg-gray-50 hover:border-gray-400";
  const disabledClass =
    "opacity-50 cursor-not-allowed bg-gray-100 text-gray-400 border-gray-200";

  return (
    <nav
      role="navigation"
      aria-label="Pagination Navigation"
      className="flex justify-center items-center gap-2 mt-8 mb-12 flex-wrap"
    >
      {/* Previous Button */}
      <button
        disabled={current === 1}
        onClick={() => onPageChange(current - 1)}
        aria-label="Go to previous page"
        className={`${btnBaseClass} ${current === 1 ? disabledClass : inactiveClass}`}
      >
        <ChevronLeft size={18} className="mr-1" aria-hidden="true" />
        Previous
      </button>

      {/* First Page and Ellipsis */}
      {current > 3 && (
        <>
          <button
            onClick={() => onPageChange(1)}
            aria-label="Go to page 1"
            className={`${btnBaseClass} ${inactiveClass}`}
          >
            1
          </button>
          {current > 4 && (
            <span className="px-2 text-gray-400" aria-hidden="true">
              ...
            </span>
          )}
        </>
      )}

      {/* Page Numbers */}
      {getPages().map((page) => (
        <button
          key={page}
          onClick={() => onPageChange(page)}
          // a11y: aria-current is essential for screen readers to know which page is active
          aria-current={current === page ? "page" : undefined}
          aria-label={`Go to page ${page}`}
          className={`${btnBaseClass} ${
            current === page ? activeClass : inactiveClass
          }`}
        >
          {page}
        </button>
      ))}

      {/* Last Page and Ellipsis */}
      {current < total - 2 && (
        <>
          {current < total - 3 && (
            <span className="px-2 text-gray-400" aria-hidden="true">
              ...
            </span>
          )}
          <button
            onClick={() => onPageChange(total)}
            aria-label={`Go to page ${total}`}
            className={`${btnBaseClass} ${inactiveClass}`}
          >
            {total}
          </button>
        </>
      )}

      {/* Next Button */}
      <button
        disabled={current === total}
        onClick={() => onPageChange(current + 1)}
        aria-label="Go to next page"
        className={`${btnBaseClass} ${current === total ? disabledClass : inactiveClass}`}
      >
        Next
        <ChevronRight size={18} className="ml-1" aria-hidden="true" />
      </button>
    </nav>
  );
};

export default Pagination;
