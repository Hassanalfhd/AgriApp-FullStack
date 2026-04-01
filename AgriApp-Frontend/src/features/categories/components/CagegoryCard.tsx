import type { ICategory } from "../types/categoriesTypes";
import placeholder from "../../../assets/images/placeholder.jpg";
import { Link } from "react-router-dom";

type CategoryProps = ICategory;

export default function CategoryCard({ id, name, imageFile }: CategoryProps) {
  return (
    <div className="bg-white rounded-2xl shadow-md hover:shadow-lg transition overflow-hidden h-full">
      <Link to={`/categories/${id}`} className="block">
        {/* Image Wrapper  */}
        <div className="relative w-full aspect-square bg-gray-100">
          <img
            src={imageFile || placeholder}
            alt={name}
            loading="lazy"
            className="absolute inset-0 w-full h-full object-cover rounded-2xl"
          />
        </div>

        {/* Content */}
        <div className="p-4 text-center">
          <h3 className="font-semibold text-lg text-gray-800 line-clamp-1">
            {name}
          </h3>
        </div>
      </Link>
    </div>
  );
}
