import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import type { IProductToCreate } from "../types/ProductToCreate";
import { getAllCropsThunk } from "@/features/Crops/actThunks/getAllCropsThunk";
import toast from "react-hot-toast";
import { createProductWithImagesThunk } from "../actThunks/createProductWithImagesThunk";
import { useNavigate, useParams } from "react-router-dom";
import ProductFields from "../components/ProductFields";
import AddFormLayout from "@/shared/components/layouts/AddFormLayout";
import { productValidationSchema } from "../validation/productSchema";
import { getProductThunk } from "../actThunks/getProductThunk";
import { updateProductWithImagesThunk } from "../actThunks/updateProductWithImagesThunk";
import { getProductsImagesThunk } from "../actThunks/getProductsImagesThunk";
import { useImagesManager } from "@/shared/hooks/images/useImagesManager";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import { useSaveGuard } from "@/shared/hooks/useSaveGuard";
import { getUserProductsThunk } from "../actThunks/getUserProductsThunk";
import { getAdminProductsThunk } from "../actThunks/productsThunks";

export default function AddProductPage() {
  const { id } = useParams<string>();
  const isEdit = Boolean(id);

  const dispatch = useAppDispatch();
  const token = useAppSelector((s) => s.auth.token);
  const userId = useAppSelector((s) => s.auth.user?.id);
  const role = useAppSelector((s) => s.auth.user?.role);

  const { items: crops } = useAppSelector((s) => s.crops);
  const {
    Data,
    loading,
    selectedItem: product,
    images: productImage,
  } = useAppSelector((s) => s.products);

  const navigate = useNavigate();

  // States
  const {
    previews,
    selectImages,
    hasChanges,
    removeImage,
    newImages,
    deleteIds,
  } = useImagesManager({
    initialImages: productImage,
    getImageSrc: (img) => img.imagePath,
    getImageId: (img) => img.id!,
  });

  const validationSchema = productValidationSchema;
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting, isDirty },
  } = useForm<IProductToCreate>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      name: "",
      description: "",
      quantityInStock: 0,
      cropTypeId: 0,
      price: 0,
      quantityTypeId: 0,
    },
  });

  const { isDisabled } = useSaveGuard({
    isSubmitting,
    isLoading: loading === "pending",
    isDirty,
    hasChanges: hasChanges,
  });

  useEffect(() => {
    if (userId) {
      reset((prev) => ({ ...prev, createdBy: userId }));
    }
  }, [userId, reset]);

  // --------------------------
  // If Edit: Load product
  // --------------------------

  useEffect(() => {
    if (isEdit) {
      const productId = id!;
      dispatch(getProductThunk(productId));
      dispatch(getProductsImagesThunk(productId));
    }
  }, [dispatch, id, isEdit]);

  // --------------------------
  // Fill form when product loaded
  // --------------------------

  useEffect(() => {
    if (isEdit && product) {
      reset({
        name: product?.name,
        description: product?.description,
        quantityInStock: product?.quantityInStock,
        price: product?.price,
        cropTypeId: product?.cropTypeId,
        quantityTypeId: product?.quantityTypeId,
      });
    }
  }, [product, reset, isEdit]);

  // check if the URL is corrected
  useEffect(() => {
    if (isEdit) {
      if (loading === "failed" && !product) {
        toast.error("Product Not Found.");
        navigate(`${ROUTES.ADMIN.PRODUCTS.LIST}`);
        return;
      }

      if (!id || isNaN(+id) || +id <= 0) {
        toast.error("Invalid link");
        navigate(`${ROUTES.ADMIN.PRODUCTS.LIST}`);
      }
    }
  }, [isEdit, loading, id, product, navigate]);
  // Load Crops on mount
  useEffect(() => {
    dispatch(getAllCropsThunk());
  }, [dispatch]);

  //submit form
  const onSubmit = async (data: IProductToCreate) => {
    if (!userId || !token) {
      toast.error("Access denied.");
      toast.remove();
      return;
    }

    try {
      let resultAction;

      if (isEdit) {
        resultAction = await dispatch(
          updateProductWithImagesThunk({
            id: id,
            product: data,
            images: newImages,
            deletedImageIds: deleteIds,
          }),
        );
      } else {
        resultAction = await dispatch(
          createProductWithImagesThunk({ product: data, images: newImages }),
        );
      }

      if (resultAction.meta.requestStatus === "fulfilled") {
        toast.success(
          isEdit
            ? "Product updated successfully."
            : "Product Added successfully.",
        );

        reset({ ...data, createdBy: userId });
        setTimeout(() => {
          navigate(`${ROUTES.ADMIN.PRODUCTS.LIST}`);
        }, 1000);

        if (isEdit && id) {
          dispatch(getProductsImagesThunk(id));
        }

        if (role === "Farmer")
          dispatch(
            getUserProductsThunk({ page: Data.page, pageSize: Data.pageSize }),
          );
        else
          dispatch(
            getAdminProductsThunk({ page: Data.page, pageSize: Data.pageSize }),
          );
      }

      if (createProductWithImagesThunk.rejected.match(resultAction)) {
        toast.error(
          (resultAction.payload as string) || "Failed to add the item.",
        );
      }
      if (updateProductWithImagesThunk.rejected.match(resultAction)) {
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
      title="Add New Product."
      description="Enter new prodcut details."
      isSubmitting={isDisabled}
      onSubmit={handleSubmit(onSubmit)}
    >
      <ProductFields
        register={register}
        errors={errors}
        crops={crops}
        images={newImages}
        imagePreviews={previews}
        onSelectImages={selectImages}
        onRemoveImage={removeImage}
      />
    </AddFormLayout>
  );
}
