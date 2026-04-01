import AddFormLayout from "@/shared/components/layouts/AddFormLayout";
import CategoryFields from "../../components/CategoryFields";
import * as Yup from "yup";
import type { ICategoryToAddEdit } from "../../types/categoriesTypes";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useSaveGuard } from "@/shared/hooks/useSaveGuard";
import { useNavigate, useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import toast from "react-hot-toast";
import { createNewCategoryThunk } from "../../actThunks/createNewCategoryThunk";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import { updateCagegoryThunk } from "../../actThunks/updateCagegoryThunk";
import { useEffect } from "react";
import { getCategoryByIdThunk } from "../../actThunks/getCategoryByIdThunk";
import { useSingleImage } from "@/shared/hooks/images/userSingleImageHandler";
import { useAuth } from "@/shared/hooks/useAuth";

export default function AddEditCategory() {
  const validationSchema = Yup.object().shape({
    name: Yup.string().required("Category name is required."),
  });

  const { id } = useParams<string>();
  const isEdit = Boolean(id);

  const dispatch = useAppDispatch();
  const userId = useAppSelector((s) => s.auth.user?.id);

  const isAuth = useAuth();
  const { loading, selectedItem: category } = useAppSelector(
    (s) => s.categories,
  );

  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting, isDirty },
  } = useForm<ICategoryToAddEdit>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      name: "",
      imageFile: undefined,
    },
  });

  // States

  const { hasChange, imageFile, removeImage, preview, selectImage } =
    useSingleImage(category?.imageFile);

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
  // If Edit: Load product
  // --------------------------
  useEffect(() => {
    if (isEdit) {
      const productId = id!;
      dispatch(getCategoryByIdThunk(productId));
    }
  }, [dispatch, id, isEdit]);

  // --------------------------
  // Fill form when product loaded
  // --------------------------

  useEffect(() => {
    if (isEdit && category) {
      reset({
        name: category?.name,
      });
      return;
    }
  }, [category, reset, isEdit]);

  // check if the URL is corrected
  useEffect(() => {
    if (isEdit) {
      if (loading === "failed" && !category) {
        toast.error("Category not found.");
        navigate(`${ROUTES.ADMIN.CATEGORIES.LIST}`);
        return;
      }

      if (!id || isNaN(+id) || +id <= 0) {
        toast.error("Invalid link.");
        navigate(`${ROUTES.ADMIN.CATEGORIES.LIST}`);
      }
    }
  }, [isEdit, loading, id, category, navigate]);

  const onSubmit = async (data: ICategoryToAddEdit) => {
    if (!userId || !isAuth) {
      toast.error("Access denied.");
      toast.remove();
      return;
    }

    try {
      if (imageFile) {
        data.imageFile = imageFile;
        console.log("Image File ", imageFile);
        console.log("Image File 3333 ", data.imageFile);
      }

      let resultAction;

      if (isEdit) {
        resultAction = await dispatch(updateCagegoryThunk({ id, data }));
      } else {
        resultAction = await dispatch(createNewCategoryThunk(data));
      }

      if (resultAction.meta.requestStatus === "fulfilled") {
        toast.success(
          isEdit
            ? "Category updated successfully"
            : "Category added successfully.",
        );
        reset({ ...data });
        setTimeout(() => {
          navigate(`${ROUTES.ADMIN.CATEGORIES.LIST}`);
        }, 1000);
      }

      if (createNewCategoryThunk.rejected.match(resultAction)) {
        toast.error(
          (resultAction.payload as string) || "Failed to add the item.",
        );
      }

      if (updateCagegoryThunk.rejected.match(resultAction)) {
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
      title="Add New Category"
      description="Enter new category details."
      isSubmitting={isDisabled}
      onSubmit={handleSubmit(onSubmit)}
    >
      <CategoryFields
        register={register}
        errors={errors}
        imagePreview={preview!}
        onRemoveImage={removeImage}
        onSelectImages={selectImage}
      ></CategoryFields>
    </AddFormLayout>
  );
}
