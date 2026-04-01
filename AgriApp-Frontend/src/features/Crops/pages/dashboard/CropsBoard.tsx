import type { Column } from "@/shared/components/generic/types/Column";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import GenericListPage from "@/shared/components/generic/ListPage/GenericListPage";
import { useEffect } from "react";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import { useNavigate } from "react-router-dom";
import { getAllCropsThunk } from "../../actThunks/getAllCropsThunk";
import { getAllCategoriesThunk } from "@/features/categories/actThunks/getAllCategoriesThunk";
import type { ICrops } from "../../types/cropsTypes";
import { deleteLocalCrop } from "../../slices/cropsSlice";
import { deleteCropThunk } from "../../actThunks/deleteCropThunk";
import { Fab } from "@/shared/components/ui/Fab";
import Loader from "@/shared/components/ui/Loader";
import { Plus } from "lucide-react";

export default function CropsBoard() {
  const dispatch = useAppDispatch();
  const navigator = useNavigate();

  const { items: crops, loading, error } = useAppSelector((s) => s.crops);

  // جلب البيانات عند دخول الصفحة
  useEffect(() => {
    dispatch(getAllCropsThunk());
    dispatch(getAllCategoriesThunk());
  }, [dispatch]);

  // تعريف الأعمدة
  const columns: Column<ICrops>[] = [
    { key: "name", label: "Crop Name" },
    { key: "categoryName", label: "Category" },
  ];

  // منع اللودر الكبير من الظهور إذا كانت هناك بيانات مخزنة مسبقاً
  if (loading === "pending" && crops.length === 0) {
    return <Loader />;
  }

  if (loading === "failed") {
    return <p className="p-6 text-center text-red-500">{error}</p>;
  }

  return (
    <>
      <Fab
        icon={<Plus />}
        onClick={() => navigator(`${ROUTES.ADMIN.CROPS.ADD}`)}
      />

      <GenericListPage<ICrops>
        title="Crops Management"
        columns={columns}
        data={crops}
        // لا نمرر مصفوفة فلاتر لتعطيل قسم البحث تماماً

        // ربط الحذف بالنظام الجديد
        onDeleteAction={(id) => {
          dispatch(deleteLocalCrop(id));
          dispatch(deleteCropThunk(id));
        }}
        // ربط التعديل
        onEdit={(id) => navigator(`/dashboard/crops/${id}/edit`)}
        // تمرير القيم الافتراضية للخصائص المطلوبة في المكون العام
        filterState={{}}
        onFilterChangeAction={() => {}}
        onSearchAction={() => {}}
        onResetAction={() => {}}
        deleteMessage="Are you sure you want to delete this crop? All associated data will be permanently removed."
        actions={{
          showDelete: true,
          showEdit: true,
          showToggleStatus: false,
        }}
        currentPage={0}
        handlePageChange={() => {}}
        totalPages={0}
      />
    </>
  );
}
