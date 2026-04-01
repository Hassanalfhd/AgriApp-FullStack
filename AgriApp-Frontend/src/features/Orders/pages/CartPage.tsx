import React from "react";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { clearCart, removeFromCart } from "../slices/cartSlice";
import toast from "react-hot-toast";
import { sendOrder } from "../actThunks/sendOrder";

const CartPage: React.FC = () => {
  const dispatch = useAppDispatch();
  const { items, totalAmount } = useAppSelector((state) => state.cart);
  const userId = useAppSelector((state) => state.auth.user?.id);

  
  const handleCheckout = async () => {
    if (items.length === 0) return toast.error("Cart is empty!");
    const resultAction = await dispatch(sendOrder({ items, userId }));
    if (resultAction.meta.requestStatus === "fulfilled") {
      toast.success("Send order successfully.");
      dispatch(clearCart());
    }
  };

  return (
    <div className="container mx-auto p-6 dir-ltr text-left">
      <h1 className="text-3xl font-bold mb-8 text-green-800">Orders Cart 🛒</h1>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* قائمة المنتجات */}
        <div className="lg:col-span-2 space-y-4">
          {items.length === 0 ? (
            <p className="text-gray-500 bg-gray-50 p-10 rounded-xl text-center">
              No Products in cart now..
            </p>
          ) : (
            items.map((item) => (
              <div
                key={item.id}
                className="flex items-center justify-between p-4 border rounded-xl bg-white shadow-sm hover:shadow-md transition"
              >
                <div className="flex items-center gap-4">
                  <img
                    src={item.image ?? ""}
                    alt={item.ItemName}
                    className="w-20 h-20 object-cover rounded-lg"
                  />
                  <div>
                    <h3 className="font-bold text-lg">{item.ItemName}</h3>
                    <p className="text-sm text-gray-500">
                      Farmer Name: {item.farmerName}
                    </p>
                    <p className="text-green-600 font-semibold">
                      {item.price} $
                    </p>
                  </div>
                </div>

                <div className="flex items-center gap-6">
                  <div className="text-center">
                    <span className="block text-xs text-gray-400">الكمية</span>
                    <span className="font-bold">{item.quantity}</span>
                  </div>
                  <button
                    onClick={() => dispatch(removeFromCart(item.id))}
                    className="text-red-500 hover:bg-red-50 p-2 rounded-full"
                  >
                    🗑️
                  </button>
                </div>
              </div>
            ))
          )}
        </div>

        {/* ملخص الطلب */}
        <div className="bg-gray-50 p-6 rounded-2xl h-fit sticky top-4">
          <h2 className="text-xl font-bold mb-4 border-b pb-2">
            Order Summary
          </h2>
          <div className="space-y-3">
            <div className="flex justify-between">
              <span>Items Counts:</span>
              <span className="font-medium">{items.length}</span>
            </div>
            <div className="flex justify-between text-xl font-bold border-t pt-4">
              <span>Total :</span>
              <span className="text-green-700">${totalAmount.toFixed(2)}</span>
            </div>
          </div>

          <button
            onClick={handleCheckout}
            disabled={items.length === 0}
            className={`w-full mt-6 py-3 rounded-xl font-bold text-white shadow-lg transition-all
              ${items.length === 0 ? "bg-gray-300" : "bg-orange-500 hover:bg-orange-600 active:scale-95"}`}
          >
            Comfierm oreder
          </button>

          <button
            onClick={() => dispatch(clearCart())}
            className="w-full mt-3 text-sm text-gray-500 hover:underline"
          >
            remove all items in cart
          </button>
        </div>
      </div>
    </div>
  );
};

export default CartPage;
