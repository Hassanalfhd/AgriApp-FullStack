import type { ImageFileToAdd } from "@/shared/hooks/images/useImagesManager";
import apiClient from "../../../shared/services/apiClient";

/**
 * رفع صور المنتج مع ترتيبها
 * @param images مصفوفة صور مع ترتيب
 * @returns روابط الصور بعد الرفع بالترتيب نفسه
 */
export async function uploadImagesService(
  images: ImageFileToAdd[],
  productId: number
): Promise<string[]> {
  if (!images || images.length === 0) return [];

  // ترتيب الصور حسب imageOrder قبل الرفع
  const sortedImages = images.sort((a, b) => a.imageOrder! - b.imageOrder!);

  const formData = new FormData();
  sortedImages.forEach((img) => formData.append("files", img.imageFile));
  try {
    const response = await apiClient.post(
      `v1/products/images/upload-multiple?productId=${productId}`,
      formData
    );

    return response.data;
  } catch (error: any) {
    console.error("Upload Images Error:", error);
    throw new Error(error.response?.data?.message || "Error uploading images");
  }
}
