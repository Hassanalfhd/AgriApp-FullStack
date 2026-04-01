import placeholder from "../../../assets/images/placeholder.jpg";
import { Link } from "react-router-dom";
import type { ICrops } from "../types/cropsTypes";

type CropProps = ICrops;

export default function CropCard({
  id,
  name,
  categoryName,
  imagePath,
}: CropProps) {
  return (
    <div
      className="
      bg-white rounded-2xl shadow-md 
      hover:shadow-lg transition 
      overflow-hidden h-full
    "
    >
      <Link to={`/categories/${id}`} className="block">
        {/* Image Wrapper */}
        <div
          className="
            relative w-full 
            aspect-square 
            sm:aspect-[4/3] 
            lg:aspect-[16/10]
            bg-gray-100
          "
        >
          <img
            src={imagePath || placeholder}
            alt={name}
            loading="lazy"
            className="absolute inset-0 w-full h-full object-cover"
          />
        </div>

        {/* Content */}
        <div className="p-3 sm:p-4 text-center">
          <h3 className="font-semibold text-base sm:text-lg text-gray-800 line-clamp-1">
            {name}
          </h3>

          <p className="text-sm sm:text-base text-gray-500 line-clamp-1 mt-1">
            {categoryName}
          </p>
        </div>
      </Link>
    </div>
  );
}
