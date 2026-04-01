import type { RootState } from "@/app/store";
import { createAsyncThunk } from "@reduxjs/toolkit";
import type { IProductToCreate } from "../types/ProductToCreate";
import { uploadImagesService } from "../services/uploadImagesService";
import type { TProducts } from "../types/IProductsResponse";
import { apiCrudService } from "@/shared/services/apiCrudService";
import type { ImageFileToAdd } from "@/shared/hooks/images/useImagesManager";

interface UpdatePayload {
  id: number | string;
  product: Omit<IProductToCreate, "createdBy">;
  images: ImageFileToAdd[];
  deletedImageIds: number[];
}

export const updateProductWithImagesThunk = createAsyncThunk(
  "products/updateProductWithImages",
  async (
    { id, product, images, deletedImageIds }: UpdatePayload,
    { rejectWithValue, getState }
  ) => {
    try {
      const state = getState() as RootState;
      const token = state.auth.token;
      const userId = state.auth.user?.id;

      const productId = typeof id === "string" ? parseInt(id) : id;

      product.id = productId;
      if (!token) return rejectWithValue("غير مصرح");

      const productToUpdate: IProductToCreate = {
        ...product,
        createdBy: userId!,
      };

      if (!product) return rejectWithValue("خطا في بيانات المنتح!");
      const updatedProduct = await apiCrudService.update<
        IProductToCreate,
        TProducts
      >("v1/products/Update", productToUpdate);

      // Delete the olde images

      if (deletedImageIds && deletedImageIds.length > 0) {
        await Promise.all(
          deletedImageIds.map((dImg) =>
            apiCrudService.delete(`v1/products/images/Delete`, dImg)
          )
        );
      }

      if (images && images.length > 0) {
        await uploadImagesService(images, product!.id ?? 0);
      }

      return updatedProduct;
    } catch (err: any) {
      console.error("Update product error:", err);

      return rejectWithValue(err.response?.data || "حدث خطأ أثناء التعديل");
    }
  }
);
