import { addToCart } from "@/features/Orders/slices/cartSlice";
import type { CartItems } from "@/features/Orders/types/cartDto";
import { useAppDispatch } from "@/shared/hooks/StoreHook";
import React from "react";
import toast from "react-hot-toast";

interface Props {
  product: CartItems;
}

const AddToCartButton: React.FC<Props> = ({ product }) => {
  const dispatch = useAppDispatch();

  const handleAddToCart = () => {
    dispatch(addToCart(product));
    toast.success("Added to cart successflly");
  };

  
  return (
    <button
      onClick={handleAddToCart}
      className="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-6 rounded-lg 
                 transition duration-200 ease-in-out transform active:scale-95 flex items-center gap-2"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        className="h-5 w-5"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth={2}
          d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"
        />
      </svg>
      Add TO Cart
    </button>
  );
};

export default AddToCartButton;
