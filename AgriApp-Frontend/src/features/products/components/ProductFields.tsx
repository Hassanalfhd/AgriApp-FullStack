import { Input } from "@/shared/components/ui/Input";
import type { FieldErrors, UseFormRegister } from "react-hook-form";
import type { IProductToCreate } from "../types/ProductToCreate";
import type { ICrops } from "@/features/Crops/types/cropsTypes";
import type { ImageFileToAdd } from "@/shared/hooks/images/useImagesManager";

interface Props {
  register: UseFormRegister<IProductToCreate>;
  errors: FieldErrors<IProductToCreate>;
  crops: ICrops[];
  images: ImageFileToAdd[];
  imagePreviews: string[];
  onSelectImages: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onRemoveImage: (index: number) => void;
}

export default function ProductFields({
  register,
  errors,
  crops,
  imagePreviews,
  onSelectImages,
  onRemoveImage,
}: Props) {
  return (
    <div className="space-y-5">
      {/* Name */}
      <Input
        label="Product Name"
        {...register("name")}
        error={errors.name?.message}
      />

      {/* Description */}
      <div className="space-y-1">
        <label className="text-sm text-gray-700">Description</label>
        <textarea
          {...register("description")}
          className="
            w-full p-3 border rounded-xl border-gray-300 
            focus:ring-2 focus:ring-green-400
          "
        />
        {errors.description && (
          <p className="text-red-600 text-sm">{errors.description.message}</p>
        )}
      </div>

      {/* Quantity */}
      <Input
        type="number"
        label="Available Quantity"
        min={1}
        {...register("quantityInStock")}
        error={errors.quantityInStock?.message}
      />

      {/* crop type*/}
      <div className="space-y-1">
        <label className="text-sm text-gray-700">Crop Type</label>
        <select
          {...register("cropTypeId")}
          className="
            w-full p-3 border rounded-xl border-gray-300 
            focus:ring-2 focus:ring-green-400
          "
        >
          <option value={0}>Select Crop Type</option>
          {crops.map((crop) => (
            <option key={crop.id} value={crop.id}>
              {crop.name}
            </option>
          ))}
        </select>
        {errors.cropTypeId && (
          <p className="text-red-600 text-sm">{errors.cropTypeId.message}</p>
        )}
      </div>

      {/* Unit Type*/}
      <Input
        type="number"
        label="Unit Type(kg)"
        {...register("quantityTypeId")}
        error={errors.quantityTypeId?.message}
      />

      {/* Price */}
      <Input
        type="number"
        label="Price"
        {...register("price")}
        error={errors.price?.message}
      />

      {/* Products Images */}
      <div className="space-y-1">
        <label className="text-sm text-gray-700">Product Images</label>
        <input
          type="file"
          accept="image/*"
          multiple
          onChange={onSelectImages}
        />
      </div>

      {/* Views Images */}
      {imagePreviews.length > 0 && (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-3">
          {imagePreviews.map((src, index) => (
            <div
              key={index}
              className="border rounded-lg overflow-hidden relative"
            >
              <img
                src={src}
                loading="lazy"
                className="h-24 w-full object-cover"
              />

              <button
                type="button"
                onClick={() => onRemoveImage(index)}
                className="
                  absolute top-1 right-1 
                  bg-red-500 text-white w-6 h-6 
                  rounded-full text-xs font-bold
                "
              >
                ×
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
