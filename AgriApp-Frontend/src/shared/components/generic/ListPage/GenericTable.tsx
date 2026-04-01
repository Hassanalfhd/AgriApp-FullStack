import { useState } from "react";
import Button from "../../ui/Button";
import ConfirmPopup from "../../ui/ConfirmPopupProps";
import type { Column } from "../types/Column";
import { Edit, Trash } from "lucide-react";
import type { IActions } from "../types/actions";

interface BaseRecord {
  id: number | string;
}

interface Props<T extends BaseRecord> {
  columns: Column<T>[];
  data: T[];
  deleteMessage?: string;
  onEdit?: (id: number | string) => void;
  onDelete?: (id: number | string) => void;
  actions?: IActions;
  emptyMessage?: string;
  renderExtraActions?: (row: T) => React.ReactNode;
}

function GenericTable<T extends BaseRecord>({
  columns,
  data,
  onEdit,
  onDelete,
  deleteMessage,
  actions,
  emptyMessage = "No data available",
  renderExtraActions,
}: Props<T>) {
  const [confirmDeleteId, setConfirmDeleteId] = useState<
    number | string | null
  >(null);

  // Dynamically calculate grid columns for desktop view
  // Adds an extra column at the end for the "Actions" section
  const gridStyle = {
    "--desktop-grid": `repeat(${columns.length}, minmax(120px, 1fr)) 140px`,
  } as React.CSSProperties;

  return (
    <div className="w-full font-sans">
      {/* Main Table Container 
        a11y: Defined as a table for screen readers
      */}
      <div
        role="table"
        aria-label="Data Table"
        className="flex flex-col gap-5 lg:gap-0 lg:bg-white lg:rounded-xl lg:shadow-md lg:border lg:border-gray-200"
        style={gridStyle}
      >
        {data.length > 0 ? (
          <>
            {/* Table Header (Visible only on Desktop) 
              a11y: rowgroup and columnheader roles
            */}
            <div
              role="rowgroup"
              className="hidden lg:block bg-gray-50 text-gray-700 rounded-t-xl border-b border-gray-200"
            >
              <div
                role="row"
                className="grid gap-4 p-4 items-center font-semibold text-left lg:[grid-template-columns:var(--desktop-grid)]"
              >
                {columns.map((col) => (
                  <div
                    role="columnheader"
                    key={String(col.key)}
                    className="px-2 uppercase tracking-wider text-xs"
                    style={{ width: col.width }}
                  >
                    {col.label}
                  </div>
                ))}
                <div
                  role="columnheader"
                  className="px-2 text-center uppercase tracking-wider text-xs"
                >
                  Actions
                </div>
              </div>
            </div>

            {/* Table Body */}
            <div
              role="rowgroup"
              className="flex flex-col gap-4 lg:gap-0 lg:divide-y lg:divide-gray-100"
            >
              {data.map((row) => (
                <div
                  role="row"
                  key={row.id}
                  // a11y: Make rows focusable for keyboard navigation
                  tabIndex={0}
                  className="
                    flex flex-col p-5 bg-white rounded-xl shadow-md border border-gray-100 
                    lg:grid lg:p-4 lg:items-center lg:[grid-template-columns:var(--desktop-grid)]
                    lg:shadow-none lg:rounded-none lg:border-none lg:hover:bg-blue-50/50 transition-colors 
                    focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600 focus-visible:bg-blue-50
                  "
                >
                  {/* Data Cells */}
                  {columns.map((col) => (
                    <div
                      role="cell"
                      key={String(col.key)}
                      className="
                        flex justify-between items-center py-2.5 border-b border-gray-100 last:border-none
                        lg:block lg:py-0 lg:border-none lg:px-2 text-sm text-gray-800
                      "
                    >
                      {/* Mobile Label: Hidden on desktop, visible on mobile as a clear key/value layout */}
                      <span
                        className="lg:hidden text-gray-500 font-medium text-xs uppercase tracking-wide"
                        aria-hidden="true"
                      >
                        {col.label}
                      </span>
                      <span className="font-semibold text-gray-900 lg:font-normal lg:text-gray-800 text-right lg:text-left break-words max-w-[60%] lg:max-w-full">
                        {col.render ? col.render(row) : (row as any)[col.key]}
                      </span>
                    </div>
                  ))}

                  {/* Actions Cell */}
                  <div
                    role="cell"
                    className="flex flex-wrap items-center justify-end lg:justify-center gap-2 mt-4 pt-4 border-t border-gray-100 lg:border-0 lg:pt-0 lg:mt-0 px-2"
                  >
                    {renderExtraActions && renderExtraActions(row)}

                    {actions?.showEdit && onEdit && (
                      <Button
                        aria-label={`Edit item ${row.id}`} // a11y: Clear label for screen readers
                        title="Edit"
                        className="p-2 bg-green-600 hover:bg-green-700 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-green-800 text-white rounded-lg transition-all shadow-sm flex items-center justify-center"
                        onClick={() => onEdit(row.id)}
                      >
                        <Edit size={16} aria-hidden="true" />
                      </Button>
                    )}

                    {actions?.showDelete && (
                      <Button
                        aria-label={`Delete item ${row.id}`} // a11y: Clear label for screen readers
                        title="Delete"
                        className="p-2 bg-red-600 hover:bg-red-700 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-red-800 text-white rounded-lg transition-all shadow-sm flex items-center justify-center"
                        onClick={() => setConfirmDeleteId(row.id)}
                      >
                        <Trash size={16} aria-hidden="true" />
                      </Button>
                    )}
                  </div>
                </div>
              ))}
            </div>
          </>
        ) : (
          /* Empty State */
          <div
            role="status"
            aria-live="polite" // a11y: Announces empty state changes to screen readers automatically
            className="w-full p-12 text-center text-gray-500 font-medium bg-white rounded-xl shadow-sm border border-gray-200"
          >
            {emptyMessage}
          </div>
        )}
      </div>

      {/* Confirmation Popup */}
      <ConfirmPopup
        isOpen={confirmDeleteId !== null}
        title="Confirm Deletion"
        message={
          deleteMessage ??
          "Are you sure you want to delete this item? This action cannot be undone."
        }
        onCancel={() => setConfirmDeleteId(null)}
        onConfirm={() => {
          if (confirmDeleteId !== null && onDelete) {
            onDelete(confirmDeleteId);
            setConfirmDeleteId(null);
          }
        }}
      />
    </div>
  );
}

export default GenericTable;
