import { useCallback, useEffect, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { Plus } from "lucide-react";

import GenericListPage from "@/shared/components/generic/ListPage/GenericListPage";
import type { Column } from "@/shared/components/generic/types/Column";
import type { FilterField } from "@/shared/components/generic/types/FilterField";
import type { TProducts } from "../../types/IProductsResponse";

import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { Fab } from "@/shared/components/ui/Fab";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import Loader from "@/shared/components/ui/Loader";

import { deleteProductThunk } from "../../actThunks/DeleteProduct";
import { getAdminProductsThunk } from "../../actThunks/productsThunks";
import { getUserProductsThunk } from "../../actThunks/getUserProductsThunk";
import { SearchSuggestionsOfProducts } from "../../actThunks/SearchSuggestionsOfProducts";
import { getProductsFilterThunk } from "../../actThunks/getProductsFilterThunk";

import {
  deleteLocalProduct,
  setFilter,
  resetFilters,
} from "../../slices/productSlice";
import { formatDate } from "@/shared/utils/formatDate";

export default function ProductsBoard() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const {
    loading,
    currentPage,
    Data,
    filters: productFiltersState,
    suggestions,
    error,
  } = useAppSelector((state) => state.products);

  const { items: products, pageSize, totalPages } = Data;
  const userRole = useAppSelector((s) => s.auth.user?.role);

  const getProducts = useCallback(
    (newPage: number) => {
      const params = { page: newPage, pageSize };
      if (userRole === "Admin") {
        dispatch(getAdminProductsThunk(params));
      } else if (userRole === "Farmer") {
        dispatch(getUserProductsThunk(params));
      }
    },
    [dispatch, userRole, pageSize],
  );

  useEffect(() => {
    getProducts(currentPage);
  }, [getProducts, currentPage]);

  const handlePageChange = (newPage: number) => {
    getProducts(newPage);
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  // --- التحسين هنا: تحديد عرض الأعمدة لضمان عدم حدوث تداخل ---
  const columns = useMemo<Column<TProducts>[]>(
    () => [
      {
        key: "name",
        label: "Product Name",
        // جعل اسم المنتج يأخذ مساحة أكبر لملء الفراغ الأفقي
        render: (row) => (
          <span className="font-semibold text-gray-900 block min-w-[150px]">
            {row.name}
          </span>
        ),
      },
      {
        key: "price",
        label: "Price",
        render: (row) => (
          <span className="text-blue-600 font-bold whitespace-nowrap">
            ${row.price.toLocaleString()}
          </span>
        ),
      },
      { key: "quantityInStock", label: "Stock" },
      {
        key: "createdAt",
        label: "Date Added",
        render: (row) => (
          <span className="text-gray-500 text-sm whitespace-nowrap">
            {formatDate(row.createdAt!)}
          </span>
        ),
      },
    ],
    [],
  );

  const filterConfig = useMemo<FilterField[]>(
    () => [
      {
        key: "search",
        label: "Search Products",
        type: "text",
        placeholder: "Search by name...",
        searchMode: "change",
        suggestions: suggestions,
        isLoading: loading === "pending",
      },
      { key: "minPrice", label: "Min Price", type: "number", placeholder: "0" },
      {
        key: "maxPrice",
        label: "Max Price",
        type: "number",
        placeholder: "5000",
      },
    ],
    [suggestions, loading],
  );

  if (loading === "pending" && !products.length) return <Loader />;

  return (
    // التحسين: max-w-full مع w-full لضمان التمدد الصحيح
    <div className="w-full max-w-[1400px] mx-auto px-2 sm:px-4 lg:px-6 relative min-h-screen">
      <Fab
        icon={<Plus size={24} />}
        onClick={() => navigate(ROUTES.ADMIN.PRODUCTS.ADD)}
        aria-label="Add New Product"
      />

      {error && (
        <div
          role="alert"
          className="mb-4 p-4 bg-red-50 border-l-4 border-red-500 text-red-700 rounded shadow-sm"
        >
          {error}
        </div>
      )}

      {/* التحسين: نغلف المكون بـ container يمنع الـ overflow غير المرغوب فيه 
          ويجبر الجدول على استخدام الـ Flexbox المتاح
      */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
        <GenericListPage<TProducts>
          title={`${userRole} Inventory`}
          columns={columns}
          filters={filterConfig}
          data={products}
          filterState={productFiltersState}
          onEdit={(id) => navigate(`${ROUTES.ADMIN.PRODUCTS.LIST}/${id}/edit`)}
          onDeleteAction={(id) => {
            dispatch(deleteLocalProduct(id));
            dispatch(deleteProductThunk(id))
              .unwrap()
              .catch(() => getProducts(currentPage));
          }}
          onFilterChangeAction={(key, value) => {
            dispatch(setFilter({ name: key, value }));
            if (key === "search" && value.length > 2) {
              dispatch(SearchSuggestionsOfProducts(value));
            }
          }}
          onSearchAction={() => dispatch(getProductsFilterThunk())}
          onResetAction={() => {
            dispatch(resetFilters());
            getProducts(1);
          }}
          deleteMessage="This action cannot be undone. Are you sure?"
          emptyMessage="No products match your criteria."
          actions={{
            showDelete: true,
            showEdit: true,
            showToggleStatus: false,
          }}
          totalPages={totalPages}
          currentPage={currentPage}
          handlePageChange={handlePageChange}
        />
      </div>
    </div>
  );
}
