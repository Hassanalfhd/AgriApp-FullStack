import { createAsyncThunk } from "@reduxjs/toolkit";
import type { IProductToCreate } from "../types/ProductToCreate";
import { uploadImagesService } from "../services/uploadImagesService";
import { createProductService } from "../services/createProductService";
import type { RootState } from "@/app/store";
import type { ImageFileToAdd } from "@/shared/hooks/images/useImagesManager";

export interface IPayload {
  product: Omit<IProductToCreate, "createdBy">;
  images: ImageFileToAdd[];
}

export const createProductWithImagesThunk = createAsyncThunk(
  "products/createProduct",
  async ({ product, images }: IPayload, { rejectWithValue, getState }) => {
    try {
      const state = getState() as RootState;

      const token = state.auth.token;
      const userId = state.auth.user?.id;

      if (!token || !userId) return rejectWithValue("User not authenticated");

      const productToCreate: IProductToCreate = {
        ...product,
        createdBy: userId,
      };

      const newProduct = await createProductService(productToCreate);

      if (images && images.length > 0) {
        await uploadImagesService(images, newProduct.id);
      }

      return newProduct;
    } catch (err: any) {
      console.error("Create Product Error:", err);
      return rejectWithValue(err.response?.data || "Error creating product");
    }
  }
);
