import { Input } from "../../../shared/components/ui/Input";
import type { FieldErrors, UseFormRegister } from "react-hook-form";
import type { ICategoryToAddEdit } from "../types/categoriesTypes";

interface Props {
  register: UseFormRegister<ICategoryToAddEdit>;
  errors: FieldErrors<ICategoryToAddEdit>;
  imagePreview?: string;
  onSelectImages: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onRemoveImage: () => void;
}

export default function CategoryFields({
  register,
  errors,
  imagePreview,
  onSelectImages,
  onRemoveImage,
}: Props) {
  return (
    <div className="space-y-5">
      {/* الاسم */}
      <Input
        label="Category Name"
        {...register("name")}
        error={errors.name?.message}
      />
      {/* Category Image */}
      <div className="space-y-1">
        <label className="text-sm text-gray-700">Category Image</label>
        <input
          type="file"
          accept="image/*"
          multiple
          onChange={onSelectImages}
        />
      </div>

      {/* View image */}
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
