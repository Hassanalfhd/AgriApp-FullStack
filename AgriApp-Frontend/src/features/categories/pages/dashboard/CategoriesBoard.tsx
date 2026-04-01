import type { Column } from "@/shared/components/generic/types/Column";
import type { ICategory } from "../../types/categoriesTypes";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import GenericListPage from "@/shared/components/generic/ListPage/GenericListPage";
import { useEffect } from "react";
import { getAllCategoriesThunk } from "../../actThunks/getAllCategoriesThunk";
import { deleteCategoryThunk } from "../../actThunks/deleteCategoryThunk";
import { deleteLocalCategory } from "../../slices/categorySlice"; // تأكد من وجود هذه الأكشنز
import { Fab } from "@/shared/components/ui/Fab";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import { useNavigate } from "react-router-dom";
import Loader from "@/shared/components/ui/Loader";
import { Plus } from "lucide-react";

export default function CategoriesBoard() {
  const dispatch = useAppDispatch();
  const navigator = useNavigate();

  const {
    items: categories,
    loading,
    error,
  } = useAppSelector((s) => s.categories);

  useEffect(() => {
    dispatch(getAllCategoriesThunk());
  }, [dispatch]);

  const columns: Column<ICategory>[] = [
    { key: "name", label: "Category Name" },
  ];

  // Loader فقط عند التحميل الأول
  if (loading === "pending" && categories.length === 0) {
    return <Loader />;
  }

  return (
    <>
      <Fab
        icon={<Plus />}
        onClick={() => navigator(`${ROUTES.ADMIN.CATEGORIES.ADD}`)}
      />

      <GenericListPage<ICategory>
        title="Categories"
        columns={columns}
        data={categories}
        onEdit={(id) => navigator(`/dashboard/categories/${id}/edit`)}
        onDeleteAction={(id) => {
          dispatch(deleteLocalCategory(id));
          dispatch(deleteCategoryThunk(id));
        }}
        deleteMessage="Are you sure you want to delete this category?"
        actions={{
          showDelete: true,
          showEdit: true,
          showToggleStatus: false,
        }}
        filterState={{}}
        onFilterChangeAction={() => {}}
        onSearchAction={() => {}}
        onResetAction={() => {}}
        currentPage={0}
        handlePageChange={() => {}}
        totalPages={0}
      />
    </>
  );
}
