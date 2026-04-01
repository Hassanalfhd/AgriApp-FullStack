import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { getProductThunk } from "../actThunks/getProductThunk";
import Loader from "@/shared/components/ui/Loader";
import toast from "react-hot-toast";
import ProductDetails from "../components/ProductDetails";

export default function ProductDetailsPage() {
  const dispatch = useAppDispatch();
  const { selectedItem, loading, error, lastFetchedProductId } = useAppSelector(
    (state) => state.products,
  );

  const { productId } = useParams<{ productId: string }>();

  useEffect(() => {
    if (!productId) {
      toast.error("Invalid product ID");
      return;
    }

    if (lastFetchedProductId !== productId) {
      dispatch(getProductThunk(productId))
        .unwrap()
        .then(() => toast.success("Product loaded!"))
        .catch((err) => toast.error(err));
    }
  }, [dispatch, productId, lastFetchedProductId]);

  if (loading === "pending") return <Loader />;

  if (error)
    return <p className="text-center text-red-500 mt-8">Error: {error}</p>;

  if (!selectedItem)
    return <p className="text-center text-gray-500 mt-8">No product found.</p>;

  return <ProductDetails product={selectedItem} />;
}
