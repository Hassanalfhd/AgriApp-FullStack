import React from "react";
import GenericFilters from "./GenericFilters";
import GenericTable from "./GenericTable";
import type { Column } from "../types/Column";
import type { FilterField } from "../types/FilterField";
import type { IActions } from "../types/actions";
import Pagination from "../ListComponets/Pagination";

interface BaseRecord {
  id: number | string;
}

interface Props<T extends BaseRecord> {
  // ... (نفس الـ Props السابقة)
  title: string;
  columns: Column<T>[];
  filters?: FilterField[];
  data: T[];
  actions?: IActions;
  deleteMessage: string;
  onEdit: (id: number | string) => void;
  onDeleteAction: (id: number | string) => void;
  filterState: Record<string, any>;
  onFilterChangeAction: (key: string, value: any) => void;
  onSearchAction: () => void;
  onResetAction: () => void;
  emptyMessage?: string;
  renderExtraActions?: (row: T) => React.ReactNode;
  currentPage: number;
  totalPages: number;
  handlePageChange: (newPage: number) => void;
  isLoading?: boolean;
}

function GenericListPage<T extends BaseRecord>({
  title,
  columns,
  filters,
  data,
  filterState,
  deleteMessage,
  actions,
  onFilterChangeAction,
  onSearchAction,
  onResetAction,
  onDeleteAction,
  onEdit,
  emptyMessage,
  renderExtraActions,
  currentPage,
  handlePageChange,
  totalPages,
  isLoading = false,
}: Props<T>) {
  return (
    /** * تحسين 1: استخدام w-full مع max-w-screen-2xl لضمان عدم الخروج عن الإطار.
     * إضافة overflow-x-hidden للحاوية الأم لمنع أي سكرول ناتج عن أخطاء بسيطة في العرض.
     */
    <main className=" max-w-[1600px] mx-auto p-4 md:p-6 lg:p-8 min-h-screen font-sans overflow-x-hidden">
      <header className="mb-8 flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h1 className="text-2xl md:text-3xl font-extrabold text-gray-900 tracking-tight">
            {title}
          </h1>
          <p className="mt-1 text-sm text-gray-500">
            Manage your {title.toLowerCase()} records efficiently.
          </p>
        </div>
      </header>

      {/* قسم الفلاتر: التأكد من أنه لا يضغط المحتوى أفقياً */}
      {filters && (
        <section className="mb-8 bg-white p-4 rounded-xl shadow-sm border border-gray-100">
          <GenericFilters
            filters={filters}
            values={filterState}
            onChange={onFilterChangeAction}
            onSearchClick={onSearchAction}
            onReset={onResetAction}
            onSearch={(key, val) => {
              onFilterChangeAction(key, val);
              onSearchAction();
            }}
          />
        </section>
      )}

      {/* تحسين 2: قسم الجدول. 
          استخدام container بخصائص flex-1 لضمان تمدد الجدول لملء المساحة 
          مع إضافة overflow-auto للجدول نفسه فقط في حال كان عدد الأعمدة ضخم جداً.
      */}
      <section className=" bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
        <h2 className="sr-only">{title} Data List</h2>

        {isLoading && (
          <div
            className="absolute inset-0 bg-white/60 z-20 flex items-center justify-center backdrop-blur-[1px]"
            role="status"
            aria-busy="true"
          >
            <div className="animate-spin rounded-full h-10 w-10 border-4 border-blue-600 border-t-transparent"></div>
          </div>
        )}

        {/* غلاف الجدول الداخلي: يسمح بالسكرول داخل منطقة الجدول فقط إذا لزم الأمر */}
        <div className="overflow-x-auto custom-scrollbar">
          <GenericTable
            columns={columns}
            data={data}
            onEdit={onEdit}
            onDelete={onDeleteAction}
            actions={actions}
            deleteMessage={deleteMessage}
            emptyMessage={emptyMessage}
            renderExtraActions={renderExtraActions}
          />
        </div>
      </section>

      <footer className="mt-8 flex justify-center pb-10">
        {totalPages > 1 && (
          <nav aria-label="Pagination">
            <Pagination
              current={currentPage}
              total={totalPages}
              onPageChange={handlePageChange}
            />
          </nav>
        )}
      </footer>
    </main>
  );
}

export default GenericListPage;
