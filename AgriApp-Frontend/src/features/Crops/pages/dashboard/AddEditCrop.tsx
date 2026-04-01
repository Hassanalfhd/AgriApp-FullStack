import AddFormLayout from "@/shared/components/layouts/AddFormLayout";
import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useSaveGuard } from "@/shared/hooks/useSaveGuard";
import { useNavigate, useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import toast from "react-hot-toast";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import { useEffect } from "react";
import { useSingleImage } from "@/shared/hooks/images/userSingleImageHandler";
import type { ICrops } from "../../types/cropsTypes";
import { getCropByIdThunk } from "../../actThunks/getCropByIdThunk";
import { updateCropThunk } from "../../actThunks/updateCropThunk";
import { addCropThunk } from "../../actThunks/addCropThunk";
import { getAllCategoriesThunk } from "@/features/categories/actThunks/getAllCategoriesThunk";
import CropFields from "../../components/CropFields";

export default function AddEditCategory() {
  const validationSchema = Yup.object().shape({
    name: Yup.string().required("Crop name is required."),
    categoryId: Yup.number().required("Category name is required."),
  });
  const { id } = useParams<string>();
  const isEdit = Boolean(id);

  const dispatch = useAppDispatch();
  const token = useAppSelector((s) => s.auth.token);
  const userId = useAppSelector((s) => s.auth.user?.id);

  const { loading, selectedItem: crop } = useAppSelector((s) => s.crops);

  const { items: categories } = useAppSelector((s) => s.categories);

  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting, isDirty },
  } = useForm<ICrops>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      name: "",
      ownerId: 0,
      categoryId: 0,
    },
  });

  // States
  const { hasChange, imageFile, removeImage, preview, selectImage } =
    useSingleImage(crop?.imagePath);

  const { isDisabled } = useSaveGuard({
    isSubmitting,
    isLoading: loading === "pending",
    isDirty,
    hasChanges: hasChange,
  });

  useEffect(() => {
    if (userId) {
      reset((prev) => ({ ...prev }));
    }
  }, [userId, reset]);

  // --------------------------
  //   get Categories
  // --------------------------
  useEffect(() => {
    if (!categories.length) dispatch(getAllCategoriesThunk());
  }, [dispatch, categories.length]);

  // --------------------------
  // If Edit: Load crop
  // --------------------------
  useEffect(() => {
    if (isEdit) {
      const productId = id!;
      dispatch(getCropByIdThunk(productId));
    }
  }, [dispatch, id, isEdit]);

  // --------------------------
  // Fill form when crop loaded
  // --------------------------

  useEffect(() => {
    console.log("Crop from store:", crop);
  }, [crop]);
  useEffect(() => {
    if (isEdit && crop) {
      reset({
        id: crop.id,
        name: crop?.name,
        categoryId: crop?.categoryId,
      });
      return;
    }
  }, [crop, reset, isEdit]);

  // check if the URL is corrected
  useEffect(() => {
    if (isEdit) {
      if (loading === "failed" && !crop) {
        toast.error("Crop not found.");
        navigate(`${ROUTES.ADMIN.CROPS.LIST}`);
        return;
      }

      if (!id || isNaN(+id) || +id <= 0) {
        toast.error("Invalid link.");
        navigate(`${ROUTES.ADMIN.CROPS.LIST}`);
      }
    }
  }, [isEdit, loading, id, crop, navigate]);

  const onSubmit = async (data: ICrops) => {
    if (!userId || !token) {
      toast.error("Access denied.");
      toast.remove();
      return;
    }

    try {
      if (imageFile) {
        data.imageFile = imageFile;
      }

      let resultAction;

      if (isEdit) {
        resultAction = await dispatch(updateCropThunk({ id, data }));
      } else {
        resultAction = await dispatch(addCropThunk(data));
      }

      if (resultAction.meta.requestStatus === "fulfilled") {
        toast.success(
          isEdit ? "Crop updated successfully." : "Crop added successfully",
        );
        reset({ ...data });
        setTimeout(() => {
          navigate(`${ROUTES.ADMIN.CROPS.LIST}`);
        }, 1000);
      }

      if (addCropThunk.rejected.match(resultAction)) {
        toast.error(
          (resultAction.payload as string) || "Failed to add the item.",
        );
      }

      if (updateCropThunk.rejected.match(resultAction)) {
        toast.error(
          (resultAction.payload as string) || "Failed to update the item.",
        );
      }
    } catch (err) {
      toast.error("An unexpected error occurred.");
      console.error(err);
    }

    toast.remove();
  };
  return (
    <AddFormLayout
      title="Add New Crop"
      description="Enter new crop details."
      isSubmitting={isDisabled}
      onSubmit={handleSubmit(onSubmit)}
    >
      <CropFields
        register={register}
        categories={categories}
        errors={errors}
        imagePreview={preview!}
        onRemoveImage={removeImage}
        onSelectImages={selectImage}
      />
    </AddFormLayout>
  );
}
