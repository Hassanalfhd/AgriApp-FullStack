// --------------------------
// Sub-components
// --------------------------

import UserIcon from "@/shared/components/ui/UserIcon";
import { formatDate } from "@/shared/utils/formatDate";
import ProductImages from "./ProductImages";
import type { TProduct } from "../types/IProductsResponse";
import AddToCartButton from "@/shared/components/ui/AddToCartButton";
import { usePermissionToDashoard } from "@/shared/hooks/usePermissionToDashoard";
import { useAuth } from "@/shared/hooks/useAuth";
import { formatCurrency } from "@/shared/utils/formatCurrency";

export interface ProductDetailsProps {
  product: TProduct;
}

function ProductDetails({ product }: ProductDetailsProps) {
  const isHavePernissionToDashboard = usePermissionToDashoard();
  const isAuth = useAuth();

  return (
    <article
      className="max-w-7xl mx-auto p-4 sm:p-6 md:p-12 grid grid-cols-1 md:grid-cols-2 gap-8"
      aria-label={`Product Details: ${product.name}`}
    >
      {/* ===== Left Column: Images ===== */}
      <section>
        <ProductImages images={product.images} name={product.name} />
      </section>

      {/* ===== Right Column: Info ===== */}
      <section className="flex flex-col gap-6">
        <h1 className="text-3xl sm:text-2xl font-bold text-gray-900">
          {product.name}
        </h1>

        <div className="flex items-center justify-between">
          <p className="text-xs text-green-600">
            {product.createdAt && formatDate(product.createdAt)}
          </p>
          <UserIcon
            userId={product.createdBy}
            userImage={product.createdByImage}
            userName={product.createdByName}
          />
        </div>

        <div className="flex flex-col sm:flex-row sm:justify-between sm:items-center gap-4">
          <span className="text-2xl sm:text-xl font-semibold text-green-600">
            {formatCurrency(product.price)}
          </span>
        </div>

        {product.quantityInStock !== undefined && (
          <p className="text-gray-600 font-medium">
            In Stock: {product.quantityInStock} {product.quantityName || ""}
          </p>
        )}

        <div className="prose max-w-full mt-4">
          <h2 className="text-xl font-semibold mt-2">Description</h2>
          <p>{product.description || "No description available."}</p>
        </div>

        <dl className="grid grid-cols-2 gap-4 mt-4">
          {product.cropName && (
            <>
              <dt className="font-semibold text-gray-700">Crop Type</dt>
              <dd className="text-gray-600">{product.cropName}</dd>
            </>
          )}
          {product.quantityName && (
            <>
              <dt className="font-semibold text-gray-700">Quantity Name</dt>
              <dd className="text-gray-600">{product.quantityName}</dd>
            </>
          )}
        </dl>

        {!isHavePernissionToDashboard && isAuth && (
          <AddToCartButton
            product={{
              farmerName: product.createdByName ?? "",
              id: product.id,
              image: product.images[0],
              ItemName: product.name,
              price: product.price,
              productId: product.id,
              quantity: product.quantityInStock ?? 0,
              status: 1,
            }}
          />
        )}
      </section>
    </article>
  );
}

export default ProductDetails;
