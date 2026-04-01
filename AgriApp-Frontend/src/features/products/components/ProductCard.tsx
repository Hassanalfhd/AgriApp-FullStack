import { Link } from "react-router-dom";
import type { TProducts } from "../types/IProductsResponse";
import placeholder from "@/assets/images/placeholder.jpg";
import UserIcon from "@/shared/components/ui/UserIcon";
import { formatDate } from "@/shared/utils/formatDate";
import { formatCurrency } from "@/shared/utils/formatCurrency";

type ProductCardProps = TProducts & { onAddToCart?: () => void } & {
  user?: { id: number; username: string; imageFile?: string };
};

export default function ProductCard({
  id,
  name,
  price,
  image,
  description,
  quantityName,
  createdAt,
  createdByName,
  createdByImage,
  createdBy,
  onAddToCart,
}: ProductCardProps) {
  return (
    <div
      className="
        bg-white rounded-2xl shadow-md hover:shadow-lg 
        transition-all duration-200 overflow-hidden 
        flex flex-col h-full
      "
    >
      {/* 🖼️ الصورة */}

      <div className="flex items-center justify-between">
        <p className="text-xs text-green-600">{formatDate(createdAt!)}</p>
        <UserIcon
          userId={createdBy}
          userImage={createdByImage}
          userName={createdByName}
        />
      </div>

      <Link to={`${id}`} className="block relative w-full h-48 bg-gray-100">
        <img
          src={image || placeholder}
          alt={name}
          loading="lazy"
          className="w-full h-full object-cover blur-sm transition  duration-500 ease-out hover:scale-105"
          onLoad={(e) => e.currentTarget.classList.remove("blur-sm")}
          onError={(e) => (e.currentTarget.src = placeholder)}
        />
      </Link>

      {/* 📄 التفاصيل */}
      <div className="p-4 flex flex-col flex-grow">
        <h3 className="font-semibold text-lg text-gray-800 mb-1 line-clamp-1">
          {name}
        </h3>

        <p className="text-sm text-gray-500 mb-2 line-clamp-2">
          {description || "No description."}
        </p>

        {/* 💲 السعر والوحدة */}
        <div className="mt-auto flex items-center justify-between">
          <span className="text-green-600 font-bold text-lg">
            {formatCurrency(price)}
            {quantityName && (
              <span className="text-sm text-gray-500 ms-1">
                / {quantityName}
              </span>
            )}
          </span>

          {/* 🎯 زر إجراء */}
          {onAddToCart ? (
            <button
              onClick={onAddToCart}
              className="
                bg-green-600 text-white px-3 py-1.5 rounded-md text-sm 
                hover:bg-green-700 transition
              "
            >
              Add To Cart
            </button>
          ) : (
            <Link
              to={`${id}`}
              className="text-green-700 text-sm font-medium hover:underline"
            >
              details
            </Link>
          )}
        </div>
      </div>
    </div>
  );
}
