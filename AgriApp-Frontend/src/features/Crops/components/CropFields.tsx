import { Input } from "../../../shared/components/ui/Input";
import type { FieldErrors, UseFormRegister } from "react-hook-form";
import type { ICategory } from "@/features/categories/types/categoriesTypes";
import type { ICrops } from "../types/cropsTypes";

interface Props {
  register: UseFormRegister<ICrops>;
  errors: FieldErrors<ICrops>;
  imagePreview?: string;
  categories: ICategory[];
  onSelectImages: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onRemoveImage: () => void;
}

export default function CropFields({
  register,
  errors,
  imagePreview,
  onSelectImages,
  categories,
  onRemoveImage,
}: Props) {
  return (
    <div className="space-y-5">
      {/* الاسم */}
      <Input
        label="Crop Name"
        {...register("name")}
        error={errors.name?.message}
      />

      {/* نوع القسم */}
      <div className="space-y-1">
        <label className="text-sm text-gray-700">Category Type</label>
        <select
          {...register("categoryId")}
          className="
            w-full p-3 border rounded-xl border-gray-300 
            focus:ring-2 focus:ring-green-400
          "
        >
          <option value={0}>Select Category Type</option>
          {categories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
        {errors.categoryId && (
          <p className="text-red-600 text-sm">{errors.categoryId.message}</p>
        )}
      </div>

      {/* Crop Image */}
      <div className="space-y-1">
        <label className="text-sm text-gray-700">Crop Image</label>
        <input
          type="file"
          accept="image/*"
          multiple
          onChange={onSelectImages}
        />
      </div>

      {/* view iamge */}
      {imagePreview && (
        <div className="relative w-32 h-32 border rounded-lg overflow-hidden">
          <img
            src={imagePreview}
            loading="lazy"
            className="h-full w-full object-cover"
          />

          <button
            type="button"
            onClick={onRemoveImage}
            className="
              absolute top-1 right-1 
              bg-red-500 text-white w-6 h-6 
              rounded-full text-xs font-bold
            "
          >
            ×
          </button>
        </div>
      )}
    </div>
  );
}
